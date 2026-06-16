using System;
using System.Collections.Generic;
using Data;
using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class HeroAbilityHandler : HeroHandler
	{
        [SerializeField]
        private List<HeroAbilityShared> listHeroSkill = new List<HeroAbilityShared>();


        public void InitHeroSkills()
		{
			int currentHeroLevel = HeroStore.Instance.GetCurrentHeroLevel(base.HeroModel.HeroID);
			for (int i = 0; i < 4; i++)
			{
				if (HeroParameterManager.Instance.GetSkillPoint(base.HeroModel.HeroID, currentHeroLevel, i) > 0)
				{
					listHeroSkill[i].Init(base.HeroModel);
				}
			}
		}

		public void InitPetSkills()
		{
			foreach (HeroAbilityShared heroSkillCommon in listHeroSkill)
			{
				heroSkillCommon.Init(base.HeroModel);
			}
		}

		public float GetActiveSkillCooldownTime()
		{
			float cooldownTime = listHeroSkill[0].GetCooldownTime();
			return listHeroSkill[0].GetCooldownTime();
		}

		public string GetActiveSkillUseType()
		{
			return listHeroSkill[0].GetUseType();
		}

		public override void OnReturnPool()
		{
			base.OnReturnPool();
			foreach (HeroAbilityShared heroSkillCommon in listHeroSkill)
			{
				heroSkillCommon.OnHeroReturnPool();
			}
		}

		public HeroAbilityShared GetSkill(int skillIndex)
		{
			if (skillIndex < listHeroSkill.Count)
			{
				return listHeroSkill[skillIndex];
			}
			return null;
		}
	}
}
