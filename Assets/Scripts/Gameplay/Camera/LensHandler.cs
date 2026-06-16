using System;
using UnityEngine;

namespace Gameplay
{
	public class LensHandler : MonoSingleton<LensHandler>
	{
        [SerializeField]
        private PinchMagnifyView pinchZoomFov;

        [SerializeField]
        private LensHolderHandler cameraHolderController;

        public PinchMagnifyView PinchZoomFov
		{
			get
			{
				return pinchZoomFov;
			}
			private set
			{
				pinchZoomFov = value;
			}
		}

		private void Awake()
		{
			PinchZoomFov = base.GetComponent<PinchMagnifyView>();
		}

		private void Start()
		{
			PinchZoomFov.enabled = true;
		}

		[ContextMenu("Shake")]
		public void ShakeNormal()
		{
			cameraHolderController.ShakeNormal();
		}
	}
}
