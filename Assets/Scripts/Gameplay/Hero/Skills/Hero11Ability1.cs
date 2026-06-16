using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class Hero11Ability1 : HeroAbilityShared
	{
        public float novaCrashDuration;

        public float delayDealdam;

        public float disTriggerAttack = 1f;

        private int heroID = 11;

        private int skillID = 1;

        private int currentLevel;

        private int currentSkillLevel;
        private Hero11Ability1Specs skillParams;

        private float cooldownDuration;

        private float cooldownCountdown;

        private float skillRange;

        private float sqDisTriggerAttack;

        private bool unlocked;

        public override void Init(HeroEntity heroModel)
		{
			base.Init(heroModel);
			unlocked = true;
			this.heroModel = heroModel;
			currentLevel = HeroStore.Instance.GetCurrentHeroLevel(heroID);
			currentSkillLevel = HeroParameterManager.Instance.GetSkillPoint(heroID, currentLevel, skillID);
			skillParams = (HeroAbilitySpec.Instance.GetHeroSkillsParameter(heroID, skillID) as HeroAbilitySpec_11_1).listParam[currentSkillLevel - 1];
			cooldownDuration = (float)skillParams.cooldown_time * 0.001f;
			cooldownCountdown = cooldownDuration * 0.1f;
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
				base.StartCoroutine(CastSkill());
			}
		}

		private IEnumerator CastSkill()
		{
			heroModel.SetSpecialStateDuration(novaCrashDuration);
			heroModel.SetSpecialStateAnimationName(HeroMotionHandler.animPassiveSkill_0);
			heroModel.GetFsmController().GetCurrentState().OnInput(PhaseInputKind.SpecialState, new object[]
			{
				HeroMotionHandler.animPassiveSkill_0
			});
			yield return new WaitForSeconds(delayDealdam);
			MonoSingleton<LensHandler>.Instance.ShakeNormal();
			List<EnemyData> targetList = GameKit.GetListEnemiesInRange(heroModel.transform.position, new SharedStrikeDamage(0, skillParams.magic_damage, false, skillRange));
			for (int i = targetList.Count - 1; i >= 0; i--)
			{
				if (GameKit.IsValidEnemy(targetList[i]))
				{
					targetList[i].ProcessDamage(DamageKind.Magic, new SharedStrikeDamage(0, skillParams.magic_damage, true, skillRange));
				}
			}
			yield break;
		}
	}
}
