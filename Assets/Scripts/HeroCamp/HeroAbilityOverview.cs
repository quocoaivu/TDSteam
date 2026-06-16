using System;
using Data;
using GameCore;
using Parameter;
using UnityEngine;
using UnityEngine.UI;

namespace HeroCamp
{
	public class HeroAbilityOverview : MonoBehaviour
	{
		public void Init(int heroID, int skillID)
		{
			InitUltimateInformation(heroID, skillID);
		}

		private void InitUltimateInformation(int heroID, int skillID)
		{
			skillName.text = Singleton<HeroSynopsis>.Instance.GetHeroSkillName(heroID, skillID).Replace('@', '\n').Replace('#', '-');
			skillType.text = Singleton<HeroSynopsis>.Instance.GetHeroSkillType(heroID, skillID);
			int numberOfMainParam = HeroAbilitySpec.Instance.GetNumberOfMainParam(heroID, skillID);
			int skillPoint = HeroStore.Instance.GetSkillPoint(heroID, skillID);
			if (numberOfMainParam != 0)
			{
				if (numberOfMainParam != 1)
				{
					if (numberOfMainParam == 2)
					{
						string highLightTextByLevel = GameUtils.GetHighLightTextByLevel(HeroAbilitySpec.Instance.GetMainParams0(heroID, skillID)[0].ToString(), HeroAbilitySpec.Instance.GetMainParams0(heroID, skillID)[1].ToString(), HeroAbilitySpec.Instance.GetMainParams0(heroID, skillID)[2].ToString(), skillPoint);
						string highLightTextByLevel2 = GameUtils.GetHighLightTextByLevel(HeroAbilitySpec.Instance.GetMainParams1(heroID, skillID)[0].ToString(), HeroAbilitySpec.Instance.GetMainParams1(heroID, skillID)[1].ToString(), HeroAbilitySpec.Instance.GetMainParams1(heroID, skillID)[2].ToString(), skillPoint);
						skillDescription.text = string.Format(Singleton<HeroSynopsis>.Instance.GetHeroSkillDescription(heroID, skillID), highLightTextByLevel, highLightTextByLevel2);
					}
				}
				else
				{
					string highLightTextByLevel3 = GameUtils.GetHighLightTextByLevel(HeroAbilitySpec.Instance.GetMainParams0(heroID, skillID)[0].ToString(), HeroAbilitySpec.Instance.GetMainParams0(heroID, skillID)[1].ToString(), HeroAbilitySpec.Instance.GetMainParams0(heroID, skillID)[2].ToString(), skillPoint);
					skillDescription.text = string.Format(Singleton<HeroSynopsis>.Instance.GetHeroSkillDescription(heroID, skillID), highLightTextByLevel3);
				}
			}
			else
			{
				skillDescription.text = Singleton<HeroSynopsis>.Instance.GetHeroSkillDescription(heroID, skillID);
			}
		}

		[SerializeField]
		private Text skillName;

		[SerializeField]
		private Text skillType;

		[SerializeField]
		private Text skillDescription;
	}
}
