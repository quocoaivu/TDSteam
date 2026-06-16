using System;
using UnityEngine;

namespace Guide
{
	public class EnemyVisual : MonoBehaviour
	{
		public void Init(int enemyID)
		{
			this.enemyID = enemyID;
		}

		public void Hide()
		{
			base.gameObject.SetActive(false);
		}

		public void Display()
		{
			base.gameObject.SetActive(true);
		}

		public int enemyID;
	}
}
