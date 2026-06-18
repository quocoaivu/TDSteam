using System;
using Upgrade;
using UnityEngine;

namespace WorldMap
{
	// Opens a tower's skill-tree panel from the Worldmap. Generic across towers: set `towerID`
	// per button (0 = Archer, 2 = Stone God, ...). Panel prefabs are wired in UIRootHandler.
	public class ArcherSkillTreeSwitchHandler : SwitchHandler
	{
        [SerializeField]
        private float timeToOpen;

        [SerializeField]
        private int towerID;


        public override void OnClick()
		{
			base.OnClick();
			base.CustomInvoke(new Action(DoOpen), timeToOpen);
		}

		private void DoOpen()
		{
			TowerSkillTreePanel panel = MonoSingleton<UIRootHandler>.Instance.GetTowerSkillTree();
			if (panel != null)
			{
				panel.Init(towerID);
			}
		}
	}
}
