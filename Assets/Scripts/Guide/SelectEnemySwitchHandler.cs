using System;
using UnityEngine;
using UnityEngine.UI;

namespace Guide
{
	public class SelectEnemySwitchHandler : SwitchHandler
	{
		private void Awake()
		{
			button = base.GetComponent<Button>();
			miniAvatar = base.GetComponent<Image>();
		}

		public void Init(int _enemyID)
		{
			enemyID = _enemyID;
			SetAvatar(_enemyID);
		}

		private void SetAvatar(int _enemyID)
		{
			enemyAvatar.sprite = Common.AssetLoader.Load<Sprite>(string.Format("Preview/Enemies/p_enemy_{0}", _enemyID));
			enemyAvatar.SetNativeSize();
			enemyAvatar.transform.localScale = Vector3.one;
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
			PrimerDialogHandler.Instance.ShowSelectedEnemyImage(base.transform);
			PrimerDialogHandler.Instance.GuideEnemyController.currentEnemyIDSelected = enemyID;
			PrimerDialogHandler.Instance.GuideEnemyController.RefreshEnemyInformation();
		}

		private Button button;

		private Image miniAvatar;

		private int enemyID;

		[SerializeField]
		private GameObject lockImage;

		[SerializeField]
		private GameObject normalImage;

		[SerializeField]
		private Image enemyAvatar;

		[SerializeField]
		private RectTransform avatarRectTransform;
	}
}
