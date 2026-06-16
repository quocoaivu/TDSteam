using System;
using System.Collections.Generic;
using Data;
using Parameter;
using UnityEngine;

namespace MapLevel
{
	public class HerosSelectClusterHandler : MonoBehaviour
	{
		public void InitListHeroToSelect()
		{
			List<int> listHeroID = HeroParameterManager.Instance.GetListHeroID();
			if (listHeroToSelect.Count == 0)
			{
				for (int i = 0; i < listHeroID.Count; i++)
				{
					HeroSelectSwitchHandler heroSelectButtonController = UnityEngine.Object.Instantiate<HeroSelectSwitchHandler>(heroSelectButtonPrefab);
					heroSelectButtonController.transform.SetParent(listButtonParent);
					heroSelectButtonController.transform.localScale = Vector3.one;
					heroSelectButtonController.Init(listHeroID[i], heroesInputGroupController);
					listHeroToSelect.Add(heroSelectButtonController);
				}
			}
			UpdateButtonsStatus();
			SetContentSize();
			SetDefaultValue();
			InitSavedValue();
		}

		public void UpdateButtonsStatus()
		{
			foreach (HeroSelectSwitchHandler heroSelectButtonController in listHeroToSelect)
			{
				heroSelectButtonController.UpdateStatus();
			}
		}

		private void SetDefaultValue()
		{
			foreach (HeroSelectSwitchHandler heroSelectButtonController in listHeroToSelect)
			{
				heroSelectButtonController.SetDefaultValue();
			}
		}

		private void InitSavedValue()
		{
			int num = 3;
			int[] listHeroIDSaved = HeroPrepareStore.Instance.GetListHeroIDSaved();
			for (int i = 0; i < num; i++)
			{
				foreach (HeroSelectSwitchHandler heroSelectButtonController in listHeroToSelect)
				{
					if (heroSelectButtonController.HeroID == listHeroIDSaved[i])
					{
						heroSelectButtonController.OnClick();
					}
				}
			}
		}

		private void SetContentSize()
		{
			Vector2 sizeDelta = content.sizeDelta;
			sizeDelta.Set(sizeDelta.x, cellSize * (float)listHeroToSelect.Count);
			content.sizeDelta = sizeDelta;
		}

		public void UnChooseHero(int heroID)
		{
			foreach (HeroSelectSwitchHandler heroSelectButtonController in listHeroToSelect)
			{
				if (heroSelectButtonController.HeroID == heroID)
				{
					heroSelectButtonController.SetView_NonSelect();
				}
			}
		}

		[Space]
		[Header("Reference")]
		[SerializeField]
		private HerosInputClusterHandler heroesInputGroupController;

		[Space]
		[Header("Init button")]
		[SerializeField]
		private HeroSelectSwitchHandler heroSelectButtonPrefab;

		[SerializeField]
		private Transform listButtonParent;

		private List<HeroSelectSwitchHandler> listHeroToSelect = new List<HeroSelectSwitchHandler>();

		[Space]
		[Header("Set Content size by number of objects")]
		[SerializeField]
		private float cellSize;

		[SerializeField]
		private RectTransform content;
	}
}
