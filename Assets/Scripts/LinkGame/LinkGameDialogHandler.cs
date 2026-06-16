using System;
using Gameplay;
using UnityEngine;

namespace LinkGame
{
	public class LinkGameDialogHandler : GameplayDialogHandler
	{
		public void Init()
		{
			OpenWithScaleAnimation();
		}

		public void DirectLinkToGame()
		{
			UnityEngine.Debug.Log("Link to store!");
			Application.OpenURL(MarketingSetup.goe_trackingLink);
			CloseWithScaleAnimation();
		}
	}
}
