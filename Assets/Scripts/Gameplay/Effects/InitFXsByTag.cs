using System;
using UnityEngine;

namespace Gameplay
{
	public class InitFXsByTag : MonoBehaviour
	{
        [SerializeField]
        private string[] effectName;

        private void Awake()
		{
			foreach (string text in effectName)
			{
				MonoSingleton<FXPool>.Instance.InitFX(text);
			}
		}
	}
}
