using System;
using GeneralVariable;
using GameCore;
using UnityEngine;

namespace Gameplay
{
	public class Hero3Ability2IceTrap : BaseMonoBehaviour
	{
        private int burnDamage;

        private string buffKey;

        private int slowPercent;

        private float duration;

        private string burningBuffKey = "Burning";

        [SerializeField]
        private new CircleCollider2D collider;
        
		public void Init(int burnDamage, string buffKey, int slowPercent, float duration)
		{
			this.burnDamage = burnDamage;
			this.buffKey = buffKey;
			this.slowPercent = slowPercent;
			this.duration = duration;
			collider.enabled = true;
			base.CustomInvoke(new Action(ReturnPool), duration);
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (other.gameObject.tag == GeneralVariable.GeneralDefine.ENEMY_TAG)
			{
				EnemyData component = other.gameObject.GetComponent<EnemyData>();
				if (!component.IsAir && component.IsAlive && !component.IsInTunnel && !component.IsUnderground)
				{
					DamageEnemy(component);
				}
			}
		}

		private void DamageEnemy(EnemyData enemyModel)
		{
			enemyModel.ProcessEffect(buffKey, slowPercent, duration, DamageVfxType.Slow);
			enemyModel.BuffsHolder.AddBuff(burningBuffKey, new BuffStatus(false, (float)burnDamage, duration), BuffStackRule.ChooseMax, BuffStackRule.ChooseMax);
		}

		private void ReturnPool()
		{
			collider.enabled = false;
			MonoSingleton<BulletPool>.Instance.Despawn(base.gameObject);
		}
	}
}
