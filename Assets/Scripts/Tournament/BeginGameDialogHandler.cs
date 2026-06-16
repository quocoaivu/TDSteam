using System;
using Data;
using Gameplay;
using MapLevel;
using MetaGame;
using Parameter;
using UnityEngine;

namespace Tournament
{
	public class BeginGameDialogHandler : GameplayDialogHandler
	{
		public void Init()
		{
			CrossSceneData.Instance.MapIDSelected = ZoneRuleSpec.Instance.GetCurrentSeasonMapID();
			OpenWithScaleAnimation();
			base.transform.SetAsLastSibling();
			heroesInputGroupController.HeroesSelectedController.InitHeroesSlot();
			heroesInputGroupController.HeroesSelectController.InitListHeroToSelect();
			powerUpItemGroupController.RefreshPowerupItems();
		}

		public void StartGame()
		{
		}

		public void InitListHeroesIDSelected()
		{
			int num = 3;
			CrossSceneData.Instance.ClearListHeroID();
			for (int i = 0; i < num; i++)
			{
				if (heroesInputGroupController.HeroesSelectedController.listHeroesIDSelected[i] >= 0)
				{
					CrossSceneData.Instance.AddHeroIDToList(heroesInputGroupController.HeroesSelectedController.listHeroesIDSelected[i]);
				}
			}
		}

		public void SaveListHeroIDSelected()
		{
			HeroPrepareStore.Instance.Save(heroesInputGroupController.HeroesSelectedController.listHeroesIDSelected);
		}

		public override void OpenWithScaleAnimation()
		{
			base.OpenWithScaleAnimation();
		}

		public override void CloseWithScaleAnimation()
		{
			base.CloseWithScaleAnimation();
			SaveListHeroIDSelected();
			if (heroesInputGroupController.AskToBuyHeroDialogHandler.isOpen)
			{
				heroesInputGroupController.AskToBuyHeroDialogHandler.CloseWithScaleAnimation();
			}
		}

		[Space]
		[Header("Controllers")]
		[SerializeField]
		private HerosInputClusterHandler heroesInputGroupController;

		[SerializeField]
		private PowerUpItemClusterHandler powerUpItemGroupController;
	}
}
