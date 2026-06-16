using System;
using System.Collections.Generic;
using MetaGame;
using Parameter;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
	public class TowerInformationPopup : GameplayDialogHandler
	{
		public void Init()
		{
			OpenWithScaleAnimation();
			GameFormat gameMode = FormatDirector.Instance.gameMode;
			if (gameMode != GameFormat.CampaignMode)
			{
				if (gameMode != GameFormat.DailyTrialMode)
				{
					if (gameMode != GameFormat.TournamentMode)
					{
					}
				}
				else
				{
					InitNormalInformation_DailyTrial();
				}
			}
			else
			{
				int tipInforID = ZoneRuleSpec.Instance.getTipInforID(MonoSingleton<GameRecord>.Instance.MapID);
				if (tipInforID > 0)
				{
					InitNormalInformation_Campaign();
				}
				else
				{
					InitUltimateInformation_Campaign();
				}
			}
		}

		private void InitNormalInformation_Campaign()
		{
			normalDescriptionHolder.SetActive(true);
			int tipInforID = ZoneRuleSpec.Instance.getTipInforID(MonoSingleton<GameRecord>.Instance.MapID);
			normalDescription.text = Singleton<GameplayTipsSynopsis>.Instance.GetDescription(tipInforID).Replace('@', '\n').Replace('#', '-');
			List<TurretTutorial> listTowerTutorialInMap = ZoneRuleSpec.Instance.getListTowerTutorialInMap(MonoSingleton<GameRecord>.Instance.MapID);
			for (int i = 0; i < listTowerTutorialInMap.Count; i++)
			{
				listNormalTowerAvatars[i].Init(listTowerTutorialInMap[i].id, listTowerTutorialInMap[i].level);
				listNormalTowerAvatars[i].Show();
			}
		}

		private void InitUltimateInformation_Campaign()
		{
			ultimateDescriptionHolder.SetActive(true);
			List<TurretTutorial> listTowerTutorialInMap = ZoneRuleSpec.Instance.getListTowerTutorialInMap(MonoSingleton<GameRecord>.Instance.MapID);
			int id = listTowerTutorialInMap[0].id;
			int level = listTowerTutorialInMap[0].level;
			generalDescription.text = Singleton<TurretSynopsis>.Instance.GetTowerUnlockDescription(id, level).Replace('@', '\n').Replace('#', '-');
			string text = Singleton<TurretSynopsis>.Instance.GetTowerUltimateDescription(id, level, 0) + "\n" + Singleton<TurretSynopsis>.Instance.GetTowerUltimateDescription(id, level, 1);
			ultimateDescription.text = text.Replace('@', '\n').Replace('#', '-');
			ultimateTowerAvatars.Init(id, level);
			ultimateTowerAvatars.Show();
		}

		private void InitNormalInformation_DailyTrial()
		{
			normalDescriptionHolder.SetActive(true);
			int notiID = 91;
			normalDescription.text = Singleton<AlertSynopsis>.Instance.GetNotiContent(notiID).Replace('@', '\n').Replace('#', '-');
			foreach (TurretPortrait towerAvatar in listNormalTowerAvatars)
			{
				towerAvatar.Hide();
			}
			int currentWave = MonoSingleton<GameRecord>.Instance.CurrentWave;
			List<int> listTowerIDForPopup = ZoneRuleSpec.Instance.GetListTowerIDForPopup(currentWave);
			for (int j = 0; j < listTowerIDForPopup.Count; j++)
			{
				int maxLevelTowerCanUpgrade_Daily = ZoneRuleSpec.Instance.GetMaxLevelTowerCanUpgrade_Daily(currentWave, listTowerIDForPopup[j]);
				listNormalTowerAvatars[j].Init(listTowerIDForPopup[j], maxLevelTowerCanUpgrade_Daily);
				listNormalTowerAvatars[j].Show();
			}
		}

		public override void OpenWithScaleAnimation()
		{
			base.OpenWithScaleAnimation();
			GameFormat gameMode = FormatDirector.Instance.gameMode;
			if (gameMode != GameFormat.CampaignMode)
			{
				if (gameMode != GameFormat.DailyTrialMode)
				{
					if (gameMode != GameFormat.TournamentMode)
					{
					}
				}
				else
				{
					GameplayDirector.Instance.gameSpeedController.PauseGame();
				}
			}
		}

		public override void CloseWithScaleAnimation()
		{
			base.CloseWithScaleAnimation();
			GameFormat gameMode = FormatDirector.Instance.gameMode;
			if (gameMode != GameFormat.CampaignMode)
			{
				if (gameMode != GameFormat.DailyTrialMode)
				{
					if (gameMode != GameFormat.TournamentMode)
					{
					}
				}
				else
				{
					GameplayDirector.Instance.gameSpeedController.UnPauseGame();
				}
			}
		}

		[Space]
		[Header("Information for normal upgrade")]
		[SerializeField]
		private TurretPortrait[] listNormalTowerAvatars;

		[SerializeField]
		private GameObject normalDescriptionHolder;

		[SerializeField]
		private Text normalDescription;

		[Space]
		[Header("Information for ultimate upgrade")]
		[SerializeField]
		private TurretPortrait ultimateTowerAvatars;

		[SerializeField]
		private GameObject ultimateDescriptionHolder;

		[SerializeField]
		private Text generalDescription;

		[SerializeField]
		private Text ultimateDescription;
	}
}
