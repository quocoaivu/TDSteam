using System;
using UnityEngine;
using UnityEngine.UI;

namespace Guide
{
	public class SelectTurretSwitchHandler : SwitchHandler
	{
		private void Awake()
		{
			button = base.GetComponent<Button>();
		}

		public void Init(int _towerID, int _towerLevel)
		{
			towerID = _towerID;
			towerLevel = _towerLevel;
			SetAvatar(_towerID, _towerLevel);
		}

		private void SetAvatar(int _towerID, int _towerLevel)
		{
			towerAvatar.sprite = Common.AssetLoader.Load<Sprite>(string.Format("Preview/Towers/p_tower_{0}_{1}", _towerID, _towerLevel));
			towerAvatar.SetNativeSize();
			towerAvatar.transform.localScale = imageSize;
		}

		public void SetLock()
		{
			button.enabled = false;
			lockImage.SetActive(true);
			normalImage.SetActive(false);
		}

		public void SetUnLock()
		{
			button.enabled = true;
			normalImage.SetActive(true);
			lockImage.SetActive(false);
		}

		public override void OnClick()
		{
			base.OnClick();
			PrimerDialogHandler.Instance.ShowSelectedTowerImage(base.transform);
			PrimerDialogHandler.Instance.GuideTowerController.currentTowerIDSelected = towerID;
			PrimerDialogHandler.Instance.GuideTowerController.currentTowerLvSelected = towerLevel;
			PrimerDialogHandler.Instance.GuideTowerController.RefreshTowerInformation();
		}

		private Button button;

		private int towerID;

		private int towerLevel;

		[SerializeField]
		private GameObject lockImage;

		[SerializeField]
		private GameObject normalImage;

		[SerializeField]
		private Image towerAvatar;

		[SerializeField]
		private RectTransform avatarRectTransform;

		private Vector3 imageSize = new Vector3(0.6f, 0.6f, 0.6f);
	}
}
