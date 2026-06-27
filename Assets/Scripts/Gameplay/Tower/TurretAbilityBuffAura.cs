using System.Collections.Generic;
using Parameter;
using UnityEngine;

namespace Gameplay
{
	// Supporter aura: every tick it re-applies short buffs to friendly towers within
	// auraRadius, reusing the same buff keys combat towers already respond to
	// (mirrors HasteUpAuraHandler). Values come from the tower parameter.
	public class TurretAbilityBuffAura : TurretHandler
	{
		public override void OnAppear()
		{
			base.OnAppear();
			TurretSpec spec = base.TowerModel.OriginalParameter;
			auraRadius = spec.auraRadius;
			damageAmp = spec.damageAmp;
			attackSpeedBonus = spec.attackSpeedBonus;
			rangeBonus = spec.rangeBonus;
			timeTracking = 0f;
			isReady = auraRadius > 0f && (damageAmp > 0 || attackSpeedBonus > 0 || rangeBonus > 0);
		}

		public override void OnReturnPool()
		{
			base.OnReturnPool();
			isReady = false;
			inRangeTowers.Clear();
		}

		public override void Update()
		{
			base.Update();
			if (!isReady || !MonoSingleton<GameRecord>.Instance.IsGameStart)
			{
				return;
			}
			if (timeTracking == 0f)
			{
				CastAura();
			}
			timeTracking = Mathf.MoveTowards(timeTracking, 0f, Time.deltaTime);
		}

		private void CastAura()
		{
			MonoSingleton<GameRecord>.Instance.GetInRangeTowers(base.transform.position, auraRadius, inRangeTowers);
			// Amplifier Crystal (and similar) items boost the aura's damage amp; read each tick so it
			// applies live when equipped mid-match (auraRadius/damageAmp are cached once in OnAppear).
			int effectiveDamageAmp = damageAmp;
			float itemAuraDamageAmp;
			if (base.TowerModel.BuffsHolder.TryGetBuffValue(BuffKeysToTurret.ITEM_AURA_DAMAGE_AMP_FLAT, out itemAuraDamageAmp) && itemAuraDamageAmp > 0f)
			{
				effectiveDamageAmp += (int)itemAuraDamageAmp;
			}
			for (int i = 0; i < inRangeTowers.Count; i++)
			{
				TurretEntity tower = inRangeTowers[i];
				if (tower == base.TowerModel)
				{
					continue; // don't buff the Supporter itself
				}
				if (effectiveDamageAmp > 0)
				{
					tower.BuffsHolder.AddBuff(DAMAGE_KEY, new BuffStatus(false, effectiveDamageAmp, BUFF_DURATION), BuffStackRule.ChooseMax, BuffStackRule.ChooseMax);
				}
				if (attackSpeedBonus > 0)
				{
					// Attack-speed key is "% of base reload" where 100 = unchanged, so the bonus sits on top of 100.
					tower.BuffsHolder.AddBuff(ATTACK_SPEED_KEY, new BuffStatus(false, 100 + attackSpeedBonus, BUFF_DURATION), BuffStackRule.ChooseMax, BuffStackRule.ChooseMax);
				}
				if (rangeBonus > 0)
				{
					tower.BuffsHolder.AddBuff(RANGE_KEY, new BuffStatus(false, rangeBonus, BUFF_DURATION), BuffStackRule.ChooseMax, BuffStackRule.ChooseMax);
				}
			}
			timeTracking = BUFF_DURATION;
		}

		private const string DAMAGE_KEY = "DamageIncrementCommon";
		private const string ATTACK_SPEED_KEY = "IncreaseAttackSpeedByPercentage";
		private const string RANGE_KEY = "AttackRangeIncrementCommon";
		private const float BUFF_DURATION = 1f;

		private float auraRadius;
		private int damageAmp;
		private int attackSpeedBonus;
		private int rangeBonus;
		private float timeTracking;
		private bool isReady;
		private List<TurretEntity> inRangeTowers = new List<TurretEntity>();
	}
}
