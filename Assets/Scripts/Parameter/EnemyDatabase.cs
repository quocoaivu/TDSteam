using System;
using System.Collections.Generic;
using Gameplay;

namespace Parameter
{
	public class EnemyDatabase
	{
		public static EnemyDatabase Instance
		{
			get
			{
				if (EnemyDatabase.instance == null)
				{
					EnemyDatabase.instance = new EnemyDatabase();
				}
				return EnemyDatabase.instance;
			}
		}

		public void SetEnemyParameter(EnemyParameter enemy)
		{
			int count = listEnemy.Count;
			if (count <= enemy.id)
			{
				List<EnemyParameter> list = new List<EnemyParameter>();
				list.Insert(enemy.level, enemy);
				listEnemy.Insert(enemy.id, list);
			}
			else
			{
				List<EnemyParameter> list2 = listEnemy[enemy.id];
				list2.Insert(enemy.level, enemy);
			}
		}

		public List<int> GetAllEnemyIds()
		{
			List<int> list = new List<int>();
			for (int i = 0; i < listEnemy.Count; i++)
			{
				list.Add(listEnemy[i][0].id);
			}
			return list;
		}

		public EnemyParameter GetEnemyParameter(int id, int level)
		{
			if (CheckParameter(id))
			{
				return listEnemy[id][level];
			}
			return default(EnemyParameter);
		}

		public EnemyParameter GetEnemyParameterForEndlessMode(int id, int level, int loopAmount)
		{
			EnemyParameter enemyParameter = GetEnemyParameter(id, level);
			float healthIncreasePercentage = GameplayDirector.Instance.endlessModeManager.healthIncreasePercentage;
			float damageIncreasePercentage = GameplayDirector.Instance.endlessModeManager.damageIncreasePercentage;
			enemyParameter.health += (int)((float)loopAmount * (healthIncreasePercentage / 100f) * (float)enemyParameter.health);
			enemyParameter.attack_physics_min += (int)((float)loopAmount * (healthIncreasePercentage / 100f) * (float)enemyParameter.attack_physics_min);
			enemyParameter.attack_physics_max += (int)((float)loopAmount * (healthIncreasePercentage / 100f) * (float)enemyParameter.attack_physics_max);
			enemyParameter.attack_magic_min += (int)((float)loopAmount * (healthIncreasePercentage / 100f) * (float)enemyParameter.attack_magic_min);
			enemyParameter.attack_magic_max += (int)((float)loopAmount * (healthIncreasePercentage / 100f) * (float)enemyParameter.attack_magic_max);
			return enemyParameter;
		}

		public int GetHealth(int enemyID, int level)
		{
			if (CheckParameter(enemyID))
			{
				return listEnemy[enemyID][level].health;
			}
			return -1;
		}

		public int GetMinDamage(int enemyID, int level)
		{
			int result;
			if (CheckParameter(enemyID))
			{
				if (listEnemy[enemyID][level].attack_physics_min == 0)
				{
					result = listEnemy[enemyID][level].attack_magic_min;
				}
				else
				{
					result = listEnemy[enemyID][level].attack_physics_min;
				}
			}
			else
			{
				result = -1;
			}
			return result;
		}

		public int GetMaxDamage(int enemyID, int level)
		{
			int result;
			if (CheckParameter(enemyID))
			{
				if (listEnemy[enemyID][level].attack_physics_max == 0)
				{
					result = listEnemy[enemyID][level].attack_magic_max;
				}
				else
				{
					result = listEnemy[enemyID][level].attack_physics_max;
				}
			}
			else
			{
				result = -1;
			}
			return result;
		}

		public bool isPhysicsAttack(int enemyID)
		{
			return listEnemy[enemyID][0].attack_physics_max > 0;
		}

		public int GetPhysicsArmor(int enemyID, int level)
		{
			if (CheckParameter(enemyID))
			{
				return listEnemy[enemyID][level].armor_physics;
			}
			return -1;
		}

		public int GetMagicArmor(int enemyID, int level)
		{
			if (CheckParameter(enemyID))
			{
				return listEnemy[enemyID][level].armor_magic;
			}
			return -1;
		}

		public int GetSpeed(int enemyID, int level)
		{
			if (CheckParameter(enemyID))
			{
				return listEnemy[enemyID][level].speed;
			}
			return -1;
		}

		public int GetLifeTaken(int enemyID, int level)
		{
			if (CheckParameter(enemyID))
			{
				return listEnemy[enemyID][level].lifeTaken;
			}
			return -1;
		}

		public int GetValue(int enemyID)
		{
			if (CheckParameter(enemyID))
			{
				return listEnemy[enemyID][0].value;
			}
			return -1;
		}

		public bool IsBoss(int enemyID)
		{
			return CheckParameter(enemyID) && listEnemy[enemyID][0].isBoss;
		}

		public bool IsWaveHaveBoss(List<int> listEnemyID)
		{
			bool result = false;
			foreach (int enemyID in listEnemyID)
			{
				if (IsBoss(enemyID))
				{
					result = true;
				}
			}
			return result;
		}

		public int GetNumberOfEnemy()
		{
			return listEnemy.Count;
		}

		public bool IsEnemyHasMoreThanOneLife(int enemyID)
		{
			bool result = false;
			if (enemyID == 10 || enemyID == 12 || enemyID == 24)
			{
				result = true;
			}
			return result;
		}

		private bool CheckParameter(int id)
		{
			return id < GetNumberOfEnemy();
		}

		public bool IsFlyEnemyInGate(int wave, int gate)
		{
			bool result = false;
			List<WaveEnemyEntry> listEnemyWithWave = MonoSingleton<GameRecord>.Instance.GetListEnemyWithWave(wave);
			foreach (WaveEnemyEntry enemyData in listEnemyWithWave)
			{
				if (enemyData.gate == gate)
				{
					int id = enemyData.id;
					if (EnemyDatabase.Instance.GetEnemyParameter(id, 0).isAir)
					{
						result = true;
					}
				}
			}
			return result;
		}

		public List<int> getListEnemyGate(int wave)
		{
			List<int> list = new List<int>();
			if (wave >= MonoSingleton<GameRecord>.Instance.TotalWave)
			{
				return null;
			}
			List<WaveEnemyEntry> listEnemyWithWave = MonoSingleton<GameRecord>.Instance.GetListEnemyWithWave(wave);
			foreach (WaveEnemyEntry enemyData in listEnemyWithWave)
			{
				if (!list.Contains(enemyData.gate))
				{
					list.Add(enemyData.gate);
				}
			}
			return list;
		}

		public List<List<EnemyParameter>> listEnemy = new List<List<EnemyParameter>>();

		private static EnemyDatabase instance;
		[UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			instance = null;
		}
	}
}
