using System;
using System.Collections.Generic;
using Data;
using MetaGame;
using Parameter;
using Tutorial;
using UnityEngine;

namespace Gameplay
{
	public class GameplayUIHeroDirector : MonoSingleton<GameplayUIHeroDirector>
	{

        [Space]
        [Header("Hero icon")]
        [SerializeField]
        private HeroPicker heroIconOrigin;

        [SerializeField]
        private Transform heroIconParent;

        private Dictionary<int, HeroPicker> listSelectHeroButton = new Dictionary<int, HeroPicker>();

        [Space]
        [Header("Hero icon skill")]
        [SerializeField]
        private SelectHeroAbility heroSkillIconOrigin;

        [SerializeField]
        private Transform heroIconSkillParent;

        public Dictionary<int, SelectHeroAbility> listSelectHeroSkillButton = new Dictionary<int, SelectHeroAbility>();

        [Space]
        [Header("Hero Current Level Information")]
        [SerializeField]
        private HeroCurrentLevelOverviewDialog heroCurrentLevelInformationPopup;

        [Space]
        [Header("Controllers")]
        [SerializeField]
        private GameObject heroMoveControllerGroup;

        [SerializeField]
        private GameObject heroSkillControllerGroup;

        private List<int> listHeroesID = new List<int>();

        public HeroCurrentLevelOverviewDialog HeroCurrentLevelInformationPopup
		{
			get
			{
				return heroCurrentLevelInformationPopup;
			}
			set
			{
				heroCurrentLevelInformationPopup = value;
			}
		}

		private void Start()
		{
			GameFormat gameMode = FormatDirector.Instance.gameMode;
			if (gameMode != GameFormat.CampaignMode)
			{
				if (gameMode != GameFormat.DailyTrialMode)
				{
					if (gameMode == GameFormat.TournamentMode)
					{
						listHeroesID = MonoSingleton<GameRecord>.Instance.ListHeroesIdsSelected;
					}
				}
				else
				{
					int currentDayIndex = DailyTrialStore.Instance.GetCurrentDayIndex();
					listHeroesID = DailyOrdealSpec.Instance.getListInputHeroesID(currentDayIndex);
				}
			}
			else
			{
				listHeroesID = MonoSingleton<GameRecord>.Instance.ListHeroesIdsSelected;
			}
			HerosDirector.Instance.onChooseHero += Instance_onChooseHero;
			HerosDirector.Instance.onUnChooseHero += Instance_onUnChooseHero;
			HerosDirector.Instance.onChooseHeroSkill += Instance_onChooseHeroSkill;
			HerosDirector.Instance.onUnChooseHeroSkill += Instance_onUnChooseHeroSkill;
			if (GameplayTutorialDirector.Instance.IsTutorialDone() || !GameplayTutorialDirector.Instance.IsTutorialMap())
			{
				EnableHeroUI();
			}
			else
			{
				DisableHeroUI();
			}
		}

		private void OnDestroy()
		{
			if (HerosDirector.Instance != null)
			{
				HerosDirector.Instance.onChooseHero -= Instance_onChooseHero;
				HerosDirector.Instance.onUnChooseHero -= Instance_onUnChooseHero;
			}
		}

		private void Instance_onChooseHero(int inputHeroID)
		{
			DisableChooseOtherButtonHero(inputHeroID);
		}

		private void Instance_onUnChooseHero(int inputHeroID)
		{
			foreach (KeyValuePair<int, HeroPicker> keyValuePair in listSelectHeroButton)
			{
				if (keyValuePair.Value.HeroID == inputHeroID)
				{
					keyValuePair.Value.HideSelected();
				}
			}
		}

		private void Instance_onChooseHeroSkill(int inputHeroID)
		{
			DisableChooseOtherButtonHeroSkill(inputHeroID);
		}

		private void Instance_onUnChooseHeroSkill(int inputHeroID)
		{
			foreach (KeyValuePair<int, SelectHeroAbility> keyValuePair in listSelectHeroSkillButton)
			{
				if (keyValuePair.Value.HeroID == inputHeroID)
				{
					keyValuePair.Value.HideSelected();
				}
			}
		}

		public void InitListHeroesSelected(int heroID)
		{
			if (!listSelectHeroButton.ContainsKey(heroID))
			{
				HeroPicker selectHero = UnityEngine.Object.Instantiate<HeroPicker>(heroIconOrigin);
				selectHero.transform.SetParent(heroIconParent);
				selectHero.transform.localScale = heroIconOrigin.transform.localScale;
				selectHero.Init(heroID);
				listSelectHeroButton.Add(heroID, selectHero);
			}
			else
			{
				listSelectHeroButton[heroID].ViewEnable();
			}
			if (!listSelectHeroSkillButton.ContainsKey(heroID))
			{
				SelectHeroAbility selectHeroSkill = UnityEngine.Object.Instantiate<SelectHeroAbility>(heroSkillIconOrigin);
				selectHeroSkill.transform.SetParent(heroIconSkillParent);
				selectHeroSkill.transform.localScale = heroSkillIconOrigin.transform.localScale;
				selectHeroSkill.Init(heroID);
				listSelectHeroSkillButton.Add(heroID, selectHeroSkill);
			}
			else
			{
				listSelectHeroSkillButton[heroID].ViewEnable();
			}
		}

		public void EnableHeroUI()
		{
			heroMoveControllerGroup.SetActive(true);
			heroSkillControllerGroup.SetActive(true);
		}

		private void DisableHeroUI()
		{
			heroMoveControllerGroup.SetActive(false);
			heroSkillControllerGroup.SetActive(false);
		}

		private void DisableChooseOtherButtonHero(int selectedHeroID)
		{
			foreach (KeyValuePair<int, HeroPicker> keyValuePair in listSelectHeroButton)
			{
				if (keyValuePair.Value.HeroID != selectedHeroID)
				{
					keyValuePair.Value.Refresh();
				}
			}
		}

		private void DisableChooseOtherButtonHeroSkill(int selectedHeroID)
		{
			foreach (KeyValuePair<int, SelectHeroAbility> keyValuePair in listSelectHeroSkillButton)
			{
				if (keyValuePair.Value.HeroID != selectedHeroID)
				{
					keyValuePair.Value.Refresh();
				}
			}
		}

		public void UpdateHeroHealthBarStatus(int heroID, int currentHealth, int OriginHealth)
		{
			if (!listSelectHeroButton.ContainsKey(heroID))
			{
				return;
			}
			listSelectHeroButton[heroID].UpdateHealthBar(currentHealth, OriginHealth);
		}

		public void DisableHeroesUI(int heroID)
		{
			if (!listSelectHeroButton.ContainsKey(heroID))
			{
				return;
			}
			listSelectHeroButton[heroID].ViewDisable();
			listSelectHeroButton[heroID].DoCooldown();
			listSelectHeroSkillButton[heroID].ViewDisableImmediately();
		}

		public void RestoreAllCooldownSkill()
		{
			foreach (int heroID in listHeroesID)
			{
				RestoreCooldownSkill(heroID);
			}
		}

		private void RestoreCooldownSkill(int heroID)
		{
			listSelectHeroSkillButton[heroID].ViewEnableImmediately();
		}
	}
}
