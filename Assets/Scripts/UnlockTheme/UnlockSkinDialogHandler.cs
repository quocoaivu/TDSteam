using System;
using System.Collections.Generic;
using Data;
using Gameplay;
using UnityEngine;
using UnityEngine.UI;
using WorldMap;

namespace UnlockTheme
{
	public class UnlockSkinDialogHandler : GameplayDialogHandler
	{
        [Space]
        [Header("Image material")]
        [SerializeField]
        private Button unlockButton;

        [SerializeField]
        private Material material;

        [Space]
        [SerializeField]
        private UnlockCriterionClusterHandler unlockConditionGroupController;

        private int[] listConditionsValue;

        private int currentThemeIDToUnlock;

        public void Init(int themeID)
		{
			currentThemeIDToUnlock = themeID;
			OpenWithScaleAnimation();
			unlockConditionGroupController.HideAll();
			InitAllUnlockCondition(themeID);
			InitUnlockButton();
		}

		private void InitUnlockButton()
		{
			if (IsAllConditionPass())
			{
				unlockButton.enabled = true;
				material.SetFloat("_EffectAmount", 0f);
			}
			else
			{
				unlockButton.enabled = false;
				material.SetFloat("_EffectAmount", 1f);
			}
		}

		private void InitAllUnlockCondition(int themeID)
		{
			listConditionsValue = new int[6];
			Dictionary<int, string> listCondition = ThemeStore.Instance.GetListCondition(themeID);
			foreach (KeyValuePair<int, string> keyValuePair in listCondition)
			{
				switch (keyValuePair.Key)
				{
				case 0:
				{
					int num = int.Parse(keyValuePair.Value.Replace("m_", string.Empty));
					if (MapProgressStore.Instance.GetMapIDUnlocked() > num)
					{
						listConditionsValue[0] = 2;
						unlockConditionGroupController.InitConditionContent(0, 0, themeID, true);
					}
					else
					{
						listConditionsValue[0] = 1;
						unlockConditionGroupController.InitConditionContent(0, 0, themeID, false);
					}
					break;
				}
				case 1:
				{
					int num2 = int.Parse(keyValuePair.Value.Replace("s_", string.Empty));
					if (PlayerCurrencyStore.Instance.GetCurrentStar() >= num2)
					{
						listConditionsValue[1] = 2;
						unlockConditionGroupController.InitConditionContent(1, 1, themeID, true);
					}
					else
					{
						listConditionsValue[1] = 1;
						unlockConditionGroupController.InitConditionContent(1, 1, themeID, false);
					}
					break;
				}
				case 2:
				{
					int num3 = int.Parse(keyValuePair.Value.Replace("ha_", string.Empty));
					if (HeroStore.Instance.GetHeroOwnedAmount() >= num3)
					{
						listConditionsValue[2] = 2;
						unlockConditionGroupController.InitConditionContent(2, 2, themeID, true);
					}
					else
					{
						listConditionsValue[2] = 1;
						unlockConditionGroupController.InitConditionContent(2, 2, themeID, false);
					}
					break;
				}
				case 3:
				{
					string[] array = keyValuePair.Value.Split(new char[]
					{
						'_'
					});
					int heroID = int.Parse(array[0]);
					int num4 = int.Parse(array[1]);
					if (HeroStore.Instance.IsHeroOwned(heroID) && HeroStore.Instance.GetCurrentHeroLevel(heroID) >= num4)
					{
						listConditionsValue[3] = 2;
						unlockConditionGroupController.InitConditionContent(3, 3, themeID, true);
					}
					else
					{
						listConditionsValue[3] = 1;
						unlockConditionGroupController.InitConditionContent(3, 3, themeID, false);
					}
					break;
				}
				case 4:
				{
					string[] array2 = keyValuePair.Value.Split(new char[]
					{
						'_'
					});
					int heroID2 = int.Parse(array2[0]);
					int num5 = int.Parse(array2[1]);
					if (HeroStore.Instance.IsHeroOwned(heroID2) && HeroStore.Instance.GetCurrentHeroLevel(heroID2) >= num5)
					{
						listConditionsValue[4] = 2;
						unlockConditionGroupController.InitConditionContent(4, 4, themeID, true);
					}
					else
					{
						listConditionsValue[4] = 1;
						unlockConditionGroupController.InitConditionContent(4, 4, themeID, false);
					}
					break;
				}
				case 6:
				{
					int heroID3 = int.Parse(keyValuePair.Value.Replace("p_", string.Empty));
					if (HeroStore.Instance.IsHeroOwned(heroID3) && HeroStore.Instance.IsPetUnlocked(heroID3))
					{
						listConditionsValue[5] = 2;
						unlockConditionGroupController.InitConditionContent(5, 6, themeID, true);
					}
					else
					{
						listConditionsValue[5] = 1;
						unlockConditionGroupController.InitConditionContent(5, 6, themeID, false);
					}
					break;
				}
				}
			}
		}

		private bool IsAllConditionPass()
		{
			bool result = false;
			int num = 0;
			int num2 = 0;
			foreach (int num3 in listConditionsValue)
			{
				if (num3 > 0)
				{
					num++;
					num2 += num3;
				}
			}
			if (num2 == num * 2)
			{
				result = true;
			}
			return result;
		}

		public void OnClick_UnlockTheme()
		{
			ThemeStore.Instance.SaveThemeUnlockData(currentThemeIDToUnlock);
			int themeIDUnlocked = ThemeStore.Instance.GetThemeIDUnlocked();
			MonoSingleton<WorldMap.UIRootHandler>.Instance.MapThemesController.SpawnTheme(themeIDUnlocked);
			ThemeStore.Instance.SaveLastThemePlayed(themeIDUnlocked);
			MonoSingleton<WorldMap.UIRootHandler>.Instance.MapThemesController.RefreshUnlockThemesStatus();
			MonoSingleton<WorldMap.UIRootHandler>.Instance.MapThemesController.RefreshSelectThemesButtonStatus();
			CloseWithScaleAnimation();
		}

		public override void OpenWithScaleAnimation()
		{
			base.OpenWithScaleAnimation();
		}

		public override void CloseWithScaleAnimation()
		{
			base.CloseWithScaleAnimation();
		}
	}
}
