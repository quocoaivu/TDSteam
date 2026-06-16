using System;
using GameCore;
using Parameter;
using UnityEngine;
using UnityEngine.UI;

namespace Guide
{
	public class HintOverviewHandler : BaseMonoBehaviour
	{
		public void InitInformation(int tipID)
		{
			tipAvatar.sprite = Common.AssetLoader.Load<Sprite>(string.Format("NewTip/avatar_tip_{0}", tipID));
			tipAvatar.SetNativeSize();
			tipName.text = Singleton<GameplayTipsSynopsis>.Instance.GetName(tipID);
			tipDescription.text = Singleton<GameplayTipsSynopsis>.Instance.GetDescription(tipID).Replace('@', '\n').Replace('#', '-');
		}

		[SerializeField]
		private Image tipAvatar;

		[SerializeField]
		private Text tipName;

		[SerializeField]
		private Text tipDescription;
	}
}
