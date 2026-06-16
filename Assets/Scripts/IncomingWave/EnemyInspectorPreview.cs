using System;
using GameCore;
using UnityEngine;
using UnityEngine.UI;

namespace IncomingWave
{
	public class EnemyInspectorPreview : BaseMonoBehaviour
	{
		public void Init(int enemyID, int amount)
		{
			this.enemyID = enemyID;
			avatar.sprite = Common.AssetLoader.Load<Sprite>(string.Format("Preview/Enemies/p_enemy_{0}", enemyID));
			avatar.SetNativeSize();
			enemyAmount = amount;
			this.amount.text = "X " + enemyAmount.ToString();
			Show();
		}

		private void Show()
		{
			base.gameObject.SetActive(true);
		}

		public void Hide()
		{
			base.gameObject.SetActive(false);
		}

		[SerializeField]
		private Image avatar;

		[SerializeField]
		private Text amount;

		private int enemyID;

		private int enemyAmount;
	}
}
