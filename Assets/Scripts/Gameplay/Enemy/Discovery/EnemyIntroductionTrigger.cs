using System;
using MetaGame;
using UnityEngine;

namespace Gameplay
{
	[DisallowMultipleComponent]
	public class EnemyIntroductionTrigger : EnemyBrain
	{
		public override void OnAppear()
		{
			base.OnAppear();
			bool flag = EnemyDiscoveryTracker.Instance.IsEnemyFirstTime(base.EnemyModel.Id);
			if (flag)
			{
				EnemyDiscoveryUIManager.Instance.TryActivateButton(base.EnemyModel.Id);
			}
		}
	}
}
