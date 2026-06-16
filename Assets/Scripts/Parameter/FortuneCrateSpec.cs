using System;
using System.Collections.Generic;

namespace Parameter
{
	public class FortuneCrateSpec
	{
		public static FortuneCrateSpec Instance
		{
			get
			{
				if (FortuneCrateSpec.instance == null)
				{
					FortuneCrateSpec.instance = new FortuneCrateSpec();
				}
				return FortuneCrateSpec.instance;
			}
		}

		public void SetParameter(FortuneCrate chest)
		{
			int count = listParam.Count;
			if (count <= chest.id)
			{
				List<FortuneCrate> list = new List<FortuneCrate>();
				list.Insert(chest.turn, chest);
				listParam.Insert(chest.id, list);
			}
			else
			{
				List<FortuneCrate> list2 = listParam[chest.id];
				list2.Insert(chest.turn, chest);
			}
		}

		public FortuneCrate GetChestParameter(int id, int level)
		{
			if (CheckParameter(id, level))
			{
				return listParam[id][level];
			}
			return default(FortuneCrate);
		}

		private bool CheckParameter(int id, int level)
		{
			return id < GetNumberOfItem() && level <= GetNumberOfLevel();
		}

		public int GetNumberOfItem()
		{
			return listParam.Count;
		}

		public int GetNumberOfLevel()
		{
			if (GetNumberOfItem() > 0)
			{
				return listParam[0].Count;
			}
			return 0;
		}

		public List<int> GetListItemsPreview()
		{
			List<int> list = new List<int>();
			for (int i = 0; i < listParam.Count; i++)
			{
				if (GetChestParameter(i, 0).isPreview == 1 && !list.Contains(i))
				{
					list.Add(i);
				}
			}
			return list;
		}

		public int GetChestRate(int id, int turn)
		{
			if (id >= 0 && id < listParam.Count)
			{
				return listParam[id][turn].rate;
			}
			return -1;
		}

		public int GetChestValue(int id, int turn)
		{
			if (id >= 0 && id < listParam.Count)
			{
				return listParam[id][turn].value;
			}
			return -1;
		}

		public List<List<FortuneCrate>> listParam = new List<List<FortuneCrate>>();

		private static FortuneCrateSpec instance;
		[UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			instance = null;
		}
	}
}
