using System;
using System.Collections;
using UnityEngine;

namespace MainMenu
{
	public class BeginSwitchHandler : SwitchHandler
	{
		public override void OnClick()
		{
			base.OnClick();
			base.StartCoroutine(StartGame());
		}

		private IEnumerator StartGame()
		{
			yield return new WaitForSeconds(timeToOpen);
			LoadingScreen.Instance.ShowLoading();
			yield return new WaitForSeconds(1f);
			LoadingAdsManager.Instance.TryToShowInterstitialAds_Loading();
			GameSceneLoader.Instance.LoadScene(GameSceneLoader.WorldMapSceneName);
			yield break;
            //PlayerPrefs.DeleteAll();
        }

		[SerializeField]
		private float timeToOpen;
	}
}
