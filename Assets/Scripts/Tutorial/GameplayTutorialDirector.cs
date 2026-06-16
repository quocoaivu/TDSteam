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
	}
}
