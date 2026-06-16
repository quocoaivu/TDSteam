using System;
using System.Collections.Generic;
using GameCore;
using UnityEngine;

namespace Gameplay
{
	public class Hero5Ability0MendBomb : BaseMonoBehaviour
	{
        private int healAmount;

        private float skillRange;

        private List<CharacterEntity> allyInRange = new List<CharacterEntity>();

        public void Init(int healAmount, float skillRange)
		{
			this.healAmount = healAmount;
			this.skillRange = skillRange;
			Healing();
			base.CustomInvoke(new Action(ReturnPool), 3f);
		}

		public void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(base.transform.position, skillRange);
		}

		private void Healing()
		{
			GetAlliesInRange();
			for (int i = 0; i < allyInRange.Count; i++)
			{
				HealingAlly(allyInRange[i]);
			}
		}

		private void HealingAlly(CharacterEntity ally)
		{
			if (ally.IsAlive)
			{
				ally.IncreaseHealth(healAmount);
				VisualEffectInstance effect = MonoSingleton<FXPool>.Instance.GetEffect(FXPool.EFFECT_HEAL_0);
				effect.transform.position = ally.transform.position;
				effect.Init(1.5f, ally.BuffsHolder.transform, ally.GetComponent<SpriteRenderer>().sprite.rect.width);
			}
		}

		private void GetAlliesInRange()
		{
			allyInRange.Clear();
			List<CharacterEntity> listActiveAlly = MonoSingleton<GameRecord>.Instance.ListActiveAlly;
			for (int i = 0; i < listActiveAlly.Count; i++)
			{
				CharacterEntity characterModel = listActiveAlly[i];
				float num = MonoSingleton<GameRecord>.Instance.SqrDistance(base.gameObject, characterModel.gameObject);
				if (num <= skillRange * skillRange)
				{
					allyInRange.Add(characterModel);
				}
			}
		}

		private void ReturnPool()
		{
			MonoSingleton<BulletPool>.Instance.Despawn(base.gameObject);
		}

	}
}
