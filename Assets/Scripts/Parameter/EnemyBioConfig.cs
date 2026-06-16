using System;
using System.Collections.Generic;

namespace Parameter
{
	public class EnemyBioConfig : Singleton<EnemyBioConfig>
	{
		public void ClearData()
		{
			listEnemy.Clear();
		}

		public void SetEnemyDescription(EnemyLocalizationData enemy)
		{
			int count = listEnemy.Count;
			if (count <= enemy.id)
			{
				List<EnemyLocalizationData> list = new List<EnemyLocalizationData>();
				list.Insert(enemy.level, enemy);
				listEnemy.Insert(enemy.id, list);
			}
			else
			{
				List<EnemyLocalizationData> list2 = listEnemy[enemy.id];
				list2.Insert(enemy.level, enemy);
			}
		}

		public EnemyLocalizationData GetEnemyParameter(int id, int level)
		{
			if (CheckParameter(id))
			{
				return listEnemy[id][level];
			}
			return default(EnemyLocalizationData);
		}

		public int GetNumberOfEnemy()
		{
			return listEnemy.Count;
		}

		public string GetEnemyName(int enemyID)
		{
			if (CheckParameter(enemyID))
			{
				return listEnemy[enemyID][0].name;
			}
			return "_";
		}

		public string GetEnemyDescription(int enemyID)
		{
			if (CheckParameter(enemyID))
			{
				return listEnemy[enemyID][0].description;
			}
			return "_";
		}

		public string GetEnemySpecialAbility(int enemyID)
		{
			if (CheckParameter(enemyID))
			{
				return listEnemy[enemyID][0].specialAbility;
			}
			return "_";
		}

		private bool CheckParameter(int id)
		{
			return id < GetNumberOfEnemy();
		}

		public List<List<EnemyLocalizationData>> listEnemy = new List<List<EnemyLocalizationData>>();
	}
}
