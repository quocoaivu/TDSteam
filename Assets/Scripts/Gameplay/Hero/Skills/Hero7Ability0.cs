using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using DG.Tweening;
using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class Hero7Ability0 : HeroAbilityShared
	{
        private int heroID = 7;

        private int skillID;

        private int currentLevel;

        private int currentSkillLevel;

        private bool unLock;
        private int physicsDamage;

        private int magicDamage;

        private float skillRange;

        private float cooldownTime;

        private string description;

        private string useType;

        [SerializeField]
        private float animDuration;

        [SerializeField]
        private float delayTimeToAttack;

        [SerializeField]
        private float delayTimeBetweenAttackFrame;

        [SerializeField]
        private int attackFrame;

        [SerializeField]
        private AoeDamageCaster damageToAOERange;

        private void Start()
		{
			HerosDirector.Instance.onCastHeroSkillToAssignedPosition += Instance_onCastHeroSkillToAssignedPosition;
		}

		private void OnDestroy()
		{
			HerosDirector.Instance.onCastHeroSkillToAssignedPosition -= Instance_onCastHeroSkillToAssignedPosition;
		}

		private void Instance_onCastHeroSkillToAssignedPosition(int heroID, Vector2 targetPosition)
		{
			if (this.heroID == heroID)
			{
				base.StartCoroutine(CastSkill(targetPosition));
			}
		}

		public override void Init(HeroEntity heroModel)
		{
			base.Init(heroModel);
			unLock = true;
			this.heroModel = heroModel;
			currentLevel = HeroStore.Instance.GetCurrentHeroLevel(heroID);
			currentSkillLevel = HeroParameterManager.Instance.GetSkillPoint(heroID, currentLevel, skillID);
			HeroAbilitySpec_7_0 heroSkillParameter_7_ = new HeroAbilitySpec_7_0();
			heroSkillParameter_7_ = (HeroAbilitySpec_7_0)HeroAbilitySpec.Instance.GetHeroSkillsParameter(heroID, skillID);
			physicsDamage = heroSkillParameter_7_.getParam(currentSkillLevel - 1).physics_damage;
			magicDamage = heroSkillParameter_7_.getParam(currentSkillLevel - 1).magic_damage;
			skillRange = (float)heroSkillParameter_7_.getParam(currentSkillLevel - 1).skill_range / GameRecord.PIXEL_PER_UNIT;
			cooldownTime = (float)heroSkillParameter_7_.getParam(currentSkillLevel - 1).cooldown_time / 1000f;
			description = heroSkillParameter_7_.getParam(currentSkillLevel - 1).description;
			useType = heroSkillParameter_7_.getParam(currentSkillLevel - 1).use_type;
			InitFXs();
		}

		private void InitFXs()
		{
			MonoSingleton<FXPool>.Instance.InitFX(FXPool.EFFECT_BLOOD_SPREAD, 10);
		}

		public override float GetCooldownTime()
		{
			return cooldownTime;
		}

		public override string GetUseType()
		{
			return useType;
		}

		private IEnumerator CastSkill(Vector2 targetPosition)
		{
			if (!IsEmptySpecialState())
			{
				yield return null;
			}
			heroModel.transform.DOMove(targetPosition, 0f, false).SetEase(Ease.Linear).OnComplete(new TweenCallback(MoveToAssignedPositionComplete));
			yield break;
		}

		private void MoveToAssignedPositionComplete()
		{
			base.StartCoroutine(CastSkill());
		}

		private IEnumerator CastSkill()
		{
			MonoSingleton<GameplayUIHeroDirector>.Instance.listSelectHeroSkillButton[heroID].DoCooldown();
			List<EnemyData> listEnemiesInRange = GameKit.GetListEnemiesInRange(base.gameObject, new SharedStrikeDamage(physicsDamage, magicDamage, skillRange));
			if (listEnemiesInRange.Count > 0)
			{
				UnityEngine.Debug.Log("co enemy!");
				heroModel.SetSpecialStateDuration(animDuration);
				heroModel.SetSpecialStateAnimationName(HeroMotionHandler.animActiveSkill);
				heroModel.GetFsmController().GetCurrentState().OnInput(PhaseInputKind.SpecialState, new object[]
				{
					HeroMotionHandler.animActiveSkill
				});
				yield return new WaitForSeconds(delayTimeToAttack);
				for (int i = 0; i < attackFrame; i++)
				{
					damageToAOERange.CastDamage(DamageKind.Range, new SharedStrikeDamage(physicsDamage / attackFrame, magicDamage / attackFrame, skillRange, true, true));
					yield return new WaitForSeconds(delayTimeBetweenAttackFrame);
				}
				foreach (EnemyData enemyModel in listEnemiesInRange)
				{
					VisualEffectInstance effect = MonoSingleton<FXPool>.Instance.GetEffect(FXPool.EFFECT_BLOOD_SPREAD);
					effect.Init(0.33f, enemyModel.transform);
				}
			}
			else
			{
				UnityEngine.Debug.Log("khong co enemy!");
			}
			heroModel.SetAssignedPosition(heroModel.transform.position);
			yield break;
		}
	}
}
