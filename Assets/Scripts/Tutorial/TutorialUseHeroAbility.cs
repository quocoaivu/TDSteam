using System;
using Data;
using Gameplay;
using MetaGame;
using UnityEngine;

namespace Tutorial
{
	public class TutorialUseHeroAbility : TutorialBase
	{
		public void ShowTutorialUseHeroSkill()
		{
			TutorialUseHeroSkillView target = GetTutInMap();
			if (target != null)
			{
				target.Show();
			}
		}

		public void HideTutorialUseHeroSkill()
		{
			TutorialUseHeroSkillView target = GetTutInMap();
			if (target != null)
			{
				target.Hide();
			}
		}

		// TutorialUseHeroSkillView náº±m trong map prefab load runtime (Addressable) nÃªn khÃ´ng
		// gÃ¡n Ä‘Æ°á»£c qua Inspector â€” pháº£i tÃ¬m theo tag. TÃ¬m Ä‘Ãºng 1 láº§n rá»“i cache, trÃ¡nh
		// FindGameObjectWithTag láº·p láº¡i má»—i láº§n Show/Hide. Object bá»‹ Destroy thÃ¬ cache == null,
		// láº§n gá»i sau tá»± tÃ¬m láº¡i.
		private TutorialUseHeroSkillView GetTutInMap()
		{
			if (tutInMap == null)
			{
				GameObject gameObject = GameObject.FindGameObjectWithTag(TutorialUseHeroAbility.TUTORIAL_USE_HERO_SKILL);
				if (gameObject)
				{
					tutInMap = gameObject.GetComponent<TutorialUseHeroSkillView>();
				}
			}
			return tutInMap;
		}

		protected override void SaveTutorialPassed()
		{
			Common.SSRTrace.Log("HoÃ n thÃ nh tut sá»­ dá»¥ng skill hero!");
			TutorialStore.Instance.SetTutorialStatus(tutorialID, true);
		}

		protected override bool ShouldShowTutorial()
		{
			if (FormatDirector.Instance.gameMode != GameFormat.CampaignMode)
			{
				return false;
			}
			return !TutorialStore.Instance.GetTutorialStatus(tutorialID)
				&& !MonoSingleton<GameRecord>.Instance.PlayerKnowHowToUseSkill
				&& MonoSingleton<GameRecord>.Instance.MapID == 0;
		}

		private static string TUTORIAL_USE_HERO_SKILL = "TutorialUseHeroSkill";

		private string tutorialID = TutorialStore.TUTORIAL_ID_HERO_SKILL;

		private TutorialUseHeroSkillView tutInMap;
	}
}
