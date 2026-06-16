using System;
using GameCore;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
	public class BonusBountyMotionHandler : BaseMonoBehaviour
	{
        [SerializeField]
        private Text bonusMoneyText;

        [SerializeField]
        private float lifeTime;

        public void Init(int bonusMoney)
		{
			bonusMoneyText.text = string.Format("+ {0}", bonusMoney);
			base.CustomInvoke(new Action(LateAnimationOpen), lifeTime);
		}

		private void LateAnimationOpen()
		{
			MonoSingleton<FXPool>.Instance.Despawn(base.gameObject);
		}
	}
}
