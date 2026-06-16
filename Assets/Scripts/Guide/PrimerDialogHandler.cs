using System;
using Data;
using Gameplay;
using Services.PlatformSpecific;
using UnityEngine;

namespace Guide
{
	public class PrimerDialogHandler : GameplayDialogHandler
	{
		public PrimerTurretHandler GuideTowerController
		{
			get
			{
				return guideTowerController;
			}
			set
			{
				guideTowerController = value;
			}
		}

		public PrimerEnemyHandler GuideEnemyController
		{
			get
			{
				return guideEnemyController;
			}
			set
			{
				guideEnemyController = value;
			}
		}

		public PrimerTipsHandler GuideTipsController
		{
			get
			{
				return guideTipsController;
			}
			set
			{
				guideTipsController = value;
			}
		}

		public static PrimerDialogHandler Instance
		{
			get
			{
				return PrimerDialogHandler._instance;
			}
		}

		private void Awake()
		{
			PrimerDialogHandler._instance = this;
		}

		public void Init()
		{
			OpenWithScaleAnimation();
			HideSelectedEnemyImage();
			SendEventOpenPanel();
		}

		private void SendEventOpenPanel()
		{
			int maxMapIDUnlocked = MapProgressStore.Instance.GetMapIDUnlocked() + 1;
			//NativeSpecificServicesSource.Services.Analytics.SendEvent_OpenGuide(maxMapIDUnlocked);
		}

		public void ShowSelectedEnemyImage(Transform transform)
		{
			if (!selectedEnemyImage.gameObject.activeSelf)
			{
				selectedEnemyImage.gameObject.SetActive(true);
			}
			selectedEnemyImage.Init(transform);
		}

		public void ShowSelectedTowerImage(Transform transform)
		{
			if (!selectedTowerImage.gameObject.activeSelf)
			{
				selectedTowerImage.gameObject.SetActive(true);
			}
			selectedTowerImage.Init(transform);
		}

		public void HideSelectedEnemyImage()
		{
			selectedEnemyImage.gameObject.SetActive(false);
		}

		public void HideSelectedTowerImage()
		{
			selectedTowerImage.gameObject.SetActive(false);
		}

		public void OpenGuideTower(float delayTime)
		{
			base.CustomInvoke(new Action(DoOpenGuideTower), delayTime);
		}

		private void DoOpenGuideTower()
		{
			GuideTowerController.Init();
		}

		public void OpenGuideEnemy(float delayTime)
		{
			base.CustomInvoke(new Action(DoOpenGuideEnemy), delayTime);
		}

		private void DoOpenGuideEnemy()
		{
			GuideEnemyController.Init();
		}

		public void OpenGuideTip(float delayTime)
		{
			base.CustomInvoke(new Action(DoOpenGuideTip), delayTime);
		}

		private void DoOpenGuideTip()
		{
			GuideTipsController.Init();
		}

		public override void OpenWithScaleAnimation()
		{
			base.OpenWithScaleAnimation();
		}

		public override void CloseWithScaleAnimation()
		{
			base.CloseWithScaleAnimation();
		}

		[SerializeField]
		private PrimerTurretHandler guideTowerController;

		[SerializeField]
		private PrimerEnemyHandler guideEnemyController;

		[SerializeField]
		private PrimerTipsHandler guideTipsController;

		[SerializeField]
		private SelectedPictureHandler selectedEnemyImage;

		[SerializeField]
		private SelectedPictureHandler selectedTowerImage;

		private static PrimerDialogHandler _instance;
		[UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			_instance = null;
		}
	}
}
