using System;
using Data;
using Parameter;
using UnityEngine;

public static class HeroGemCalculator
{
	public static int GetGemAmountToLevelUp(int heroID)
	{
		return Mathf.RoundToInt((float)HeroStore.Instance.GetExpToLevelUp(heroID) * HeroGemCalculator.expToGemRatio);
	}

	public static bool IsEnoughGemToUpgrade(int heroID)
	{
		return !HeroStore.Instance.IsReachMaxLevel(heroID) && PlayerCurrencyStore.Instance.GetCurrentGem() >= HeroGemCalculator.GetGemAmountToLevelUp(heroID);
	}

	public static int GetGemAmountToUnlockPet(int heroID)
	{
		int petID = HeroParameterManager.Instance.GetPetID(heroID);
		return ConfigRegistry.Instance.petConfig.dataArray[petID % 1000].Price;
	}

	public static bool IsEnoughGemToUnlockPet(int heroID)
	{
		return PlayerCurrencyStore.Instance.GetCurrentGem() >= HeroGemCalculator.GetGemAmountToUnlockPet(heroID);
	}

	private static float expToGemRatio = 0.5f;
}
