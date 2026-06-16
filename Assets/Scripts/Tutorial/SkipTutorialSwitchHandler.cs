using System;
using Data;

namespace Tutorial
{
	public class SkipTutorialSwitchHandler : SwitchHandler
	{
		public override void OnClick()
		{
			base.OnClick();
			TutorialStore.Instance.SkipAllTutorials();
			TutorialStore.Instance.SetCurrentTutorialPass();
		}
	}
}
