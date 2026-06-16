using System;
using System.Collections.Generic;
using Data;

namespace Parameter
{
	public class UnlockHeroSpec : Singleton<UnlockHeroSpec>
	{
		public void SetHeroUnlockParamParameter(HeroUnlockSpecs heroUnlockParam)
		{
			int count = listHeroUnlockParam.Count;
			if (count <= heroUnlockParam.id)
			{
				listHeroUnlockParam.Add(heroUnlockParam);
			}
		}

		public bool IsHeroUnlockByPlay(int heroID)
		{
			bool result = false;
			if (heroID < listHeroUnlockParam.Count && heroID >= 0)
			{
				result = (listHeroUnlockParam[heroID].isUnlockByPlay == 1);
			}
			return result;
		}

		public int GetMapIDToUnlockHero(int heroID)
		{
			if (heroID < listHeroUnlockParam.Count && heroID >= 0)
			{
				return listHeroUnlockParam[heroID].mapIDToUnlock;
			}
			return -1;
		}

		public bool IsHeroAvailableToUnlock(int heroID)
		{
			return MapProgressStore.Instance.GetMapIDUnlocked() >= GetMapIDToUnlockHero(heroID);
		}

		public bool IsHeroUnlockByGem(int heroID)
		{
			bool result = false;
			if (heroID < listHeroUnlockParam.Count && heroID >= 0)
			{
				result = (listHeroUnlockParam[heroID].isUnlockByGem == 1);
			}
			return result;
		}

		public bool IsHeroAvailableToBuy(int heroID)
		{
			return PlayerCurrencyStore.Instance.GetCurrentGem() >= GetGemAmountToUnlockHero(heroID);
		}

		public int GetGemAmountToUnlockHero(int heroID)
		{
			if (heroID < listHeroUnlockParam.Count && heroID >= 0)
			{
				return listHeroUnlockParam[heroID].gemAmountToUnlock;
			}
			return -1;
		}

		public HeroUnlockSpecs GetWeapon(int heroID)
		{
			return listHeroUnlockParam[heroID];
		}

		public List<int> GetListHeroNotOwnedByPlay()
		{
			List<int> list = new List<int>();
			foreach (HeroUnlockSpecs heroUnlockParam in listHeroUnlockParam)
			{
				if (heroUnlockParam.isUnlockByPlay == 1 && !HeroStore.Instance.IsHeroOwned(heroUnlockParam.id))
				{
					list.Add(heroUnlockParam.id);
				}
			}
			return list;
		}

		public List<int> GetListHeroNotOwnedByGem()
		{
			List<int> list = new List<int>();
			foreach (HeroUnlockSpecs heroUnlockParam in listHeroUnlockParam)
			{
				if (heroUnlockParam.isUnlockByGem == 1 && !HeroStore.Instance.IsHeroOwned(heroUnlockParam.id))
				{
					list.Add(heroUnlockParam.id);
				}
			}
			return list;
		}

		public bool IsHeroUnlockByRealMoney(int heroID)
		{
			bool result = false;
			if (heroID < listHeroUnlockParam.Count && heroID >= 0)
			{
				result = (listHeroUnlockParam[heroID].isUnlockByRealMoney == 1);
			}
			return result;
		}

		private List<HeroUnlockSpecs> listHeroUnlockParam = new List<HeroUnlockSpecs>();
	}
}
