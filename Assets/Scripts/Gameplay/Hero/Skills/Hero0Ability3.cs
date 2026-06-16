using System;
using System.Collections.Generic;
using Data;
using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class Hero0Ability3 : HeroAbilityShared
	{
        private int heroID;

        private int skillID = 3;

        private int currentLevel;

        private int currentSkillLevel;

        private bool unLock;

        private float skillRange;

        private float armorPerUnit;

        private float armorMax;

        [SerializeField]
        private float timeToCheck;

        private float timeTracking;

        private float armorAmount;


        private List<EnemyData> inRangeEnemies = new List<EnemyData>();
        public override void Init(HeroEntity heroModel)
		{
			base.Init(heroModel);
			unLock = true;
			currentLevel = HeroStore.Instance.GetCurrentHeroLevel(heroID);
			currentSkillLevel = HeroParameterManager.Instance.GetSkillPoint(heroID, currentLevel, skillID);
			HeroAbilitySpec_0_3 heroSkillParameter_0_ = new HeroAbilitySpec_0_3();
			heroSkillParameter_0_ = (HeroAbilitySpec_0_3)HeroAbilitySpec.Instance.GetHeroSkillsParameter(heroID, skillID);
			skillRange = (float)heroSkillParameter_0_.getParam(currentSkillLevel - 1).skill_range / GameRecord.PIXEL_PER_UNIT;
			armorPerUnit = (float)heroSkillParameter_0_.getParam(currentSkillLevel - 1).armor_per_unit / 100f;
			armorMax = (float)heroSkillParameter_0_.getParam(currentSkillLevel - 1).armor_max / 100f;
		}

		public void Init(bool unlock, HeroEntity heroModel, int _skillRange, int _armorPerUnit, int _armorMax)
		{
			this.heroModel = heroModel;
			skillRange = (float)_skillRange / GameRecord.PIXEL_PER_UNIT;
			armorPerUnit = (float)_armorPerUnit / 100f;
			armorMax = (float)_armorMax / 100f;
		}

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
			if (timeTracking == 0f)
			{
				GetEnemies();
			}
			timeTracking = Mathf.MoveTowards(timeTracking, 0f, Time.deltaTime);
		}

		public void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.cyan;
			Gizmos.DrawWireSphere(base.transform.position, skillRange);
		}

		private void GetEnemies()
		{
			MonoSingleton<GameRecord>.Instance.GetInRangeEnemies(heroModel.transform.position, skillRange, inRangeEnemies);
			timeTracking = timeToCheck;
			AddPassiveArmor();
		}

		private void AddPassiveArmor()
		{
			armorAmount = (float)inRangeEnemies.Count * armorPerUnit;
			armorAmount = Mathf.Clamp(armorAmount, 0f, armorMax);
			heroModel.HeroHealthController.CurrentPhysicsArmor = heroModel.HeroHealthController.OriginPhysicsArmor + armorAmount;
			heroModel.HeroHealthController.CurrentMagicArmor = heroModel.HeroHealthController.OriginMagicArmor + armorAmount;
		}
	}
}
