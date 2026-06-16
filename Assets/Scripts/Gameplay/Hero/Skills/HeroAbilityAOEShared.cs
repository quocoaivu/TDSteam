using System;
using GeneralVariable;
using GameCore;
using UnityEngine;

namespace Gameplay
{
	public class HeroAbilityAOEShared : BaseMonoBehaviour
	{
        private string buffKey;

        private int slowPercent;

        private float duration;

        [SerializeField]
        private AoeDamageCaster damageToAOERange;

        private SharedStrikeDamage commonAttackDamageSender;

        private OnHitStatusApplier effectAttackSender;

        private bool hasEffect;

        private string effectName;

        private float effectDuration;

        [SerializeField]
        private TimerBombTicker timerBombCountdown;

        public void Init_DamageOnTrap(SharedStrikeDamage _commonAttackDamageSender, OnHitStatusApplier _effectAttackSender, float duration, string effectName, float effectDuration)
		{
			commonAttackDamageSender = new SharedStrikeDamage();
			commonAttackDamageSender.physicsDamage = _commonAttackDamageSender.physicsDamage;
			commonAttackDamageSender.magicDamage = _commonAttackDamageSender.magicDamage;
			commonAttackDamageSender.aoeRange = _commonAttackDamageSender.aoeRange;
			effectAttackSender.buffKey = _effectAttackSender.buffKey;
			effectAttackSender.debuffChance = _effectAttackSender.debuffChance;
			effectAttackSender.debuffEffectValue = _effectAttackSender.debuffEffectValue;
			effectAttackSender.debuffEffectDuration = _effectAttackSender.debuffEffectDuration;
			effectAttackSender.damageFXType = _effectAttackSender.damageFXType;
			hasEffect = true;
			this.effectName = effectName;
			this.effectDuration = effectDuration;
			base.CustomInvoke(new Action(ReturnPool), duration);
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (other.CompareTag(GeneralVariable.GeneralDefine.ENEMY_TAG))
			{
				EnemyData component = other.GetComponent<EnemyData>();
				if (!component.IsAir && !component.IsUnderground && !component.IsInTunnel)
				{
					DamageWithAOE();
					CastEffect();
					ReturnPool();
				}
			}
		}

		public void Init_DamgeAfterTime(SharedStrikeDamage _commonAttackDamageSender, float countdownTime, string effectName, float effectDuration)
		{
			commonAttackDamageSender = new SharedStrikeDamage();
			commonAttackDamageSender.physicsDamage = _commonAttackDamageSender.physicsDamage;
			commonAttackDamageSender.magicDamage = _commonAttackDamageSender.magicDamage;
			commonAttackDamageSender.aoeRange = _commonAttackDamageSender.aoeRange;
			hasEffect = false;
			this.effectName = effectName;
			this.effectDuration = effectDuration;
			if (timerBombCountdown)
			{
				timerBombCountdown.Init(countdownTime);
			}
			base.CustomInvoke(new Action(DamageWithAOE), countdownTime);
			base.CustomInvoke(new Action(CastEffect), countdownTime);
			base.CustomInvoke(new Action(ReturnPool), countdownTime);
		}

		private void CastEffect()
		{
			if (!string.IsNullOrEmpty(effectName))
			{
				VisualEffectInstance effect = MonoSingleton<FXPool>.Instance.GetEffect(effectName);
				effect.transform.position = base.transform.position;
				effect.Init(effectDuration);
			}
		}

		public void Init_DamageImmediately(SharedStrikeDamage _commonAttackDamageSender, OnHitStatusApplier _effectAttackSender, float effectLifeTime)
		{
			commonAttackDamageSender = new SharedStrikeDamage();
			commonAttackDamageSender.physicsDamage = _commonAttackDamageSender.physicsDamage;
			commonAttackDamageSender.magicDamage = _commonAttackDamageSender.magicDamage;
			commonAttackDamageSender.aoeRange = _commonAttackDamageSender.aoeRange;
			effectAttackSender.buffKey = _effectAttackSender.buffKey;
			effectAttackSender.debuffChance = _effectAttackSender.debuffChance;
			effectAttackSender.debuffEffectValue = _effectAttackSender.debuffEffectValue;
			effectAttackSender.debuffEffectDuration = _effectAttackSender.debuffEffectDuration;
			effectAttackSender.damageFXType = _effectAttackSender.damageFXType;
			hasEffect = true;
			DamageWithAOE();
			base.CustomInvoke(new Action(ReturnPool), effectLifeTime);
		}

		private void DamageWithAOE()
		{
			if (hasEffect)
			{
				damageToAOERange.CastDamage(DamageKind.Range, commonAttackDamageSender, effectAttackSender);
			}
			else
			{
				damageToAOERange.CastDamage(DamageKind.Range, commonAttackDamageSender);
			}
			hasEffect = false;
		}

		private void ReturnPool()
		{
			MonoSingleton<BulletPool>.Instance.Despawn(base.gameObject);
		}

	}
}
