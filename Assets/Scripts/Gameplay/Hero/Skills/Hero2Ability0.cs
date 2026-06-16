using System;
using System.Collections;
using Data;
using Parameter;
using Tutorial;
using UnityEngine;

namespace Gameplay
{
	public class Hero2Ability0 : HeroAbilityShared
	{

        private int heroID = 2;

        private int skillID;

        private int currentLevel;

        private int currentSkillLevel;

        private bool unLock;

        private HeroSpec heroParameter;

        private int numberClone;

        private float parameter_scale;

        private float duration;

        private float cooldownTime;

        private string description;

        private string useType;

        private RaycastHit2D hit;

        [SerializeField]

        private LayerMask avaiableMovingLayerMask;
        private void Start()
		{
			HerosDirector.Instance.onCastHeroSkillToAssignedPosition += Instance_onCastHeroSkillToAssignedPosition;
		}

		private new void Update()
		{
			if (!unLock)
			{
				return;
			}
			if (HerosDirector.Instance.HeroSkillIDChoosing == heroID)
			{
				GameplayTutorialDirector.Instance.TutorialUseHeroSkill.TryMoveToStep(1);
			}
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
			currentLevel = HeroStore.Instance.GetCurrentHeroLevel(heroID);
			currentSkillLevel = HeroParameterManager.Instance.GetSkillPoint(heroID, currentLevel, skillID);
			heroParameter = HeroParameterManager.Instance.GetHeroParameter(heroID, currentLevel);
			HeroAbilitySpec_2_0 heroSkillParameter_2_ = new HeroAbilitySpec_2_0();
			heroSkillParameter_2_ = (HeroAbilitySpec_2_0)HeroAbilitySpec.Instance.GetHeroSkillsParameter(heroID, skillID);
			numberClone = heroSkillParameter_2_.getParam(currentSkillLevel - 1).number_clone;
			parameter_scale = (float)heroSkillParameter_2_.getParam(currentSkillLevel - 1).parameter_Scale / 100f;
			duration = (float)heroSkillParameter_2_.getParam(currentSkillLevel - 1).duration / 1000f;
			cooldownTime = (float)heroSkillParameter_2_.getParam(currentSkillLevel - 1).cooldown_time / 1000f;
			description = heroSkillParameter_2_.getParam(currentSkillLevel - 1).description;
			useType = heroSkillParameter_2_.getParam(currentSkillLevel - 1).use_type;
			MonoSingleton<AllyPool>.Instance.PushAlliesToPool(102, 102, 0);
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
			CreateClones(targetPosition);
			heroModel.SetSpecialStateDuration(0.5f);
			heroModel.SetSpecialStateAnimationName(HeroMotionHandler.animActiveSkill);
			heroModel.GetFsmController().GetCurrentState().OnInput(PhaseInputKind.SpecialState, new object[]
			{
				HeroMotionHandler.animActiveSkill
			});
			yield break;
		}

		private void CreateClones(Vector2 targetPosition)
		{
			UnityEngine.Debug.Log("Hero 2 Cast Skill 0");
			MonoSingleton<GameRecord>.Instance.PlayerKnowHowToUseSkill = true;
			GameplayTutorialDirector.Instance.TutorialUseHeroSkill.SetTutorialPassed();
			int num = 0;
			for (int i = 0; i < numberClone; i++)
			{
				MinionEntity allyModel = MonoSingleton<AllyPool>.Instance.GetAlly(102, 102);
				allyModel.transform.position = targetPosition;
				allyModel.InitFromHero(heroParameter, parameter_scale, duration);
				allyModel.gameObject.SetActive(true);
				num++;
			}
			MonoSingleton<GameplayUIHeroDirector>.Instance.listSelectHeroSkillButton[2].DoCooldown();
		}
	}
}
