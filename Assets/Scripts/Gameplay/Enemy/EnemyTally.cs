using System;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
	public class EnemyTally : MonoBehaviour
	{
		private void Update()
		{
			text.text = MonoSingleton<GameRecord>.Instance.ListActiveEnemy.Count.ToString();
		}

		public Text text;
	}
}
