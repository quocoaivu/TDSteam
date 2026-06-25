using System;
using Data;
using Gameplay;
using MetaGame;
using GameCore;
using UnityEngine;

namespace Tutorial
{
	public class GameplayTutorialDirector : BaseMonoBehaviour
	{
        private string tutorialID = TutorialStore.TUTORIAL_ID_BUILD_TOWER;

        [Space]
        [SerializeField]
        private TutorialUseHeroAbility tutorialUseHeroSkill;

        [Space]
        [SerializeField]
        private TutorialTravelHero tutorialMoveHero;

        [Space]
        [SerializeField]
        private float delayTimeToStartTutorialControlHero;

        public TutorialUseHeroAbility TutorialUseHeroSkill
		{
			get
			{
				return tutorialUseHeroSkill;
			}
			set
			{
				tutorialUseHeroSkill = value;
			}
		}

		public TutorialTravelHero TutorialMoveHero
		{
			get
			{
				return tutorialMoveHero;
			}
			set
			{
				tutorialMoveHero = value;
			}
		}

		public static GameplayTutorialDirector Instance { get; set; }

		private void Awake()
		{
			if (GameplayTutorialDirector.Instance)
			{
				UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
			GameplayTutorialDirector.Instance = this;
		}

		private void Start()
		{
			MonoSingleton<EnemyPool>.Instance.onDispatchEventTutorialHeroSkill += Instance_onDispatchEventTutorialHeroSkill;
		}

		private void OnDestroy()
		{
			EnemyPool spawnEnemy = MonoSingleton<EnemyPool>.InstanceIfExists;
			if (spawnEnemy)
			{
				spawnEnemy.onDispatchEventTutorialHeroSkill -= Instance_onDispatchEventTutorialHeroSkill;
			}
			if (GameplayTutorialDirector.Instance == this)
			{
				GameplayTutorialDirector.Instance = null;
			}
		}

		private void Instance_onDispatchEventTutorialHeroSkill()
		{
			CheckConditionAfterTime();
		}

		private void CheckConditionAfterTime()
		{
			base.CustomInvoke(new Action(DoCheck), delayTimeToStartTutorialControlHero / GameplayDirector.Instance.gameSpeedController.GameSpeed);
		}

		private void DoCheck()
		{
			TutorialUseHeroSkill.CheckCondition();
		}

		public bool IsTutorialDone()
		{
			return TutorialStore.Instance.GetTutorialStatus(tutorialID);
		}

		public bool IsTutorialMap()
		{
			return FormatDirector.Instance.gameMode == GameFormat.CampaignMode && MonoSingleton<GameRecord>.Instance.MapID == 0;
		}

		// True only during the VERY FIRST play of the campaign tutorial map (Map 0). playCount is bumped at
		// end-game (GameRuleHandler victory/defeat), so it stays 0 for the whole first match and becomes >= 1
		// afterwards. Every in-match tutorial step gates on this so none of them replay when Map 0 is played
		// again, even if a step's own "passed" flag was never saved (e.g. the first run was abandoned). Static
		// so steps can call it from Start() without depending on this director's Instance being ready, and it
		// only uses already-initialised singletons. Note: IsTutorialMap() above is deliberately left WITHOUT
		// this playCount guard — it gates end-game UI (e.g. the open-chest tutorial) that must still run on the
		// first play, where playCount has already become 1 by the time the end-game popup opens.
		public static bool IsFirstPlayTutorialMap()
		{
			return FormatDirector.Instance.gameMode == GameFormat.CampaignMode
				&& MonoSingleton<GameRecord>.Instance.MapID == 0
				&& Data.MapProgressStore.Instance.GetCurrentPlayCount(0) == 0;
		}
	}
}
