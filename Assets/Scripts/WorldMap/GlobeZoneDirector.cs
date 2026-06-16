using Data;
using GiftcodeSystem;
using Services.PlatformSpecific;
using Tutorial;
using UnityEngine;

namespace WorldMap
{
	public class GlobeZoneDirector : MonoSingleton<GlobeZoneDirector>
	{
        [SerializeField]
        private TutorialWorldMap worldMapTutorial;

        [SerializeField]
        private VoucherCodeDirector giftCodeManager;

        public TutorialWorldMap WorldMapTutorial
		{
			get
			{
				return worldMapTutorial;
			}
			set
			{
				worldMapTutorial = value;
			}
		}

		public VoucherCodeDirector GiftCodeManager
		{
			get
			{
				return giftCodeManager;
			}
			set
			{
				giftCodeManager = value;
			}
		}

		private void Start()
		{
			SendEventOpenScene();
			worldMapTutorial.SyncPassedIfObsolete();
			DailyCheckinDirector.Instance.WorldMapCheckIn(worldMapTutorial);
		}

		private void SendEventOpenScene()
		{
			int currentGem = PlayerCurrencyStore.Instance.GetCurrentGem();
			int currentStar = PlayerCurrencyStore.Instance.GetCurrentStar();
			int maxMapIDUnlocked = MapProgressStore.Instance.GetMapIDUnlocked() + 1;
			//NativeSpecificServicesSource.Services.Analytics.SendEvent_OpenSceneWorldMap(currentGem, currentStar, maxMapIDUnlocked);
		}
	}
}
