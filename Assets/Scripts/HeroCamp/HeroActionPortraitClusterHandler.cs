using System;
using System.Collections.Generic;
using GameCore;
using Parameter;
using UnityEngine;

namespace HeroCamp
{
	public class HeroActionPortraitClusterHandler : BaseMonoBehaviour
	{
		private void Awake()
		{
			InitListHeroActionAvatars();
		}

		private void InitListHeroActionAvatars()
		{
			if (listHeroActionAvatar.Count < 1)
			{
				List<int> listHeroID = HeroParameterManager.Instance.GetListHeroID();
				for (int i = 0; i < listHeroID.Count; i++)
				{
					HeroActionPortraitHandler heroActionAvatarController = UnityEngine.Object.Instantiate<HeroActionPortraitHandler>(Common.AssetLoader.Load<HeroActionPortraitHandler>(string.Format("HeroCamp/MiniActionAvatars/action_avatar_hero_{0}", listHeroID[i])));
					heroActionAvatarController.transform.SetParent(heroActionAvatarHolder);
					heroActionAvatarController.transform.localScale = Vector3.one;
					heroActionAvatarController.transform.localPosition = Vector3.zero;
					heroActionAvatarController.Init(listHeroID[i]);
					listHeroActionAvatar.Add(heroActionAvatarController);
					heroActionAvatarController.Hide();
				}
			}
		}

		public void ShowSelectedHeroActionAvatar(int currentHeroID)
		{
			HideAll();
			DisplayHeroActionAvatar(currentHeroID);
		}

		private void HideAll()
		{
			foreach (HeroActionPortraitHandler heroActionAvatarController in listHeroActionAvatar)
			{
				heroActionAvatarController.Hide();
			}
		}

		private void DisplayHeroActionAvatar(int currentHeroID)
		{
			for (int i = 0; i < listHeroActionAvatar.Count; i++)
			{
				if (listHeroActionAvatar[i].HeroID == currentHeroID)
				{
					listHeroActionAvatar[i].Show();
				}
			}
		}

		[SerializeField]
		private Transform heroActionAvatarHolder;

		private List<HeroActionPortraitHandler> listHeroActionAvatar = new List<HeroActionPortraitHandler>();
	}
}
