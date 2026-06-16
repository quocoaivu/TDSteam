using UnityEditor;
using UnityEngine;

namespace EditorTools
{
	// Xóa PlayerPrefs đã lưu trên Editor.
	// Menu: Tools/Cheat/PlayerPrefs/...
	public static class PlayerPrefsCleaner
	{
		// Phải khớp KEY_FORMAT trong MetaGame.EnemyDiscoveryTracker.
		private const string ENEMY_KEY_FORMAT = "EnemyUnlocked_{0}";

		// Quét rộng hơn số enemy thực tế để chắc chắn xóa hết.
		private const int MAX_ENEMY_ID = 200;

		[MenuItem("Tools/Cheat/PlayerPrefs/Clear All")]
		private static void ClearAll()
		{
			bool confirmed = EditorUtility.DisplayDialog(
				"Clear All PlayerPrefs",
				"Xóa TOÀN BỘ PlayerPrefs (gem, settings, unlock...). Không thể hoàn tác. Tiếp tục?",
				"Xóa hết",
				"Hủy");
			if (!confirmed)
			{
				return;
			}

			PlayerPrefs.DeleteAll();
			PlayerPrefs.Save();
			Debug.Log("[PlayerPrefsCleaner] Đã xóa toàn bộ PlayerPrefs.");
		}

		[MenuItem("Tools/Cheat/PlayerPrefs/Clear Unlocked Enemies")]
		private static void ClearUnlockedEnemies()
		{
			int removed = 0;
			for (int i = 0; i < MAX_ENEMY_ID; i++)
			{
				string key = string.Format(ENEMY_KEY_FORMAT, i);
				if (PlayerPrefs.HasKey(key))
				{
					PlayerPrefs.DeleteKey(key);
					removed++;
				}
			}

			PlayerPrefs.Save();
			Debug.Log($"[PlayerPrefsCleaner] Đã xóa {removed} key {ENEMY_KEY_FORMAT}. Enemy sẽ được coi là 'mới' trở lại.");
		}
	}
}
