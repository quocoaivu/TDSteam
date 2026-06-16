using System;
using GameCore;

namespace Gameplay
{
	public class Hero4Ability1Inferno : BaseMonoBehaviour
	{
		public void Init(float lifeTime)
		{
			Show();
			base.CustomInvoke(new Action(Hide), lifeTime);
		}

		private void Show()
		{
			base.gameObject.SetActive(true);
		}

		public void Hide()
		{
			base.gameObject.SetActive(false);
		}
	}
}
