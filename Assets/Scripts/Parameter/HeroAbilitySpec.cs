using System;
using System.Collections.Generic;

namespace Parameter
{
	public class HeroAbilitySpec
	{
		public static HeroAbilitySpec Instance
		{
			get
			{
				if (HeroAbilitySpec._instance == null)
				{
					HeroAbilitySpec._instance = new HeroAbilitySpec();
				}
				return HeroAbilitySpec._instance;
			}
		}

		public void SetHeroSkillParameter(int heroID, int skillID, HeroAbilitySpecBasic heroSkillParameterBasic)
		{
			parameterDictionary[heroID][skillID] = heroSkillParameterBasic;
		}

		public HeroAbilitySpecBasic GetHeroSkillsParameter(int heroID, int skillID)
		{
			return parameterDictionary[heroID][skillID];
		}

		public int GetNumberOfMainParam(int heroID, int skillID)
		{
			int num = 0;
			if (parameterDictionary[heroID][skillID].mainParam0.Count > 0)
			{
				num++;
			}
			if (parameterDictionary[heroID][skillID].mainParam1.Count > 0)
			{
				num++;
			}
			return num;
		}

		public List<float> GetMainParams0(int heroID, int skillID)
		{
			return parameterDictionary[heroID][skillID].mainParam0;
		}

		public List<float> GetMainParams1(int heroID, int skillID)
		{
			return parameterDictionary[heroID][skillID].mainParam1;
		}

		private static HeroAbilitySpec _instance;

		[UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			_instance = null;
		}
		private Dictionary<int, HeroAbilitySpecBasic[]> parameterDictionary = new Dictionary<int, HeroAbilitySpecBasic[]>
		{
			{
				0,
				new HeroAbilitySpecBasic[4]
			},
			{
				1,
				new HeroAbilitySpecBasic[4]
			},
			{
				2,
				new HeroAbilitySpecBasic[4]
			},
			{
				3,
				new HeroAbilitySpecBasic[4]
			},
			{
				4,
				new HeroAbilitySpecBasic[4]
			},
			{
				5,
				new HeroAbilitySpecBasic[4]
			},
			{
				6,
				new HeroAbilitySpecBasic[4]
			},
			{
				7,
				new HeroAbilitySpecBasic[4]
			},
			{
				8,
				new HeroAbilitySpecBasic[4]
			},
			{
				9,
				new HeroAbilitySpecBasic[4]
			},
			{
				10,
				new HeroAbilitySpecBasic[4]
			},
			{
				11,
				new HeroAbilitySpecBasic[4]
			}
		};
	}
}
