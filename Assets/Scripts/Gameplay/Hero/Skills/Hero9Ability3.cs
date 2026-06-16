using System;
using System.Collections;
using Data;
using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class Hero9Ability3 : HeroAbilityShared
	{
        private int heroID = 9;

        private int skillID = 3;

        private int currentLevel;

        private int currentSkillLevel;

        private bool unLock;
        private HeroSpec heroParameter;

        private int numberClone;

        private float parameter_scale;

        private float duration;

        private float timeTracking;

        private float cooldownTime;

        private string description;

        [SerializeField]
        private float animDuration;

        [SerializeField]
        private float delayTimeCastSkill;

        [SerializeField]

        private Transform clonePosition;
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
			heroParameter = HeroParameterManager.Instance.GetHeroParameter(heroID, currentLevel);
			HeroAbilitySpec_9_3 heroSkillParameter_9_ = new HeroAbilitySpec_9_3();
			heroSkillParameter_9_ = (HeroAbilitySpec_9_3)HeroAbilitySpec.Instance.GetHeroSkillsParameter(heroID, skillID);
			numberClone = heroSkillParameter_9_.getParam(currentSkillLevel - 1).number_clone;
			parameter_scale = (float)heroSkillParameter_9_.getParam(currentSkillLevel - 1).parameter_Scale / 100f;
			duration = (float)heroSkillParameter_9_.getParam(currentSkillLevel - 1).duration / 1000f;
			cooldownTime = (float)heroSkillParameter_9_.getParam(currentSkillLevel - 1).cooldown_time / 1000f;
			description = heroSkillParameter_9_.getParam(currentSkillLevel - 1).description;
			InitFXs();
			timeTracking = cooldownTime * 0.1f;
			heroModel.OnHitEnemyEvent += HeroModel_OnHitEnemyEvent;
		}

		private void InitFXs()
		{
			MonoSingleton<AllyPool>.Instance.PushAlliesToPool(109, 109, 0);
		}

		private bool IsCooldownDone()
		{
			return timeTracking == 0f;
		}

		private void HeroModel_OnHitEnemyEvent()
		{
			if (IsCooldownDone() && IsEmptySpecialState() && unLock)
			{
				base.StartCoroutine(CastSkill());
			}
		}

		private IEnumerator CastSkill()
		{
			heroModel.SetSpecialStateDuration(animDuration);
			heroModel.SetSpecialStateAnimationName(HeroMotionHandler.animPassiveSkill_2);
			heroModel.GetFsmController().GetCurrentState().OnInput(PhaseInputKind.SpecialState, new object[]
			{
				HeroMotionHandler.animPassiveSkill_2
			});
			timeTracking = cooldownTime;
			yield return new WaitForSeconds(delayTimeCastSkill);
			MinionEntity clone = MonoSingleton<AllyPool>.Instance.GetAlly(109, 109);
			clone.InitFromHero(heroParameter, parameter_scale, duration);
			clone.gameObject.SetActive(true);
			clone.transform.position = clonePosition.position;
			clone.SetAssignedPosition(clonePosition.position);
			yield break;
		}
	}
}
