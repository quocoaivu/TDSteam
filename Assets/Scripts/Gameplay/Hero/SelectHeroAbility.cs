using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Common;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
	public class SelectHeroAbility : MonoBehaviour
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
		}

		private void Start()
		{
			gameEventSubscriberId = GameKit.GetUniqueId();
			GameSignalCenter.Instance.Subscribe(GameSignalKind.OnClickButton, new TapSwitchListenerRecord(gameEventSubscriberId, new GameSignalCenter.ClickButtonMethod(HandleButtonClicked)));
		}

		private void OnDestroy()
		{
			GameSignalCenter.Instance.Unsubscribe(gameEventSubscriberId, GameSignalKind.OnClickButton);
		}

		public void Init(int heroID)
		{
			InitHeroAvatar(heroID);
			HeroID = heroID;
			cooldownTime = MonoSingleton<AllyPool>.Instance.GetHeroSkillCooldownTime(heroID);
			useTypeValue = MonoSingleton<AllyPool>.Instance.GetHeroSkillUseType(heroID);
			ViewEnable();
		}

		private void InitHeroAvatar(int heroID)
		{
			imageButton.sprite = Common.AssetLoader.Load<Sprite>(string.Format("HeroCamp/SkillIcons/hero_{0}_skill_0", heroID));
			imageCooldown.sprite = Common.AssetLoader.Load<Sprite>(string.Format("HeroCamp/SkillIcons/hero_{0}_skill_0", heroID));
		}

		public void OnClick()
		{
			UISfxDirector.Instance.PlayClick();
			GameSignalCenter.Instance.Trigger(GameSignalKind.OnClickButton, new TappedObjectRecord(TappedObjectKind.HeroSkillBtn, HeroID));
		}

		public void HandleButtonClicked(TappedObjectRecord clickedObjData)
		{
			if (clickedObjData.clickedObjType == TappedObjectKind.HeroSkillBtn && clickedObjData.id == HeroID)
			{
				string text = useTypeValue;
				if (text != null)
				{
					if (!(text == "TapToUse"))
					{
						if (text == "TapNClickToUse")
						{
							toggle = !toggle;
							if (toggle)
							{
								HerosDirector.Instance.ChooseHeroSkill(HeroID);
							}
							else
							{
								HerosDirector.Instance.UnChooseHeroSkill(HeroID);
							}
							Refresh();
						}
					}
					else
					{
						UnityEngine.Debug.Log("3");
					}
				}
			}
			else
			{
				if (HeroID == HerosDirector.Instance.HeroSkillIDChoosing)
				{
					HerosDirector.Instance.UnChooseHeroSkill(HeroID);
				}
				Refresh();
			}
		}

		public void Refresh()
		{
			if (HerosDirector.Instance.HeroSkillIDChoosing == HeroID)
			{
				ViewSelected();
			}
			else
			{
				HideSelected();
			}
		}

		public void CancelSelect()
		{
			HideSelected();
		}

		public void DoCooldown()
		{
			CancelSelect();
			ViewDisable();
			tween = DOTween.To(() => 0f, delegate(float x)
			{
				imageCooldown.fillAmount = x;
			}, 1f, cooldownTime).SetEase(Ease.Linear).OnComplete(new TweenCallback(CooldownComplete));
			imageCooldown.gameObject.SetActive(true);
		}

		public void ViewDisableImmediately()
		{
			tween.Kill(false);
			button.enabled = false;
			imageButton.color = Color.gray;
			imageCooldown.gameObject.SetActive(false);
		}

		public void ViewEnableImmediately()
		{
			tween.Kill(false);
			CooldownComplete();
		}

		private void CooldownComplete()
		{
			ViewEnable();
			imageCooldown.gameObject.SetActive(false);
			OnCooldownDone.Dispatch();
		}

		private void ViewSelected()
		{
			selectedImage.SetActive(true);
			closeImage.SetActive(true);
		}

		public void HideSelected()
		{
			selectedImage.SetActive(false);
			closeImage.SetActive(false);
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
		private OrderedUnityEvent OnCooldownDone;

		private const string TAP_TO_USE = "TapToUse";

		private const string TAP_N_CLICK_TO_USE = "TapNClickToUse";

		private int heroID;

		public float cooldownTime;

		private Button button;

		[SerializeField]
		private Image imageButton;

		[SerializeField]
		private Image imageCooldown;

		[SerializeField]
		private GameObject selectedImage;

		[SerializeField]
		private GameObject closeImage;

		private string useTypeValue;

		private bool toggle;

		private Tweener tween;

		private int gameEventSubscriberId;
	}
}
