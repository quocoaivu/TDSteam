using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class Hero4Ability3 : HeroAbilityShared
	{
        public CFX_AutoPlayAndDestruction attackUpFxSample;

        private int heroID = 4;

        private int skillID = 3;

        private int currentLevel;

        private int currentSkillLevel;

        private bool unLock;
        private int generalAttackDamageBonus;

        private int attackSpeedBonus;

        private int movementSpeedBonus;

        private float duration;

        private float timeTracking;

        private float cooldownTime;

        private string description;

        private string increasePhysicsDamageBuffkey = "IncreaseDamagePhysics";

        private string increaseMagicDamageBuffkey = "IncreaseDamageMagic";

        private string increaseAttackSpeedBuffkey = "IncreaseAttackSpeed";

        private string increaseMovementSpeedBuffkey = "IncreaseMovementSpeed";

        public override void Update()
		{
			base.Update();
			if (!unLock)
			{
				return;
			}
			if (heroModel && !heroModel.IsAlive)
			{
				return;
			}
			if (IsCooldownDone())
			{
				base.StartCoroutine(CastSkill());
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
			HeroAbilitySpec_4_3 heroSkillParameter_4_ = new HeroAbilitySpec_4_3();
			heroSkillParameter_4_ = (HeroAbilitySpec_4_3)HeroAbilitySpec.Instance.GetHeroSkillsParameter(heroID, skillID);
			generalAttackDamageBonus = heroSkillParameter_4_.getParam(currentSkillLevel - 1).general_attack_damage_bonus;
			attackSpeedBonus = heroSkillParameter_4_.getParam(currentSkillLevel - 1).attack_speed_bonus;
			movementSpeedBonus = heroSkillParameter_4_.getParam(currentSkillLevel - 1).movement_speed_bonus;
			duration = (float)heroSkillParameter_4_.getParam(currentSkillLevel - 1).duration / 1000f;
			cooldownTime = (float)heroSkillParameter_4_.getParam(currentSkillLevel - 1).cooldown_time / 1000f;
			description = heroSkillParameter_4_.getParam(currentSkillLevel - 1).description;
			InitFXs();
			timeTracking = cooldownTime * UnityEngine.Random.Range(0.7f, 0.9f);
		}

		private void InitFXs()
		{
			MonoSingleton<FXPool>.Instance.InitFX(FXPool.INFERNO_GOLEM_AURA);
		}

		private bool IsCooldownDone()
		{
			return timeTracking == 0f;
		}

		private IEnumerator CastSkill()
		{
			if (!IsEmptySpecialState())
			{
				yield return null;
			}
			heroModel.SetSpecialStateDuration(1f);
			heroModel.SetSpecialStateAnimationName(HeroMotionHandler.animPassiveSkill_2);
			heroModel.GetFsmController().GetCurrentState().OnInput(PhaseInputKind.SpecialState, new object[]
			{
				HeroMotionHandler.animPassiveSkill_2
			});
			AbilitiesBuff();
			yield break;
		}

		private void AbilitiesBuff()
		{
			List<CharacterEntity> listActiveAlly = MonoSingleton<GameRecord>.Instance.ListActiveAlly;
			foreach (CharacterEntity characterModel in listActiveAlly)
			{
				characterModel.BuffsHolder.AddBuff(increasePhysicsDamageBuffkey, new BuffStatus(true, (float)generalAttackDamageBonus, duration), BuffStackRule.ChooseMax, BuffStackRule.ChooseMax);
				characterModel.BuffsHolder.AddBuff(increaseMagicDamageBuffkey, new BuffStatus(true, (float)generalAttackDamageBonus, duration), BuffStackRule.ChooseMax, BuffStackRule.ChooseMax);
				characterModel.BuffsHolder.AddBuff(increaseAttackSpeedBuffkey, new BuffStatus(true, (float)attackSpeedBonus, duration), BuffStackRule.ChooseMax, BuffStackRule.ChooseMax);
				characterModel.BuffsHolder.AddBuff(increaseMovementSpeedBuffkey, new BuffStatus(true, (float)movementSpeedBonus, duration), BuffStackRule.ChooseMax, BuffStackRule.ChooseMax);
				VisualEffectInstance effect = MonoSingleton<FXPool>.Instance.GetEffect(FXPool.INFERNO_GOLEM_AURA);
				effect.transform.position = characterModel.transform.position;
				effect.Init(duration, characterModel.transform);
				CFX_AutoPlayAndDestruction cfx_AutoPlayAndDestruction = ObjectCache.Spawn<CFX_AutoPlayAndDestruction>(attackUpFxSample, characterModel.BuffsHolder.transform, new Vector3(0f, characterModel.GetCharacterHeight() + 0.05f, 0f));
				cfx_AutoPlayAndDestruction.SetTimer(duration + 1f);
			}
			timeTracking = cooldownTime;
		}
	}
}
