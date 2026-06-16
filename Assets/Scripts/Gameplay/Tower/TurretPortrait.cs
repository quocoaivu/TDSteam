using System;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
	public class TurretPortrait : MonoBehaviour
	{
		public void Init(int towerID, int towerLevel)
		{
			this.towerID = towerID;
			this.towerLevel = towerLevel;
			image.sprite = Common.AssetLoader.Load<Sprite>(string.Format("Preview/Towers/p_tower_{0}_{1}", towerID, towerLevel));
		}

		public void Hide()
		{
			base.gameObject.SetActive(false);
		}

		public void Show()
		{
			base.gameObject.SetActive(true);
		}

		[SerializeField]
		private Image image;

		private int towerID;

		private int towerLevel;
	}
}
