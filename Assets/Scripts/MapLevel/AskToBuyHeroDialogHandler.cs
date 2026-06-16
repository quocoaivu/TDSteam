using System;
using Gameplay;
using WorldMap;

namespace MapLevel
{
	public class AskToBuyHeroDialogHandler : GameplayDialogHandler
	{
		public void GoToHeroCamp()
		{
			MonoSingleton<WorldMap.UIRootHandler>.Instance.heroCampPopupController.Init();
			MonoSingleton<WorldMap.UIRootHandler>.Instance.heroCampPopupController.ChooseDefaultHero(currentHeroIDSelected);
			CloseWithScaleAnimation();
		}

		public void Init(int heroID)
		{
			currentHeroIDSelected = heroID;
			OpenWithScaleAnimation();
		}

		private int currentHeroIDSelected;
	}
}
