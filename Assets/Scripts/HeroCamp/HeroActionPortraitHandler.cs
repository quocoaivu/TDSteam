using System;
using GameCore;

namespace HeroCamp
{
	public class HeroActionPortraitHandler : BaseMonoBehaviour
	{
		public int HeroID { get; set; }

		public void Init(int _heroID)
		{
			HeroID = _heroID;
		}

		public void Show()
		{
			base.gameObject.SetActive(true);
		}

		public void Hide()
		{
			base.gameObject.SetActive(false);
		}
	}
}
