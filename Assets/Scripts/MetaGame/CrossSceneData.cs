using System;
using System.Collections.Generic;

namespace MetaGame
{
	public class CrossSceneData
	{
		public static CrossSceneData Instance
		{
			get
			{
				if (CrossSceneData.instance == null)
				{
					CrossSceneData.instance = new CrossSceneData();
				}
				return CrossSceneData.instance;
			}
		}

		public int OpenSceneCount
		{
			get
			{
				return openSceneCount;
			}
			private set
			{
				openSceneCount = value;
			}
		}

		public void IncreaseOpenSceneCount()
		{
			OpenSceneCount++;
		}

		public int MapIDSelected
		{
			get
			{
				return mapIDSelected;
			}
			set
			{
				mapIDSelected = value;
			}
		}

		public BattleDifficulty BattleDifficulty { get; set; }

		public List<int> ListHeroesIdsSelected
		{
			get
			{
				return listHeroesIdsSelected;
			}
			private set
			{
				listHeroesIdsSelected = value;
			}
		}

		public void AddHeroIDToList(int heroID)
		{
			ListHeroesIdsSelected.Add(heroID);
		}

		public void ClearListHeroID()
		{
			ListHeroesIdsSelected.Clear();
		}

		private static CrossSceneData instance;

		[UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			instance = null;
		}
		private int openSceneCount;

		private int mapIDSelected;

		private List<int> listHeroesIdsSelected = new List<int>();
	}
}
