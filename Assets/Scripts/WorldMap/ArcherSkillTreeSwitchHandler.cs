using System;
using UnityEngine;

namespace WorldMap
{
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
			MonoSingleton<UIRootHandler>.Instance.archerSkillTreePopupController.Init(towerID);
		}
	}
}
