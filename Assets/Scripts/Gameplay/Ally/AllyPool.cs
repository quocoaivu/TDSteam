using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using DG.Tweening;
using MetaGame;
using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class AllyPool : MonoSingleton<AllyPool>
	{

        private Dictionary<int, HeroEntity> listActiveHero = new Dictionary<int, HeroEntity>();

        public Dictionary<int, HeroEntity> ListActiveHero
		{
			get
			{
				return listActiveHero;
			}
			set
			{
				listActiveHero = value;
			}
		}

		// Hero IDs the player selected, depending on the current game mode.
		private List<int> GetSelectedHeroIds()
		{
			switch (FormatDirector.Instance.gameMode)
			{
				case GameFormat.DailyTrialMode:
					int currentDayIndex = DailyTrialStore.Instance.GetCurrentDayIndex();
					return DailyOrdealSpec.Instance.getListInputHeroesID(currentDayIndex);
				case GameFormat.CampaignMode:
				case GameFormat.TournamentMode:
					return GameRecord.Instance.ListHeroesIdsSelected;
				default:
					return new List<int>();
			}
		}

		public void InitHeroesStartPosition()
		{
			List<int> list = GetSelectedHeroIds();
			for (int i = 0; i < list.Count; i++)
			{
				HeroEntity heroModel = GetHero(list[i]);
				heroModel.transform.position = MonoSingleton<HeroBeginPositionDirector>.Instance.listStartPosition[i].position;
				heroModel.SetAssignedPosition(heroModel.transform.position);
				if (!ListActiveHero.ContainsKey(list[i]))
				{
					ListActiveHero.Add(list[i], heroModel);
				}
			}
		}

		public float GetHeroSkillCooldownTime(int heroID)
		{
			if (!listActiveHero.TryGetValue(heroID, out HeroEntity heroModel))
			{
				UnityEngine.Debug.LogError("GetHeroSkillCooldownTime: no active hero " + heroID);
				return 0f;
			}
			return heroModel.HeroSkillController.GetActiveSkillCooldownTime();
		}

		public string GetHeroSkillUseType(int heroID)
		{
			if (!listActiveHero.TryGetValue(heroID, out HeroEntity heroModel))
			{
				UnityEngine.Debug.LogError("GetHeroSkillUseType: no active hero " + heroID);
				return string.Empty;
			}
			return heroModel.HeroSkillController.GetActiveSkillUseType();
		}

		public void PushAlliesToPool(int towerID, int towerLevel, int allocateNumber)
		{
			string arg = string.Format("ally_{0}_{1}", towerID, towerLevel);
			MinionEntity prefab = Common.AssetLoader.Load<MinionEntity>(string.Format("Allies/{0}", arg));
			if (prefab == null)
			{
				UnityEngine.Debug.LogError("PushAlliesToPool: ally prefab not found: " + arg);
				return;
			}
			MinionEntity allyModel = Instantiate(prefab);
			allyModel.gameObject.SetActive(false);
			Common.GameObjectPool.ManagePool(allyModel.gameObject, allocateNumber);
			Common.GameObjectPool.Despawn(allyModel.gameObject);
		}

		public MinionEntity GetAlly(int id, int level)
		{
			string gameObjectName = string.Format("ally_{0}_{1}(Clone)", id, level);
			GameObject gameObject = Common.GameObjectPool.Spawn(gameObjectName, default(Vector3), default(Quaternion));
			MinionEntity component = gameObject.GetComponent<MinionEntity>();
			component.transform.parent = transform;
			component.gameObject.SetActive(false);
			return component;
		}

		public void Despawn(MinionEntity ally, float delayTime)
		{
			StartCoroutine(IDespawnAlly(ally, delayTime));
		}

		private IEnumerator IDespawnAlly(MinionEntity ally, float delayTime)
		{
			ally.transform.DOKill(false);
			yield return new WaitForSeconds(delayTime);
			ally.transform.position = Common.GameObjectPool.PoolPosition;
			ally.gameObject.SetActive(false);
			Common.GameObjectPool.Despawn(ally.gameObject);
		}

		public void InitPoolHeroes()
		{
			List<int> list = GetSelectedHeroIds();
			for (int i = 0; i < list.Count; i++)
			{
				HeroEntity prefab = Common.AssetLoader.Load<HeroEntity>(string.Format("Heroes/hero_{0}", list[i]));
				if (prefab == null)
				{
					UnityEngine.Debug.LogError("InitPoolHeroes: hero prefab not found: hero_" + list[i]);
					continue;
				}
				HeroEntity heroModel = Instantiate(prefab);
				heroModel.gameObject.SetActive(false);
				heroModel.transform.position = Vector3.zero;
				Common.GameObjectPool.ManagePool(heroModel.gameObject, 0);
				Common.GameObjectPool.Despawn(heroModel.gameObject);
			}
		}

		private HeroEntity GetHero(int id)
		{
			if (id < 0)
			{
				UnityEngine.Debug.LogError("Input Id <0");
				return null;
			}
			string gameObjectName = string.Format("hero_{0}(Clone)", id);
			GameObject gameObject = Common.GameObjectPool.Spawn(gameObjectName, default(Vector3), default(Quaternion));
			HeroEntity component = gameObject.GetComponent<HeroEntity>();
			component.transform.parent = transform;
			return component;
		}

		public void Despawn(HeroEntity hero)
		{
			hero.transform.DOKill(false);
			hero.transform.position = Common.GameObjectPool.PoolPosition;
			hero.gameObject.SetActive(false);
			Common.GameObjectPool.Despawn(hero.gameObject);
		}

		public void RestoreHealthForAllAllies()
		{
			foreach (CharacterEntity characterModel in GameRecord.Instance.ListActiveAlly)
			{
				characterModel.RestoreHealth();
			}
		}
	}
}
