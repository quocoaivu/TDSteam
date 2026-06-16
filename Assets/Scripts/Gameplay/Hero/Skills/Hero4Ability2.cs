using System;
using Data;
using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class Hero4Ability2 : HeroAbilityShared
	{
        private int heroID = 4;

        private int skillID = 2;

        private int currentLevel;

        private int currentSkillLevel;

        private bool unLock;
        private int changeToStun;

        private float duration;

        private string description;

        private string buffkey = "Slow";

        public override void Init(HeroEntity heroModel)
		{
			base.Init(heroModel);
			unLock = true;
			this.heroModel = heroModel;
			currentLevel = HeroStore.Instance.GetCurrentHeroLevel(heroID);
			currentSkillLevel = HeroParameterManager.Instance.GetSkillPoint(heroID, currentLevel, skillID);
			HeroAbilitySpec_4_2 heroSkillParameter_4_ = new HeroAbilitySpec_4_2();
			heroSkillParameter_4_ = (HeroAbilitySpec_4_2)HeroAbilitySpec.Instance.GetHeroSkillsParameter(heroID, skillID);
			changeToStun = heroSkillParameter_4_.getParam(currentSkillLevel - 1).change_to_stun;
			duration = (float)heroSkillParameter_4_.getParam(currentSkillLevel - 1).duration / 1000f;
			description = heroSkillParameter_4_.getParam(currentSkillLevel - 1).description;
			InitFXs();
			heroModel.OnHitEnemyEvent += HeroModel_OnHitEnemyEvent;
		}

		private void InitFXs()
		{
			MonoSingleton<FXPool>.Instance.InitFX(FXPool.EFFECT_STUN);
		}

		private void HeroModel_OnHitEnemyEvent()
		{
			if (heroModel.currentTarget)
			{
				EnemyData currentTarget = heroModel.currentTarget;
				if (currentTarget && UnityEngine.Random.Range(0, 100) < changeToStun)
				{
					currentTarget.ProcessEffect(buffkey, 100, duration, DamageVfxType.Stun);
				}
			}
		}
	}
}
