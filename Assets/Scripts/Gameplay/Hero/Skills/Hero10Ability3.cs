using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using DG.Tweening;
using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class Hero10Ability3 : HeroAbilityShared
	{
        public GameObject lazeObj;

        public float disTriggerAttack = 1f;

        public float delayDealdam = 0.2f;

        public float beamDuration = 1f;

        private int heroID = 10;

        private int skillID = 3;

        private int currentLevel;

        private int currentSkillLevel;
        private Hero10Ability3Specs skillParams;

        private float cooldownDuration;

        private float cooldownCountdown;

        private float skillRange;

        private float sqDisTriggerAttack;

        private bool unlocked;

        public override void Init(HeroEntity heroModel)
		{
			base.Init(heroModel);
			this.heroModel = heroModel;
			unlocked = true;
			currentLevel = HeroStore.Instance.GetCurrentHeroLevel(heroID);
			currentSkillLevel = HeroParameterManager.Instance.GetSkillPoint(heroID, currentLevel, skillID);
			skillParams = (HeroAbilitySpec.Instance.GetHeroSkillsParameter(heroID, skillID) as HeroAbilitySpec_10_3).listParam[currentSkillLevel - 1];
			cooldownDuration = (float)skillParams.cooldown_time * 0.001f;
			cooldownCountdown = cooldownDuration * 0.5f;
			sqDisTriggerAttack = disTriggerAttack * disTriggerAttack;
			skillRange = (float)skillParams.skill_range / GameRecord.PIXEL_PER_UNIT;
			lazeObj.transform.localScale = new Vector3(1f, 0f, 1f);
			lazeObj.gameObject.SetActive(false);
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
			else if (IsEmptySpecialState() && GameKit.IsValidEnemy(heroModel.GetCurrentTarget()) && !heroModel.GetCurrentTarget().IsAir && MonoSingleton<GameRecord>.Instance.SqrDistance(heroModel.transform.position, heroModel.GetCurrentTarget().transform.position) < sqDisTriggerAttack)
			{
				cooldownCountdown = cooldownDuration;
				base.StartCoroutine(CastSkill(heroModel.GetCurrentTarget().transform.position));
			}
		}

		private IEnumerator CastSkill(Vector3 targetPos)
		{
			float moveToAtkPos = 0.5f;
			heroModel.SetSpecialStateDuration(delayDealdam + beamDuration + moveToAtkPos);
			heroModel.SetSpecialStateAnimationName(HeroMotionHandler.animPassiveSkill_2);
			heroModel.GetFsmController().GetCurrentState().OnInput(PhaseInputKind.SpecialState, new object[]
			{
				HeroMotionHandler.animPassiveSkill_2,
				delayDealdam + beamDuration
			});
			float disToTarget = 1.2f;
			Vector3 attackPos = targetPos + new Vector3((float)((heroModel.transform.localScale.x <= 0f) ? 1 : -1) * disToTarget, 0f, 0f);
			heroModel.transform.DOMove(attackPos, moveToAtkPos, false);
			yield return new WaitForSeconds(moveToAtkPos);
			heroModel.GetAnimationController().ToSpecialState(HeroMotionHandler.animPassiveSkill_2, delayDealdam + beamDuration);
			lazeObj.SetActive(true);
			lazeObj.transform.DOScaleY(1f, delayDealdam);
			yield return new WaitForSeconds(delayDealdam);
			MonoSingleton<LensHandler>.Instance.ShakeNormal();
			OnHitStatusApplier ccData = default(OnHitStatusApplier);
			ccData.buffKey = "Stun";
			ccData.damageFXType = DamageVfxType.Stun;
			ccData.debuffChance = 100;
			ccData.debuffEffectDuration = (float)skillParams.stun_duration * 0.001f;
			List<EnemyData> targetList = GameKit.GetListEnemiesInRange(targetPos, new SharedStrikeDamage(0, skillParams.magic_damage, false, skillRange));
			for (int i = targetList.Count - 1; i >= 0; i--)
			{
				if (GameKit.IsValidEnemy(targetList[i]))
				{
					targetList[i].ProcessDamage(DamageKind.Magic, new SharedStrikeDamage(0, skillParams.magic_damage, true, skillRange), ccData);
				}
			}
			yield return new WaitForSeconds(beamDuration);
			lazeObj.transform.DOScaleY(0f, delayDealdam);
			yield break;
		}
	}
}
