using System;
using Data;
using GameCore;
using Parameter;
using UnityEngine;
using UnityEngine.UI;

namespace Upgrade
{
	public class EnhanceOverviewHandler : BaseMonoBehaviour
	{
        [SerializeField]
        private Image upgradeTierIcon;

        [SerializeField]
        private Text starRequire;

        [SerializeField]
        private Text upgradeTierTitle;

        [SerializeField]
        private Text upgradeTierDescription;

        public void InitDefaultData()
		{
			GlobalUpgradePopupController.Instance.upgradeGroupControllers[0].listTierUpgrade[0].InitDefaultData();
		}

		public void InitData(Sprite _tierIcon, int tierID, int upgradeID, int towerID)
		{
			upgradeTierIcon.enabled = true;
			upgradeTierIcon.sprite = _tierIcon;
			starRequire.text = GlobalUpgradeStore.Instance.GetStarRequireForUpgrade(towerID, tierID).ToString();
			upgradeTierTitle.text = Singleton<GlobalEnhanceSynopsis>.Instance.GetTitle(upgradeID);
			upgradeTierDescription.text = string.Format(Singleton<GlobalEnhanceSynopsis>.Instance.GetDescription(upgradeID), GlobalUpgradeStore.Instance.GetUpgradeValue(upgradeID, towerID)).Replace('@', '\n').Replace('#', '-');
		}
	}
}
