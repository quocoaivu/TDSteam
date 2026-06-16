using System;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
	public class TurretTally : MonoBehaviour
	{
		private void Update()
		{
			text.text = MonoSingleton<GameRecord>.Instance.ListActiveTower.Count.ToString();
		}

		public Text text;
	}
}
