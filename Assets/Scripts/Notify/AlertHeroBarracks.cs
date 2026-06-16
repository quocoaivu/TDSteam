using System;
using System.Collections.Generic;
using Data;
using Parameter;

namespace Notify
{
	public class AlertHeroBarracks : AlertTrooper
	{
		protected override bool ShouldShowNotify()
		{
			return isAvailable();
		}

		private bool isAvailable()
		{
			bool result = false;
			if (isEnoughGemToUpgrade())
			{
				result = true;
			}
			if (isAvailableToBuy())
			{
				result = true;
			}
			if (isAvailableToUnlock())
			{
				result = true;
			}
			if (isAvailableToUpgradeHeroSkill())
			{
				result = true;
			}
			return result;
		}

		private bool isEnoughGemToUpgrade()
		{
			bool result = false;
			List<int> listHeroIDOwned = HeroStore.Instance.GetListHeroIDOwned();
			foreach (int heroID in listHeroIDOwned)
			{
				if (HeroGemCalculator.IsEnoughGemToUpgrade(heroID))
				{
					result = true;
				}
			}
			return result;
		}

		private bool isAvailableToUnlock()
		{
			bool result = false;
			List<int> listHeroNotOwnedByPlay = Singleton<UnlockHeroSpec>.Instance.GetListHeroNotOwnedByPlay();
			foreach (int heroID in listHeroNotOwnedByPlay)
			{
				if (Singleton<UnlockHeroSpec>.Instance.IsHeroAvailableToUnlock(heroID))
				{
					result = true;
				}
			}
			return result;
		}

		private bool isAvailableToBuy()
		{
			bool result = false;
			List<int> listHeroNotOwnedByGem = Singleton<UnlockHeroSpec>.Instance.GetListHeroNotOwnedByGem();
			foreach (int heroID in listHeroNotOwnedByGem)
			{
				if (Singleton<UnlockHeroSpec>.Instance.IsHeroAvailableToBuy(heroID))
				{
					result = true;
				}
			}
			return result;
		}

		private bool isAvailableToUpgradeHeroSkill()
		{
			bool result = false;
			List<int> listHeroIDOwned = HeroStore.Instance.GetListHeroIDOwned();
			foreach (int heroID in listHeroIDOwned)
			{
				if (HeroStore.Instance.GetCurrentSkillPoint(heroID) >= 1)
				{
					result = true;
				}
			}
			return result;
		}
	}
}
