using System;
using GeneralVariable;
using UnityEngine;

namespace Gameplay
{
	public class EnemyPathFollower : EnemyBrain
	{
        [SerializeField]
        private float offset = 0.1f;

        private GameObject tunnelInPos;

        private GameObject tunnelOutPos;

        private void Start()
		{
			tunnelInPos = GameObject.FindGameObjectWithTag(GeneralVariable.GeneralDefine.TUNNEL_IN_NAME);
			tunnelOutPos = GameObject.FindGameObjectWithTag(GeneralVariable.GeneralDefine.TUNNEL_OUT_NAME);
		}

		public override void Update()
		{
			base.Update();
			if (tunnelInPos == null || tunnelOutPos == null)
			{
				return;
			}
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (GameKit.IsEnemyAbleToGoTunnel(base.EnemyModel))
			{
				if (other.CompareTag(GeneralVariable.GeneralDefine.TUNNEL_IN_NAME) && !base.EnemyModel.IsInTunnel)
				{
					OnGoIntoTunnel();
				}
				if (other.CompareTag(GeneralVariable.GeneralDefine.TUNNEL_OUT_NAME) && base.EnemyModel.IsInTunnel)
				{
					OnGoOutOfTunnel();
				}
			}
		}

		private void OnGoIntoTunnel()
		{
			if (!base.EnemyModel.IsInTunnel && GameKit.IsEnemyAbleToGoTunnel(base.EnemyModel))
			{
				base.EnemyModel.IsInTunnel = true;
				MonoSingleton<GameRecord>.Instance.RemoveEnemyFromListActiveEnemy(base.EnemyModel);
				base.EnemyModel.EnemyEffectController.Hide();
				base.EnemyModel.EnemyHealthController.HideHealthBar();
			}
		}

		private void OnGoOutOfTunnel()
		{
			if (base.EnemyModel.IsInTunnel)
			{
				base.EnemyModel.IsInTunnel = false;
				MonoSingleton<GameRecord>.Instance.AddEnemyToListActiveEnemy(base.EnemyModel);
			}
			base.EnemyModel.EnemyEffectController.Show();
		}
	}
}
