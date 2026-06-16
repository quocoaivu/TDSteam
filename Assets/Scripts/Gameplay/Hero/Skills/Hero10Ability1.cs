using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class Hero10Ability1 : HeroAbilityShared
	{
        public GameObject steelFxPrefab;

        public float disTriggerAttack = 1f;

        public float delayDealdam = 0.3f;

        public int numOfSteelProj = 5;

        private int heroID = 10;

        private int skillID = 1;

        private int currentLevel;

        private int currentSkillLevel;

        private float sqDisTriggerAttack;

        private float cooldownDuration;

        private float cooldownCountdown;

        private float skillRange;
        private Hero10Ability1Specs skillParams;


        private bool unlocked;
        public override void Init(HeroEntity heroModel)
		{
			base.Init(heroModel);
			this.heroModel = heroModel;
			unlocked = true;
			currentLevel = HeroStore.Instance.GetCurrentHeroLevel(heroID);
			currentSkillLevel = HeroParameterManager.Instance.GetSkillPoint(heroID, currentLevel, skillID);
			skillParams = (HeroAbilitySpec.Instance.GetHeroSkillsParameter(heroID, skillID) as HeroAbilitySpec_10_1).listParam[currentSkillLevel - 1];
			cooldownDuration = (float)skillParams.cooldown_time * 0.001f;
			cooldownCountdown = cooldownDuration * 0.7f;
			sqDisTriggerAttack = disTriggerAttack * disTriggerAttack;
			skillRange = (float)skillParams.skill_range / GameRecord.PIXEL_PER_UNIT;
		}

		public override void Update()
		{
			base.Update();
			if (!unlocked)
			{
				return;
			}
			if (cooldownCountdown > 0f)
			{
				cooldownCountdown -= Time.deltaTime;
			}
			else if (IsEmptySpecialState() && GameKit.IsValidEnemy(heroModel.GetCurrentTarget()) && MonoSingleton<GameRecord>.Instance.SqrDistance(heroModel.transform.position, heroModel.GetCurrentTarget().transform.position) < sqDisTriggerAttack)
			{
				cooldownCountdown = cooldownDuration;
				base.StartCoroutine(CastSkill(heroModel.GetCurrentTarget().transform.position));
			}
		}

		private IEnumerator CastSkill(Vector3 targetPos)
		{
			heroModel.SetSpecialStateDuration(delayDealdam * 2.5f);
			heroModel.SetSpecialStateAnimationName(HeroMotionHandler.animPassiveSkill_0);
			heroModel.GetFsmController().GetCurrentState().OnInput(PhaseInputKind.SpecialState, new object[]
			{
				HeroMotionHandler.animPassiveSkill_0,
				delayDealdam * 2.5f
			});
			yield return new WaitForSeconds(0.3f);
			ObjectCache.Spawn(steelFxPrefab, targetPos);
			for (int i = 0; i < numOfSteelProj - 1; i++)
			{
				Vector2 offset = UnityEngine.Random.insideUnitCircle * skillRange;
				Vector3 projTargetPos = targetPos + new Vector3(offset.x, offset.y, 0f);
				ObjectCache.Spawn(steelFxPrefab, projTargetPos);
				yield return new WaitForSeconds(0.12f);
				MonoSingleton<LensHandler>.Instance.ShakeNormal();
			}
			OnHitStatusApplier ccData = default(OnHitStatusApplier);
			ccData.buffKey = "DefDown";
			ccData.damageFXType = DamageVfxType.DefDown;
			ccData.debuffChance = 100;
			ccData.debuffEffectDuration = (float)skillParams.def_down_duration * 0.001f;
			ccData.debuffEffectValue = skillParams.def_down_percent;
			List<EnemyData> targetList = GameKit.GetListEnemiesInRange(targetPos, new SharedStrikeDamage(0, skillParams.magic_damage, false, skillRange));
			for (int j = targetList.Count - 1; j >= 0; j--)
			{
				if (GameKit.IsValidEnemy(targetList[j]))
				{
					targetList[j].ProcessDamage(DamageKind.Magic, new SharedStrikeDamage(0, skillParams.magic_damage, true, skillRange), ccData);
				}
			}
			yield break;
		}
	}
}
