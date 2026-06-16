using System.Collections.Generic;
using GameCore;
using Parameter;
using UnityEngine;

namespace MetaGame
{
	public class EnemyDiscoveryTracker : BaseMonoBehaviour
	{
		// PlayerPrefs key per enemy. Value 0 = first time (not seen), 1 = unlocked.
		private const string KEY_FORMAT = "EnemyUnlocked_{0}";

		// _enemiesFirstTime[id] == true means the enemy has NOT been seen yet.
		private static bool[] _enemiesFirstTime;

		public static EnemyDiscoveryTracker Instance { get; private set; }

		private void Awake()
		{
			Instance = this;
			EnsureLoaded();
		}

		// NOTE: has a side effect â€” unlocks the enemy when it is seen for the first time.
		public bool IsEnemyFirstTime(int enemyId)
		{
			EnsureLoaded();
			if (!IsValidId(enemyId))
			{
				return false;
			}

			bool isFirstTime = _enemiesFirstTime[enemyId];
			if (isFirstTime)
			{
				UnlockEnemy(enemyId);
			}
			return isFirstTime;
		}

		public static bool EnemyAppeared(int enemyId)
		{
			EnsureLoaded();
			return IsValidId(enemyId) && !_enemiesFirstTime[enemyId];
		}

		public List<bool> GetListEnemyUnlockStatus()
		{
			EnsureLoaded();
			List<bool> unlockStatus = new List<bool>(_enemiesFirstTime.Length);
			for (int i = 0; i < _enemiesFirstTime.Length; i++)
			{
				unlockStatus.Add(!_enemiesFirstTime[i]);
			}
			return unlockStatus;
		}

		private static void UnlockEnemy(int enemyId)
		{
			_enemiesFirstTime[enemyId] = false;
			PlayerPrefs.SetInt(string.Format(KEY_FORMAT, enemyId), 1);
			PlayerPrefs.Save();
		}

		// Builds the first-time array from PlayerPrefs. Rebuilds if the enemy
		// count is not ready yet at Awake time, so init order does not matter.
		private static void EnsureLoaded()
		{
			int count = EnemyDatabase.Instance.GetNumberOfEnemy();
			if (_enemiesFirstTime != null && _enemiesFirstTime.Length == count)
			{
				return;
			}

			_enemiesFirstTime = new bool[count];
			for (int i = 0; i < count; i++)
			{
				_enemiesFirstTime[i] = PlayerPrefs.GetInt(string.Format(KEY_FORMAT, i), 0) == 0;
			}
		}

		private static bool IsValidId(int enemyId)
		{
			return _enemiesFirstTime != null && enemyId >= 0 && enemyId < _enemiesFirstTime.Length;
		}
	}
}
