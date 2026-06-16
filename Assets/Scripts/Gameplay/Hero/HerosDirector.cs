using System;
using System.Collections.Generic;
using System.Diagnostics;
using GameCore;
using UnityEngine;

namespace Gameplay
{
	public class HerosDirector : BaseMonoBehaviour
	{
		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<int> onChooseHero;

		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<int> onUnChooseHero;

		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<int, Vector2> onHeroMoveToAssignedPosition;

		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<int> onChooseHeroSkill;

		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<int> onUnChooseHeroSkill;

		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<int, Vector2> onCastHeroSkillToAssignedPosition;

        private Dictionary<int, HeroEntity> listHeroes = new Dictionary<int, HeroEntity>();

        private int heroIDChoosing;

        private int lastHeroIDChoosed;

        private int heroSkillIDChoosing;


        public int HeroIDChoosing
		{
			get
			{
				return heroIDChoosing;
			}
			private set
			{
				heroIDChoosing = value;
			}
		}

		public int HeroSkillIDChoosing
		{
			get
			{
				return heroSkillIDChoosing;
			}
			private set
			{
				heroSkillIDChoosing = value;
			}
		}

		public static HerosDirector Instance { get; set; }

		private void Awake()
		{
			if (HerosDirector.Instance)
			{
				UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
			HerosDirector.Instance = this;
			InitDefaultData();
		}

		private void InitDefaultData()
		{
			HeroIDChoosing = -1;
			HeroSkillIDChoosing = -1;
		}

		public void AddToList(int heroID, HeroEntity heroModel)
		{
			if (!listHeroes.ContainsKey(heroID))
			{
				listHeroes.Add(heroID, heroModel);
			}
		}

		public HeroEntity GetHero(int heroId)
		{
			if (!listHeroes.ContainsKey(heroId))
			{
				return null;
			}
			return listHeroes[heroId];
		}

		public void CalculateExp()
		{
			int totalExp = MonoSingleton<GameRecord>.Instance.TotalExp;
			foreach (HeroEntity heroModel in listHeroes.Values)
			{
				float num = (float)UnityEngine.Random.Range(85, 115) / 100f;
				int amountEXP = (int)((float)(totalExp / listHeroes.Count) * num);
				heroModel.AddExp(amountEXP);
			}
		}

		public void ChooseHero(int heroID)
		{
			HeroIDChoosing = heroID;
			lastHeroIDChoosed = HeroIDChoosing;
			if (onChooseHero != null)
			{
				onChooseHero(heroID);
			}
		}

		public void UnChooseHero(int heroID)
		{
			if (heroID == -1)
			{
				return;
			}
			if (onUnChooseHero != null)
			{
				onUnChooseHero(heroID);
			}
			HeroIDChoosing = -1;
		}

		public void MoveHeroToAssignedPosition(int heroID, Vector3 targetPosition)
		{
			listHeroes[heroID].GetFsmController().GetCurrentState().OnInput(PhaseInputKind.UserAssignPosition, new object[]
			{
				targetPosition
			});
			DispatchOnMoveHeroToAssignedPosition(heroID, targetPosition);
		}

		private void DispatchOnMoveHeroToAssignedPosition(int heroID, Vector3 targetPosition)
		{
			if (onHeroMoveToAssignedPosition != null)
			{
				onHeroMoveToAssignedPosition(heroID, targetPosition);
			}
		}

		public void ChooseHeroSkill(int heroID)
		{
			HeroSkillIDChoosing = heroID;
			if (onChooseHeroSkill != null)
			{
				onChooseHeroSkill(heroID);
			}
		}

		public void UnChooseHeroSkill(int heroID)
		{
			if (onUnChooseHeroSkill != null)
			{
				onUnChooseHeroSkill(heroID);
			}
			HeroSkillIDChoosing = -1;
		}

		public void CastHeroSkillToAssignedPosition(int heroID, Vector3 targetPosition)
		{
			if (onCastHeroSkillToAssignedPosition != null)
			{
				onCastHeroSkillToAssignedPosition(heroID, targetPosition);
			}
		}
	}
}
