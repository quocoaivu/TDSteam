using System;
using GameCore;
using UnityEngine;

namespace Gameplay
{
	public class CrystalHandler : BaseMonoBehaviour
	{
        [SerializeField]
        private float lifeTime;

        [SerializeField]
        private TextMesh textMesh;
        
		public void Init(int gemAmount)
		{
			textMesh.text = "+ " + gemAmount.ToString();
			base.CustomInvoke(new Action(LateAnimationOpen), lifeTime);
		}

		private void LateAnimationOpen()
		{
			MonoSingleton<FXPool>.Instance.Despawn(base.gameObject);
		}
	}
}
