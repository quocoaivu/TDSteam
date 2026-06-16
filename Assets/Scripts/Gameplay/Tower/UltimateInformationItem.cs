using System;
using Parameter;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
	public class UltimateInformationItem : MonoBehaviour
	{
		public void Init(int _towerID, int _towerLevel, int posX, int skillID)
		{
			base.transform.localPosition = new Vector3((float)posX, 0f, 0f);
			ultimateName.text = Singleton<TurretSynopsis>.Instance.GetTowerUltimateName(_towerID, _towerLevel, skillID);
			ultimateDescription.text = Singleton<TurretSynopsis>.Instance.GetTowerUltimateDescription(_towerID, _towerLevel, skillID).Remove(0, 2).Replace('@', '\n').Replace('#', '-');
		}

		public void Show()
		{
			base.gameObject.SetActive(true);
		}

		public void Hide()
		{
			base.gameObject.SetActive(false);
		}

		[SerializeField]
		private Text ultimateName;

		[SerializeField]
		private Text ultimateDescription;
	}
}
