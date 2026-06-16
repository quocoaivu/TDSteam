using System;
using Data;
using UnityEngine;

namespace HotFix
{
	public class LostHerosRecordFixer : MonoBehaviour
	{
		private void Start()
		{
			Fix();
		}

		private void Fix()
		{
			if (!HeroStore.Instance.IsHeroOwned(1) && MapProgressStore.Instance.GetMapIDUnlocked() >= 2)
			{
				HeroStore.Instance.UnlockHero(1);
			}
		}
	}
}
