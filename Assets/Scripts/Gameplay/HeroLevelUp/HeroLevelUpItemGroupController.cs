using System;
using System.Collections.Generic;
using Data;
using GameCore;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay
{
	public class HeroLevelUpItemGroupController : BaseMonoBehaviour
	{
		public void InitData()
		{
			base.CustomInvoke(new Action(OpenWithEffect), 0f);
			base.CustomInvoke(new Action(DisplayExpBarAfterCalculating), 2f);
		}

		private void DisplayExpBarAfterCalculating()
		{
			GameplayDirector.Instance.heroesManager.CalculateExp();
			foreach (HeroLevelUpCard heroLevelUpItem in listHeroLevelUpItem)
			{
				heroLevelUpItem.InitExpBarAfterCalculating();
			}
		}

		private void OpenWithEffect()
		{
			Open();
			ClearItems();
			MonoSingleton<LensHandler>.Instance.PinchZoomFov.MoveToOriginPos();
			foreach (int heroID in MonoSingleton<GameRecord>.Instance.ListHeroesIdsSelected)
			{
				HeroLevelUpCard heroLevelUpItem = UnityEngine.Object.Instantiate<HeroLevelUpCard>(heroLevelUpItemPrefab);
				heroLevelUpItem.transform.SetParent(listHeroItemsParent, false);
				heroLevelUpItem.transform.localScale = Vector3.one;
				heroLevelUpItem.Init(heroID);
				listHeroLevelUpItem.Add(heroLevelUpItem);
			}
		}

		private void ClearItems()
		{
			foreach (HeroLevelUpCard heroLevelUpItem in listHeroLevelUpItem)
			{
				UnityEngine.Object.Destroy(heroLevelUpItem.gameObject);
			}
			listHeroLevelUpItem.Clear();
		}

		private void Open()
		{
			base.gameObject.SetActive(true);
		}

		private void Close()
		{
			base.gameObject.SetActive(false);
		}

		[SerializeField]
		private Transform listHeroItemsParent;

		private List<HeroLevelUpCard> listHeroLevelUpItem = new List<HeroLevelUpCard>();

		[SerializeField]
		[FormerlySerializedAs("heroLevelUpItemPrefabs")]
		private HeroLevelUpCard heroLevelUpItemPrefab;
	}
}
