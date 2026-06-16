using System;
using UnityEngine;
using UnityEngine.UI;

namespace RewardPopup
{
	public class RewardItemView : MonoBehaviour
	{
		public void Init(PrizeItem data)
		{
			Show();
			if (data != null && data.isDisplayQuantity && data.value == 0)
			{
				Hide();
			}
			GameKit.SetRewardSprite(data, itemAvatar);
			itemQuantity.text = data.value.ToString();
			itemQuantity.gameObject.SetActive(data.isDisplayQuantity);
		}

		private void Show()
		{
			base.gameObject.SetActive(true);
		}

		public void Hide()
		{
			base.gameObject.SetActive(false);
		}

		[SerializeField]
		private Image itemAvatar;

		[SerializeField]
		private Text itemQuantity;
	}
}
