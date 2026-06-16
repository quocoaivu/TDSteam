using System;
using MetaGame;
using UnityEngine;

namespace Gameplay
{
	public class EnemyLootDropper : EnemyBrain
	{
        private float dropGemPercent;

        private int dropGemAmountMin;

        private int dropGemAmountMax;

        public override void OnAppear()
		{
			base.OnAppear();
			dropGemAmountMin = base.EnemyModel.OriginalParameter.dropGemAmountMin;
			dropGemAmountMax = base.EnemyModel.OriginalParameter.dropGemAmountMax;
			int playCountMapInCampaign = MonoSingleton<GameRecord>.Instance.PlayCountMapInCampaign;
			if (playCountMapInCampaign == 0)
			{
				dropGemPercent = (float)base.EnemyModel.OriginalParameter.dropGemPercent * ((float)Setup.Instance.FirstTimeGemTakenPercentage / 100f);
			}
			if (playCountMapInCampaign == 1)
			{
				dropGemPercent = (float)base.EnemyModel.OriginalParameter.dropGemPercent * ((float)Setup.Instance.SecondTimeGemTakenPercentage / 100f);
			}
			if (playCountMapInCampaign >= 2)
			{
				dropGemPercent = (float)base.EnemyModel.OriginalParameter.dropGemPercent * ((float)Setup.Instance.ThirdTimeGemTakenPercentage / 100f);
			}
		}

		public void TryDropDiamond()
		{
			GameFormat gameMode = FormatDirector.Instance.gameMode;
			if (gameMode != GameFormat.CampaignMode)
			{
				if (gameMode != GameFormat.DailyTrialMode)
				{
					if (gameMode != GameFormat.TournamentMode)
					{
					}
				}
			}
			else
			{
				bool flag = (float)UnityEngine.Random.Range(0, 100) < dropGemPercent && !MonoSingleton<GameRecord>.Instance.IsGameOver;
				if (flag)
				{
					int num = UnityEngine.Random.Range(dropGemAmountMin, dropGemAmountMax);
					CrystalHandler droppedGem = MonoSingleton<FXPool>.Instance.GetDroppedGem();
					droppedGem.gameObject.SetActive(true);
					droppedGem.transform.position = base.EnemyModel.transform.position;
					droppedGem.Init(num);
					MonoSingleton<GameRecord>.Instance.GameplayGem += num;
				}
			}
		}
	}
}
