using Gameplay;
using UnityEngine;

namespace Tutorial
{
	// Base chung cho cÃ¡c panel tutorial: báº­t/táº¯t GameObject vÃ  cáº­p nháº­t cá» IsAnyTutorialPopupOpen.
	public class TutorialPopupPanel : MonoBehaviour
	{
		public void Show()
		{
			base.gameObject.SetActive(true);
			MonoSingleton<GameRecord>.Instance.IsAnyTutorialPopupOpen = true;
		}

		public void Hide()
		{
			base.gameObject.SetActive(false);
			MonoSingleton<GameRecord>.Instance.IsAnyTutorialPopupOpen = false;
		}
	}
}
