using System;
using MetaGame;
using GameCore;
using UnityEngine;

namespace Gameplay
{
	[DisallowMultipleComponent]
	public class FirstTimeHintAppear : BaseMonoBehaviour
	{
        [SerializeField]
        private int tipID;

        public void TryToGetTip()
		{
			bool flag = GameplayTipDiscoveryTracker.Instance.IsTipFirstTime(tipID);
			if (flag)
			{
				TipInformationUIManager.Instance.TryActivateButton(tipID);
			}
		}
	}
}
