using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class Hero9Ability1 : HeroAbilityShared
	{

        private int heroID = 9;

        private int skillID = 1;

        private int currentLevel;

        private int currentSkillLevel;

        private bool unLock;
        private int enemyAffected;

        private float knocbackDistance;

        private float knocbackDuration;

        private float skillRange;

        private float timeTracking;

        private float cooldownTime;

        private string description;

        [SerializeField]
        private float animDuration;

        [SerializeField]
        private float delayTimeCastSkill;

        public override void Update()
		{
			base.Update();
			if (!MonoSingleton<GameRecord>.Instance.IsGameStart)
			{
				return;
			}
			if (!unLock)
			{
				return;
			}
			if (heroModel && !heroModel.IsAlive)
			{
				return;
			}
			timeTracking = Mathf.MoveTowards(timeTracking, 0f, Time.deltaTime);
		}

		public override void Init(HeroEntity heroModel)
		{
			base.Init(heroModel);
			unLock = true;
			this.heroModel = heroModel;
			currentLevel = HeroStore.Instance.GetCurrentHeroLevel(heroID);
			currentSkillLevel = HeroParameterManager.Instance.GetSkillPoint(heroID, currentLevel, skillID);
			HeroAbilitySpec_9_1 heroSkillParameter_9_ = new HeroAbilitySpec_9_1();
			heroSkillParameter_9_ = (HeroAbilitySpec_9_1)HeroAbilitySpec.Instance.GetHeroSkillsParameter(heroID, skillID);
			enemyAffected = heroSkillParameter_9_.getParam(currentSkillLevel - 1).enemy_affected;
			knocbackDistance = (float)heroSkillParameter_9_.getParam(currentSkillLevel - 1).knock_back_distance / GameRecord.PIXEL_PER_UNIT;
			knocbackDuration = (float)heroSkillParameter_9_.getParam(currentSkillLevel - 1).knock_back_duration / 1000f;
			skillRange = (float)heroSkillParameter_9_.getParam(currentSkillLevel - 1).skill_range / GameRecord.PIXEL_PER_UNIT;
			cooldownTime = (float)heroSkillParameter_9_.getParam(currentSkillLevel - 1).cooldown_time / 1000f;
			description = heroSkillParameter_9_.getParam(currentSkillLevel - 1).description;
			InitFXs();
			timeTracking = cooldownTime * 0.4f;
			heroModel.OnHitEnemyEvent += HeroModel_OnHitEnemyEvent;
		}

		private void HeroModel_OnHitEnemyEvent()
		{
			if (IsCooldownDone() && IsEmptySpecialState() && unLock)
			{
				base.StartCoroutine(CastSkill());
			}
		}

		private void InitFXs()
		{
			MonoSingleton<FXPool>.Instance.InitFX(FXPool.EFFECT_POISON0);
		}

		private bool IsCooldownDone()
		{
			return timeTracking == 0f;
		}

		private IEnumerator CastSkill()
		{
			heroModel.SetSpecialStateDuration(animDuration);
			heroModel.SetSpecialStateAnimationName(HeroMotionHandler.animPassiveSkill_0);
			heroModel.GetFsmController().GetCurrentState().OnInput(PhaseInputKind.SpecialState, new object[]
			{
				HeroMotionHandler.animPassiveSkill_0
			});
			timeTracking = cooldownTime;
			yield return new WaitForSeconds(delayTimeCastSkill);
			List<EnemyData> listEnemyInRange = GameKit.GetListEnemiesInRange(base.gameObject, new SharedStrikeDamage(0, 0, skillRange));
			if (listEnemyInRange.Count > 0)
			{
				if (enemyAffected < listEnemyInRange.Count)
				{
					for (int i = 0; i < enemyAffected; i++)
					{
						base.StartCoroutine(PushbackSequence(listEnemyInRange[i]));
					}
				}
				else
				{
					for (int j = 0; j < listEnemyInRange.Count; j++)
					{
						base.StartCoroutine(PushbackSequence(listEnemyInRange[j]));
					}
				}
			}
			yield break;
		}

		private IEnumerator PushbackSequence(EnemyData target)
		{
			yield return new WaitForSeconds(0.1f);
			float pushTimeTracking = knocbackDuration;
			if (GameKit.IsValidEnemy(target))
			{
				target.EnemyAnimationController.TurnBack();
				VisualEffectInstance effect = MonoSingleton<FXPool>.Instance.GetEffect(FXPool.EFFECT_POISON0);
				effect.Init(0.77f, target.transform);
			}
			while (pushTimeTracking > 0f)
			{
				pushTimeTracking -= Time.deltaTime;
				if (GameKit.IsValidEnemy(target) && !target.OriginalParameter.isBoss)
				{
					target.SetSpecialStateDuration(pushTimeTracking);
					target.enemyFsmController.GetCurrentState().OnInput(PhaseInputKind.SpecialState, new object[]
					{
						EnemyAnimation.animRunRight
					});
					float num = (float)target.OriginalParameter.speed / GameRecord.PIXEL_PER_UNIT * Time.deltaTime;
					LineDirector.Current.RequestMove(target, target.monsterPathData, -num, false, 0f);
				}
				yield return null;
			}
			yield return new WaitForSeconds(0.1f);
			yield break;
		}
	}
}
