using System;
using System.Collections;
using Data;
using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class Hero10Ability2 : HeroAbilityShared
	{
        public GameObject biteFxPrefab;

        public float disTriggerAttack = 2f;

        public float delayDealdam = 0.3f;

        private int heroID = 10;

        private int skillID = 2;

        private int currentLevel;

        private int currentSkillLevel;
        private Hero10Ability2Specs skillParams;

        private float cooldownDuration;

        private float cooldownCountdown;

        private float sqDisTriggerAttack;


        private bool unlocked;
        public override void Init(HeroEntity heroModel)
		{
			base.Init(heroModel);
			this.heroModel = heroModel;
			unlocked = true;
			currentLevel = HeroStore.Instance.GetCurrentHeroLevel(heroID);
			currentSkillLevel = HeroParameterManager.Instance.GetSkillPoint(heroID, currentLevel, skillID);
			skillParams = (HeroAbilitySpec.Instance.GetHeroSkillsParameter(heroID, skillID) as HeroAbilitySpec_10_2).listParam[currentSkillLevel - 1];
			cooldownDuration = (float)skillParams.cooldown_time * 0.001f;
			cooldownCountdown = cooldownDuration * 0.2f;
			sqDisTriggerAttack = disTriggerAttack * disTriggerAttack;
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
				base.StartCoroutine(CastSkill(heroModel.GetCurrentTarget()));
			}
		}

		private IEnumerator CastSkill(EnemyData target)
		{
			Vector3 targetPos = target.transform.position;
			heroModel.SetSpecialStateDuration(delayDealdam * 3f);
			heroModel.SetSpecialStateAnimationName(HeroMotionHandler.animPassiveSkill_1);
			heroModel.GetFsmController().GetCurrentState().OnInput(PhaseInputKind.SpecialState, new object[]
			{
				HeroMotionHandler.animPassiveSkill_1
			});
			yield return new WaitForSeconds(delayDealdam);
			ObjectCache.Spawn(biteFxPrefab, targetPos);
			if (GameKit.IsValidEnemy(target))
			{
				target.ProcessDamage(DamageKind.Melee, new SharedStrikeDamage(skillParams.physic_damage, 0, true, 0f));
			}
			yield break;
		}
	}
}
