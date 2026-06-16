using System;
using System.Collections.Generic;
using Gameplay;
using MetaGame;
using Parameter;
using UnityEngine;

namespace IncomingWave
{
	public class IncomingSurgeDialogHandler : GameplayDialogHandler
	{
		public void Init(Vector2 buttonPosition, Vector2 buttonSizeDelta)
		{
			GameFormat gameMode = FormatDirector.Instance.gameMode;
			if (gameMode != GameFormat.CampaignMode)
			{
				if (gameMode != GameFormat.DailyTrialMode)
				{
					if (gameMode == GameFormat.TournamentMode)
					{
						if (!GameplayDirector.Instance.endlessModeManager.IsLastEnemyInNormalWave)
						{
							currentWave = MonoSingleton<GameRecord>.Instance.CurrentWave;
							if (MonoSingleton<GameRecord>.Instance.CurrentWave >= MonoSingleton<GameRecord>.Instance.TotalWave)
							{
								currentWave = MonoSingleton<GameRecord>.Instance.TotalWave - 1;
							}
							listEnemyIDInWave = MonoSingleton<GameRecord>.Instance.GetListEnemyIDWithWave(currentWave);
						}
						else
						{
							currentWave = GameplayDirector.Instance.endlessModeManager.CurrentWaveEndless;
							listEnemyIDInWave = MonoSingleton<GameRecord>.Instance.GetListEnemyIDWithWave(currentWave);
						}
					}
				}
				else
				{
					currentWave = MonoSingleton<GameRecord>.Instance.CurrentWave;
					listEnemyIDInWave = MonoSingleton<GameRecord>.Instance.GetListEnemyIDWithWave(MonoSingleton<GameRecord>.Instance.CurrentWave);
				}
			}
			else
			{
				currentWave = MonoSingleton<GameRecord>.Instance.CurrentWave;
				listEnemyIDInWave = MonoSingleton<GameRecord>.Instance.GetListEnemyIDWithWave(MonoSingleton<GameRecord>.Instance.CurrentWave);
			}
			HideAllListEnemies();
			for (int i = 0; i < listEnemyIDInWave.Count; i++)
			{
				if (EnemyDatabase.Instance.IsBoss(listEnemyIDInWave[i]))
				{
					int totalEnemyInWave = MonoSingleton<GameRecord>.Instance.GetTotalEnemyInWave(listEnemyIDInWave[i], currentWave);
					bossPreview.Init(listEnemyIDInWave[i], totalEnemyInWave);
				}
				else
				{
					int totalEnemyInWave = MonoSingleton<GameRecord>.Instance.GetTotalEnemyInWave(listEnemyIDInWave[i], currentWave);
					listEnemyPreview[i].Init(listEnemyIDInWave[i], totalEnemyInWave);
				}
			}
			SetContentSize();
			Open();
			InitPopupPosition(buttonPosition, buttonSizeDelta);
		}

		private void InitPopupPosition(Vector2 buttonPosition, Vector2 buttonSizeDelta)
		{
			float num = 1.77777779f;
			float num2 = (float)Screen.width / (float)Screen.height;
			float d = num / num2;
			if (buttonPosition.x <= 0f && buttonPosition.y <= 0f)
			{
				popupPosition = buttonPosition + new Vector2(content.sizeDelta.x / 2f + buttonSizeDelta.x / 2f, content.sizeDelta.y / 2f + buttonSizeDelta.y / 2f) * d;
			}
			else if (buttonPosition.x <= 0f && buttonPosition.y > 0f)
			{
				popupPosition = buttonPosition + new Vector2(content.sizeDelta.x / 2f + buttonSizeDelta.x / 2f, -content.sizeDelta.y / 2f - buttonSizeDelta.y / 2f) * d;
			}
			else if (buttonPosition.x > 0f && buttonPosition.y > 0f)
			{
				popupPosition = buttonPosition + new Vector2(-content.sizeDelta.x / 2f - buttonSizeDelta.x / 2f, -content.sizeDelta.y / 2f - buttonSizeDelta.y / 2f) * d;
			}
			else if (buttonPosition.x > 0f && buttonPosition.y < 0f)
			{
				popupPosition = buttonPosition + new Vector2(-content.sizeDelta.x / 2f - buttonSizeDelta.x / 2f, content.sizeDelta.y / 2f + buttonSizeDelta.y / 2f) * d;
			}
			content.transform.localPosition = popupPosition;
		}

		private void SetContentSize()
		{
			contentSize = content.sizeDelta;
			if (EnemyDatabase.Instance.IsWaveHaveBoss(listEnemyIDInWave))
			{
				float num = Mathf.Ceil((float)(listEnemyIDInWave.Count - 1) / 2f);
				float newY = (float)baseHeightValue + num * (float)normalEnemyHeightSize + (float)bossHeightSize;
				contentSize.Set(contentSize.x, newY);
			}
			else
			{
				float num2 = Mathf.Ceil((float)listEnemyIDInWave.Count / 2f);
				float newY2 = (float)baseHeightValue + num2 * (float)normalEnemyHeightSize;
				contentSize.Set(contentSize.x, newY2);
			}
			content.sizeDelta = contentSize;
		}

		private void HideAllListEnemies()
		{
			foreach (EnemyInspectorPreview enemyPreview in listEnemyPreview)
			{
				enemyPreview.Hide();
			}
			bossPreview.Hide();
		}

		public override void Open()
		{
			base.Open();
			base.gameObject.SetActive(true);
		}

		public override void Close()
		{
			base.Close();
			base.gameObject.SetActive(false);
		}

		[Space]
		[SerializeField]
		private List<EnemyInspectorPreview> listEnemyPreview = new List<EnemyInspectorPreview>();

		[SerializeField]
		private EnemyInspectorPreview bossPreview;

		[Space]
		[Header("Content size setter")]
		[SerializeField]
		private RectTransform content;

		private Vector2 popupPosition = Vector2.zero;

		private Vector2 contentSize = Vector2.zero;

		[SerializeField]
		private int baseHeightValue;

		[SerializeField]
		private int normalEnemyHeightSize;

		[SerializeField]
		private int bossHeightSize;

		private List<int> listEnemyIDInWave = new List<int>();

		private int currentWave;
	}
}
