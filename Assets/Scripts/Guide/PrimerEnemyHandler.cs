using System;
using System.Collections.Generic;
using MetaGame;
using Parameter;
using UnityEngine;
using UnityEngine.UI;

namespace Guide
{
	public class PrimerEnemyHandler : GeneralDialogHandler
	{
		public void Init()
		{
			Open();
			listEnemyID = EnemyDatabase.Instance.GetAllEnemyIds();
			InitListButton();
			SetButtonStatus();
			base.CustomInvoke(new Action(InitDefaultData), Time.deltaTime);
		}

		private void InitListButton()
		{
			if (listSelectEnemy.Count < 1)
			{
				for (int i = 0; i < listEnemyID.Count; i++)
				{
					string path = string.Format("UI WorldMap/Guide/SelectEnemy/select_enemy", new object[0]);
					SelectEnemySwitchHandler selectEnemyButtonController = UnityEngine.Object.Instantiate<SelectEnemySwitchHandler>(Common.AssetLoader.Load<SelectEnemySwitchHandler>(path));
					selectEnemyButtonController.transform.SetParent(listButtonsHolder);
					selectEnemyButtonController.Init(listEnemyID[i]);
					selectEnemyButtonController.transform.localScale = Vector3.one;
					listSelectEnemy.Add(selectEnemyButtonController);
				}
			}
		}

		private void SetButtonStatus()
		{
			listUnlockedEnemy = EnemyDiscoveryTracker.Instance.GetListEnemyUnlockStatus();
			for (int i = 0; i < listEnemyID.Count; i++)
			{
				if (listUnlockedEnemy[i])
				{
					listSelectEnemy[i].SetUnLock();
				}
				else
				{
					listSelectEnemy[i].SetLock();
				}
			}
		}

		public void RefreshEnemyInformation()
		{
			SetAvatar();
			enemyInformationController.Init(currentEnemyIDSelected);
		}

		private void SetAvatar()
		{
			enemyAvatar.sprite = Common.AssetLoader.Load<Sprite>(string.Format("Preview/Enemies/FullAvatars/fa_enemy_{0}", currentEnemyIDSelected));
		}

		private void InitDefaultData()
		{
			if (listUnlockedEnemy.Count > 0 && listUnlockedEnemy[0])
			{
				listSelectEnemy[0].OnClick();
			}
		}

		public override void Open()
		{
			base.Open();
		}

		public override void Close()
		{
			base.Close();
			PrimerDialogHandler.Instance.HideSelectedEnemyImage();
		}

		[NonSerialized]
		public int currentEnemyIDSelected;

		[SerializeField]
		private Transform listButtonsHolder;

		[SerializeField]
		private Transform listAvatarHolder;

		[SerializeField]
		private EnemyOverviewHandler enemyInformationController;

		[SerializeField]
		private Image enemyAvatar;

		private List<SelectEnemySwitchHandler> listSelectEnemy = new List<SelectEnemySwitchHandler>();

		private List<int> listEnemyID;

		private List<bool> listUnlockedEnemy;
	}
}
