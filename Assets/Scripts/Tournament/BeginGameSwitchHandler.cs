using System;
using MapLevel;
using MetaGame;
using Parameter;
using UnityEngine;
using UnityEngine.UI;

namespace Tournament
{
	public class BeginGameSwitchHandler : MonoBehaviour
	{
		private void Awake()
		{
			startButton = base.GetComponent<Button>();
		}

		private void Update()
		{
			if (heroesInputGroupController.HeroesSelectedController.IsChooseAtLeastOneHero())
			{
				SetStartButtonEnable();
			}
			else
			{
				SetStartButtonDisable();
			}
		}

		private void SetStartButtonEnable()
		{
			startButton.enabled = true;
			material.SetFloat("_EffectAmount", 0f);
			textNotification.text = Singleton<AlertSynopsis>.Instance.GetNotiContent(readyContentID);
			textNotification.color = readyColor;
		}

		private void SetStartButtonDisable()
		{
			startButton.enabled = false;
			material.SetFloat("_EffectAmount", 1f);
			textNotification.text = Singleton<AlertSynopsis>.Instance.GetNotiContent(notReadyContentID);
			textNotification.color = notReadyColor;
		}

		private void DoLoadSceneGameplay()
		{
			GameSceneLoader.Instance.LoadScene(GameSceneLoader.GameplaySceneName);
		}

		public void OnClick()
		{
			base.Invoke("OnPrepareToLoad", 0.1f);
			UISfxDirector.Instance.PlayStartGameAtMapLevel();
		}

		private void OnPrepareToLoad()
		{
			LoadingScreen.Instance.ShowLoading();
			base.Invoke("DoLoadSceneGameplay", 0.3f);
			FormatDirector.Instance.gameMode = GameFormat.TournamentMode;
			startGamePopupController.InitListHeroesIDSelected();
			startGamePopupController.SaveListHeroIDSelected();
		}

		[SerializeField]
		private BeginGameDialogHandler startGamePopupController;

		[SerializeField]
		private HerosInputClusterHandler heroesInputGroupController;

		[Space]
		[Header("Image material")]
		[SerializeField]
		private Material material;

		private Button startButton;

		[Space]
		[Header("Text notification")]
		[SerializeField]
		private Text textNotification;

		[SerializeField]
		private int readyContentID;

		[SerializeField]
		private int notReadyContentID;

		[SerializeField]
		private Color readyColor;

		[SerializeField]
		private Color notReadyColor;
	}
}
