using System;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
	public class MinionTally : MonoBehaviour
	{
		private void Update()
		{
			text.text = MonoSingleton<GameRecord>.Instance.ListActiveAlly.Count.ToString();
		}

		public Text text;
	}
}
