using System;
using Data;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Parameter;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
	public class HeroPicker : MonoBehaviour
	{
		public int HeroID
		{
			get
			{
				return heroID;
			}
			set
			{
				heroID = value;
			}
		}

		private void Awake()
		{
			button = base.GetComponent<Button>();
			imageButton = base.GetComponent<Image>();
		}

		public void Init(int heroID)
		{
			HeroID = heroID;
			InitHeroAvatar(heroID);
			int currentHeroLevel = HeroStore.Instance.GetCurrentHeroLevel(heroID);
			heroSpawnTime = (float)HeroParameterManager.Instance.GetHeroParameter(heroID, currentHeroLevel).respawn_time / 1000f;
			ViewEnable();
			gameEventSubscriberId = GameKit.GetUniqueId();
			GameSignalCenter.Instance.Subscribe(GameSignalKind.OnSelectHero, new SelectPersonaListenerRecord(gameEventSubscriberId, new GameSignalCenter.SelectCharacterMethod(HandleSelectingHero)));
			GameSignalCenter.Instance.Subscribe(GameSignalKind.OnClickButton, new TapSwitchListenerRecord(gameEventSubscriberId, new GameSignalCenter.ClickButtonMethod(OnAButtonClicked)));
		}

		private void OnDestroy()
		{
			GameSignalCenter.Instance.Unsubscribe(gameEventSubscriberId);
		}

		private void InitHeroAvatar(int heroID)
		{
			imageButton.sprite = Common.AssetLoader.Load<Sprite>(string.Format("Gameplay-HeroIcon/mini_icon_hero_{0}", heroID));
			imageCooldown.sprite = Common.AssetLoader.Load<Sprite>(string.Format("Gameplay-HeroIcon/mini_icon_hero_{0}", heroID));
		}

		public void UpdateHealthBar(int currentHealth, int originHealth)
		{
			float num = (float)originHealth / (maxHealthBarValue - minHealthBarValue);
			float num2 = (float)currentHealth / num;
			healthBarSize.Set(num2 + minHealthBarValue, healthBar.sizeDelta.y);
			healthBar.sizeDelta = healthBarSize;
		}

		public void Refresh()
		{
			if (HerosDirector.Instance.HeroIDChoosing == HeroID)
			{
				ViewSelected();
			}
			else
			{
				HideSelected();
			}
		}

		public void OnClick()
		{
			GameSignalCenter.Instance.Trigger(GameSignalKind.OnSelectHero, HeroID);
			GameSignalCenter.Instance.Trigger(GameSignalKind.OnClickButton, new TappedObjectRecord(TappedObjectKind.HeroIconBtn));
		}

		public void HandleSelectingHero(int heroId)
		{
			if (heroId != HeroID)
			{
				return;
			}
			UISfxDirector.Instance.PlayClick();
			toggle = !toggle;
			if (toggle)
			{
				HerosDirector.Instance.ChooseHero(HeroID);
			}
			else
			{
				HerosDirector.Instance.UnChooseHero(HeroID);
			}
			Refresh();
		}

		public void OnAButtonClicked(TappedObjectRecord clickedObjData)
		{
			if (clickedObjData.clickedObjType == TappedObjectKind.HeroIconBtn)
			{
				return;
			}
			if (HeroID == HerosDirector.Instance.HeroIDChoosing)
			{
				HandleSelectingHero(HeroID);
			}
		}

		public void DoCooldown()
		{
			DOTween.To(() => 0f, delegate(float x)
			{
				imageCooldown.fillAmount = x;
			}, 1f, heroSpawnTime).SetEase(Ease.Linear).OnComplete(new TweenCallback(CooldownComplete));
			imageCooldown.gameObject.SetActive(true);
		}

		private void CooldownComplete()
		{
			ViewEnable();
			imageCooldown.gameObject.SetActive(false);
		}

		private void ViewSelected()
		{
			selectedImage.SetActive(true);
			MonoSingleton<GameplayUIHeroDirector>.Instance.HeroCurrentLevelInformationPopup.Init(HeroID, HeroStore.Instance.GetCurrentHeroLevel(HeroID));
		}

		public void HideSelected()
		{
			selectedImage.SetActive(false);
			MonoSingleton<GameplayUIHeroDirector>.Instance.HeroCurrentLevelInformationPopup.Close();
			toggle = false;
		}

		public void ViewEnable()
		{
			button.enabled = true;
			imageButton.color = Color.white;
		}

		public void ViewDisable()
		{
			button.enabled = false;
			imageButton.color = Color.gray;
		}

		[SerializeField]
		private int heroID;

		public float heroSpawnTime;

		private Button button;

		private Image imageButton;

		[SerializeField]
		private Image imageCooldown;

		[SerializeField]
		private GameObject selectedImage;

		private bool toggle;

		[Space]
		[Header("Hero health bar")]
		[SerializeField]
		private RectTransform healthBar;

		private Vector2 healthBarSize;

		[SerializeField]
		private float maxHealthBarValue;

		[SerializeField]
		private float minHealthBarValue;

		private int gameEventSubscriberId;
	}
}
