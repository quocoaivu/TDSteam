using System;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
	public class TipInformationUIManager : MonoBehaviour
	{
		public static TipInformationUIManager Instance
		{
			get
			{
				return TipInformationUIManager.instance;
			}
			private set
			{
				TipInformationUIManager.instance = value;
			}
		}

		public void Awake()
		{
			TipInformationUIManager.Instance = this;
		}

		public void TryActivateButton(int tipID)
		{
			string path = string.Format("NewTip/ButtonNewTip", new object[0]);
			TipInformationButton tipInformationButton = UnityEngine.Object.Instantiate<TipInformationButton>(Common.AssetLoader.Load<TipInformationButton>(path));
			tipInformationButton.gameObject.SetActive(false);
			tipInformationButton.transform.SetParent(cardsRoot);
			tipInformationButton.transform.localScale = Vector3.one;
			tipInformationButton.TipId = tipID;
			tipInformationButton.ShowButton(buttonLifeTime);
			if (tipID == 5)
			{
				tipInformationButton.GetComponent<Image>().sprite = Common.AssetLoader.Load<Sprite>(string.Format("NewTip/icon_newtips_lives", new object[0]));
			}
			if (tipID == 6)
			{
				tipInformationButton.GetComponent<Image>().sprite = Common.AssetLoader.Load<Sprite>(string.Format("NewTip/icon_newtips_coin", new object[0]));
			}
		}

		[Space]
		[SerializeField]
		private Transform cardsRoot;

		[Space]
		[SerializeField]
		private float buttonLifeTime = 60f;

		private static TipInformationUIManager instance;

		// Reset the static singleton when entering Play mode so a stale
		// reference doesn't survive when "Reload Domain" is disabled.
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			instance = null;
		}
	}
}
