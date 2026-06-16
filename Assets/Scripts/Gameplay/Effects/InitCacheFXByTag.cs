using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	public class InitCacheFXByTag : MonoBehaviour
	{
        [SerializeField]
        private List<string> listEffectsName = new List<string>();


        private void Start()
		{
			InitPoolFXs();
		}

		private void InitPoolFXs()
		{
			foreach (string arg in listEffectsName)
			{
				VisualEffectInstance effectController = UnityEngine.Object.Instantiate<VisualEffectInstance>(Common.AssetLoader.Load<VisualEffectInstance>(string.Format("FXs/{0}", arg)));
				effectController.gameObject.SetActive(false);
								Common.GameObjectPool.ManagePool(effectController.gameObject, 0);
				Common.GameObjectPool.Despawn(effectController.gameObject);
			}
		}
	}
}
