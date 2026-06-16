using System;
using System.Collections.Generic;
using GameCore;
using Parameter;
using UnityEngine;

namespace HeroCamp
{
	public class PetActionPortraitClusterHandler : BaseMonoBehaviour
	{
		private void InitListPetActionAvatars()
		{
			if (listPetActionAvatar.Count < 1)
			{
				List<int> listPetID = HeroParameterManager.Instance.GetListPetID();
				for (int i = 0; i < listPetID.Count; i++)
				{
					HeroActionPortraitHandler heroActionAvatarController = UnityEngine.Object.Instantiate<HeroActionPortraitHandler>(Common.AssetLoader.Load<HeroActionPortraitHandler>(string.Format("HeroCamp/MiniActionAvatars/action_avatar_hero_{0}", listPetID[i])));
					heroActionAvatarController.transform.SetParent(petActionAvatarHolder);
					heroActionAvatarController.transform.localScale = Vector3.one;
					heroActionAvatarController.transform.localPosition = Vector3.zero;
					heroActionAvatarController.transform.localRotation = Quaternion.Euler(Vector3.zero);
					heroActionAvatarController.Init(listPetID[i]);
					listPetActionAvatar.Add(heroActionAvatarController);
					heroActionAvatarController.Hide();
				}
			}
		}

		public void ShowSelectedPetActionAvatar(int petID)
		{
			InitListPetActionAvatars();
			HideAll();
			DisplayPetActionAvatar(petID);
		}

		public void HideAll()
		{
			foreach (HeroActionPortraitHandler heroActionAvatarController in listPetActionAvatar)
			{
				heroActionAvatarController.Hide();
			}
		}

		private void DisplayPetActionAvatar(int petID)
		{
			for (int i = 0; i < listPetActionAvatar.Count; i++)
			{
				if (listPetActionAvatar[i].HeroID == petID)
				{
					listPetActionAvatar[i].Show();
				}
			}
		}

		[SerializeField]
		private Transform petActionAvatarHolder;

		private List<HeroActionPortraitHandler> listPetActionAvatar = new List<HeroActionPortraitHandler>();
	}
}
