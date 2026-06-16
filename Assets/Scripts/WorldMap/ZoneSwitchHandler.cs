using Data;
using LifetimePopup;
using MetaGame;
using UnityEngine;
using UnityEngine.UI;

namespace WorldMap
{
	public class ZoneSwitchHandler : SwitchHandler
	{
        [SerializeField]
        private int mapID;

        [SerializeField]
        private int themeID;

        [Space]
        [SerializeField]
        private StarClusterHandler starGroupController;

        [Space]
        [SerializeField]
        private GameObject[] listFlagObject;

        [SerializeField]
        private GameObject lockObject;

        [Space]
        [Header("Components")]
        [SerializeField]
        private Button button;

        [Space]
        [Header("Specific setting")]
        [SerializeField]
        private bool lockMap;
		 
        private void Awake()
		{
			button = base.GetComponent<Button>();
		}

		private void Start()
		{
			starGroupController.DisplayStarGroup(MapProgressStore.Instance.GetStarEarnedByMap(mapID));
		}

		public void ViewLock()
		{
			button.enabled = false;
			HideAllFlags();
			ShowLockFlag();
		}

		public void ViewUnLock()
		{
			int mapModeResult = MapProgressStore.Instance.GetMapModeResult(mapID);
			HideAllFlags();
			switch (mapModeResult)
			{
			case 0:
				ShowFlag(0);
				break;
			case 1:
				ShowFlag(0);
				break;
			case 2:
				ShowFlag(1);
				break;
			case 3:
				ShowFlag(2);
				break;
			}
			button.enabled = true;
		}

		private void HideAllFlags()
		{
			foreach (GameObject gameObject in listFlagObject)
			{
				gameObject.SetActive(false);
			}
			lockObject.SetActive(false);
		}

		private void ShowFlag(int flagID)
		{
			listFlagObject[flagID].SetActive(true);
		}

		private void ShowLockFlag()
		{
			lockObject.SetActive(true);
		}

		public override void OnClick()
		{
			base.OnClick();
			if (lockMap)
			{
				string content = "Coming soon!";
				MonoSingleton<LifespanSurface>.Instance.NotifyPopupController.Init(content, false, false);
			}
			else
			{
				CrossSceneData.Instance.MapIDSelected = mapID;
				ThemeStore.Instance.SaveLastThemePlayed(themeID);
				MonoSingleton<UIRootHandler>.Instance.mapLevelSelectPopupController.Init(mapID);
			}
		}
	}
}
