using System;
using UnityEngine;

namespace Gameplay
{
	public class ProducedBullionHandler : MonoBehaviour
	{
		public void ResetParameter()
		{
			totalGold = 0;
			Hide();
		}

		public void Init(int goldAmount)
		{
			Show();
			totalGold += goldAmount;
			totalGoldText.text = totalGold.ToString();
		}

		public void TapOnGold()
		{
			MonoSingleton<GameRecord>.Instance.IncreaseMoney(totalGold);
			ResetParameter();
		}

		private void Show()
		{
			base.gameObject.SetActive(true);
			totalGoldText.gameObject.SetActive(true);
		}

		private void Hide()
		{
			base.gameObject.SetActive(false);
			totalGoldText.gameObject.SetActive(false);
		}

		[SerializeField]
		private TextMesh totalGoldText;

		private int totalGold;
	}
}
