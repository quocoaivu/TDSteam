using System;
using MetaGame;
using GameCore;
using UnityEngine;

namespace Gameplay
{
	public class ConstructSectorHandler : BaseMonoBehaviour
	{
        public int regionID;

        private BoxCollider2D boxCollider;

        [SerializeField]
        private GameObject buildableImage;

        public Transform spawnAllyPosition;

        private void Awake()
		{
			GetAllComponents();
		}

		private void GetAllComponents()
		{
			boxCollider = base.GetComponent<BoxCollider2D>();
		}

		public void DisplayBuildable()
		{
			TurnOnCollider();
			buildableImage.SetActive(true);
		}

		public void DisplayNotBuildable()
		{
			TurnOffCollider();
			buildableImage.SetActive(false);
		}

		public void TurnOnCollider()
		{
			boxCollider.enabled = true;
		}

		public void TurnOffCollider()
		{
			boxCollider.enabled = false;
		}

		public void TryToClick()
		{
			if (MonoSingleton<UIRootHandler>.Instance.BuyTowerPopupController.isOpen)
			{
				GameplayDirector.Instance.CurrentTowerRange.GetComponent<TurretRangeHandler>().HideRange();
			}
			MonoSingleton<UIRootHandler>.Instance.BuyTowerPopupController.Init(base.transform);
			Setup.Instance.currentTowerRegionIDSelected = regionID;
		}
	}
}
