using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class Hero9Ability0 : HeroAbilityShared
	{
        private int heroID = 9;

        private int skillID;

        private int currentLevel;

        private int currentSkillLevel;

        private bool unLock;
        private int numberOfMinion;

        private int physicsDamage;

        private int magicDamage;

        private float minionAttackRange;

        private float minionAttackCooldown;

        private float minionLifeTime;

        private float skillRange;

        private float timeTracking;

        private float cooldownTime;

        private string description;

        private string useType;

        [SerializeField]
        private float animDuration;

        [SerializeField]
        private float delayTimeToAttack;

        [SerializeField]
        private GameObject weaponStationPrefab;

        [SerializeField]
        private string weaponname;

        [Space]
        [SerializeField]

        private PointPatternsCluster pointPatternsGroup;
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
			HeroAbilitySpec_9_0 heroSkillParameter_9_ = new HeroAbilitySpec_9_0();
			heroSkillParameter_9_ = (HeroAbilitySpec_9_0)HeroAbilitySpec.Instance.GetHeroSkillsParameter(heroID, skillID);
			numberOfMinion = heroSkillParameter_9_.getParam(currentSkillLevel - 1).number_of_minion;
			physicsDamage = heroSkillParameter_9_.getParam(currentSkillLevel - 1).physics_damage;
			magicDamage = heroSkillParameter_9_.getParam(currentSkillLevel - 1).magic_damage;
			minionAttackRange = (float)heroSkillParameter_9_.getParam(currentSkillLevel - 1).minion_attack_range / GameRecord.PIXEL_PER_UNIT;
			minionAttackCooldown = (float)heroSkillParameter_9_.getParam(currentSkillLevel - 1).minion_attack_cooldown / 1000f;
			minionLifeTime = (float)heroSkillParameter_9_.getParam(currentSkillLevel - 1).minion_lifetime / 1000f;
			skillRange = (float)heroSkillParameter_9_.getParam(currentSkillLevel - 1).skill_range / GameRecord.PIXEL_PER_UNIT;
			cooldownTime = (float)heroSkillParameter_9_.getParam(currentSkillLevel - 1).cooldown_time / 1000f;
			description = heroSkillParameter_9_.getParam(currentSkillLevel - 1).description;
			useType = heroSkillParameter_9_.getParam(currentSkillLevel - 1).use_type;
			InitFXs();
			timeTracking = cooldownTime;
		}

		private void InitFXs()
		{
			MonoSingleton<TowerPool>.Instance.InitWeaponStation(weaponStationPrefab);
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
			MonoSingleton<GameplayUIHeroDirector>.Instance.listSelectHeroSkillButton[heroID].DoCooldown();
			heroModel.SetSpecialStateDuration(animDuration);
			heroModel.SetSpecialStateAnimationName(HeroMotionHandler.animActiveSkill);
			heroModel.GetFsmController().GetCurrentState().OnInput(PhaseInputKind.SpecialState, new object[]
			{
				HeroMotionHandler.animActiveSkill
			});
			yield return new WaitForSeconds(delayTimeToAttack);
			List<Transform> listPosByPattern = pointPatternsGroup.getPoints(numberOfMinion);
			Vector2 patternParentPos = pointPatternsGroup.getParentPointsPosition(numberOfMinion);
			for (int i = 0; i < numberOfMinion; i++)
			{
				ArmamentBay weapon = MonoSingleton<TowerPool>.Instance.GetWeaponStation(weaponname);
				Vector2 minionPosition = (Vector2)listPosByPattern[i].position + (targetPosition - patternParentPos);
				weapon.transform.position = minionPosition;
				weapon.InitFromHero(heroModel, physicsDamage, minionAttackCooldown, minionAttackRange, minionLifeTime);
				yield return new WaitForSeconds(0.1f);
			}
			yield break;
		}
	}
}
