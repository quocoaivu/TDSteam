using Data;
using Gameplay;
using UnityEditor;
using UnityEngine;

namespace EditorTools
{
	// Cheat tool: cá»™ng tiá»n (Money trong mÃ n) vÃ  Gem (lÆ°u file).
	// Má»Ÿ qua menu: Tools/Cheat/Currency
	public class CurrencyCheatWindow : EditorWindow
	{
		private int _moneyAmount = 1000;
		private int _gemAmount = 100;

		[MenuItem("Tools/Cheat/Currency")]
		private static void Open()
		{
			GetWindow<CurrencyCheatWindow>("Currency Cheat");
		}

		private void OnGUI()
		{
			EditorGUILayout.LabelField("Currency Cheat", EditorStyles.boldLabel);
			EditorGUILayout.HelpBox(
				"Money: tiá»n vÃ ng trong mÃ n chÆ¡i (chá»‰ cÃ³ tÃ¡c dá»¥ng khi Ä‘ang Play 1 level).\n" +
				"Gem: tiá»n premium, Ä‘Æ°á»£c lÆ°u vÃ o file ngay láº­p tá»©c.",
				MessageType.Info);

			DrawMoneySection();
			EditorGUILayout.Space(10);
			DrawGemSection();
		}

		private void DrawMoneySection()
		{
			EditorGUILayout.LabelField("Money (Gold)", EditorStyles.boldLabel);

			using (new EditorGUI.DisabledScope(!Application.isPlaying))
			{
				_moneyAmount = EditorGUILayout.IntField("Amount", _moneyAmount);

				GameRecord gameData = FindGameData();
				EditorGUILayout.LabelField("Current Money", gameData != null ? gameData.Money.ToString() : "(chÆ°a cÃ³ GameRecord)");

				EditorGUILayout.BeginHorizontal();
				if (GUILayout.Button("+ Add Money"))
				{
					AddMoney(_moneyAmount);
				}
				if (GUILayout.Button("- Subtract"))
				{
					AddMoney(-_moneyAmount);
				}
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				if (GUILayout.Button("+1k")) AddMoney(1000);
				if (GUILayout.Button("+10k")) AddMoney(10000);
				if (GUILayout.Button("+100k")) AddMoney(100000);
				EditorGUILayout.EndHorizontal();
			}

			if (!Application.isPlaying)
			{
				EditorGUILayout.HelpBox("VÃ o Play Mode Ä‘á»ƒ cá»™ng Money.", MessageType.Warning);
			}
		}

		private void DrawGemSection()
		{
			EditorGUILayout.LabelField("Gem", EditorStyles.boldLabel);

			using (new EditorGUI.DisabledScope(!Application.isPlaying))
			{
				_gemAmount = EditorGUILayout.IntField("Amount", _gemAmount);

				PlayerCurrencyStore currency = PlayerCurrencyStore.Instance;
				EditorGUILayout.LabelField("Current Gem", currency != null ? currency.GetCurrentGem().ToString() : "(chÆ°a cÃ³ Instance)");

				EditorGUILayout.BeginHorizontal();
				if (GUILayout.Button("+ Add Gem"))
				{
					AddGem(_gemAmount);
				}
				if (GUILayout.Button("- Subtract"))
				{
					AddGem(-_gemAmount);
				}
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				if (GUILayout.Button("+100")) AddGem(100);
				if (GUILayout.Button("+1k")) AddGem(1000);
				if (GUILayout.Button("+10k")) AddGem(10000);
				EditorGUILayout.EndHorizontal();
			}

			if (!Application.isPlaying)
			{
				EditorGUILayout.HelpBox("VÃ o Play Mode Ä‘á»ƒ cá»™ng Gem.", MessageType.Warning);
			}
		}

		private static GameRecord FindGameData()
		{
			return Object.FindAnyObjectByType<GameRecord>();
		}

		private static void AddMoney(int amount)
		{
			GameRecord gameData = FindGameData();
			if (gameData == null)
			{
				Debug.LogWarning("[CurrencyCheat] KhÃ´ng tÃ¬m tháº¥y GameRecord â€” pháº£i Ä‘ang á»Ÿ trong mÃ n gameplay.");
				return;
			}
			if (amount >= 0)
			{
				gameData.IncreaseMoney(amount);
			}
			else
			{
				gameData.DecreaseMoney(-amount);
			}
			Debug.Log($"[CurrencyCheat] Money {(amount >= 0 ? "+" : "")}{amount}. New = {gameData.Money}");
		}

		private static void AddGem(int amount)
		{
			if (PlayerCurrencyStore.Instance == null)
			{
				Debug.LogWarning("[CurrencyCheat] PlayerCurrencyStore.Instance == null.");
				return;
			}
			PlayerCurrencyStore.Instance.ChangeGem(amount, true);
			Debug.Log($"[CurrencyCheat] Gem {(amount >= 0 ? "+" : "")}{amount}. New = {PlayerCurrencyStore.Instance.GetCurrentGem()}");
		}
	}
}
