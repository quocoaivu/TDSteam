using System;
using System.Collections.Generic;
using Data;
using UnityEngine;

namespace HeroCamp
{
	public class SelectHeroSwitchClusterHandler : MonoBehaviour
	{
		private void Start()
		{
			HeroStore.Instance.OnSkillPointChangeEvent += Instance_OnSkillPointChangeEvent;
			HeroStore.Instance.OnHeroLevelChangeEvent += Instance_OnHeroLevelChangeEvent;
			UpdateNotify();
		}

		private void OnDestroy()
		{
			if (HeroStore.Instance != null)
			{
				HeroStore.Instance.OnSkillPointChangeEvent -= Instance_OnSkillPointChangeEvent;
				HeroStore.Instance.OnHeroLevelChangeEvent -= Instance_OnHeroLevelChangeEvent;
			}
		}

		private void Instance_OnSkillPointChangeEvent()
		{
			UpdateNotify();
		}

		private void Instance_OnHeroLevelChangeEvent()
		{
			UpdateNotify();
		}

		public void AutoChoseHero(int heroID)
		{
			foreach (SelectHeroSwitchHandler selectHeroButtonController in listSelectHeroButton)
			{
				if (selectHeroButtonController.HeroID == heroID)
				{
					selectHeroButtonController.OnClick();
				}
			}
		}

		public void Init()
		{
			foreach (SelectHeroSwitchHandler selectHeroButtonController in listSelectHeroButton)
			{
				selectHeroButtonController.UpdateHeroLevel();
			}
		}

		private void UpdateNotify()
		{
			foreach (SelectHeroSwitchHandler selectHeroButtonController in listSelectHeroButton)
			{
				selectHeroButtonController.UpdateNotifyHeroSkill();
			}
		}

		public List<SelectHeroSwitchHandler> listSelectHeroButton = new List<SelectHeroSwitchHandler>();
	}
}
