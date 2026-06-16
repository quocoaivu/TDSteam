using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	public class EnemyDiscoveryUIManager : MonoBehaviour
	{
		public static EnemyDiscoveryUIManager Instance
		{
			get
			{
				return EnemyDiscoveryUIManager.instance;
			}
			private set
			{
				EnemyDiscoveryUIManager.instance = value;
			}
		}

		public void Awake()
		{
			EnemyDiscoveryUIManager.Instance = this;
		}

		private void Start()
		{
			InitializeButtons();
		}

		public void TryActivateButton(int enemyId)
		{
			foreach (EnemyDiscoveryButton enemyDiscoveryButton in buttons)
			{
				if (enemyDiscoveryButton.EnemyId == enemyId)
				{
					enemyDiscoveryButton.ShowButton(buttonLifeTime);
				}
			}
		}

		private void InitializeButtons()
		{
			List<int> listEnemyID = MonoSingleton<GameRecord>.Instance.ListEnemyID;
			string path = string.Empty;
			for (int i = 0; i < listEnemyID.Count; i++)
			{
				path = string.Format("NewEnemy/ButtonNewEnemy", new object[0]);
				EnemyDiscoveryButton enemyDiscoveryButton = UnityEngine.Object.Instantiate<EnemyDiscoveryButton>(Common.AssetLoader.Load<EnemyDiscoveryButton>(path));
				enemyDiscoveryButton.Init(listEnemyID[i]);
				enemyDiscoveryButton.gameObject.SetActive(false);
				enemyDiscoveryButton.transform.SetParent(cardsRoot);
				enemyDiscoveryButton.transform.localScale = Vector3.one;
				buttons.Add(enemyDiscoveryButton);
			}
		}

		[Space]
		[SerializeField]
		private Transform cardsRoot;

		[Space]
		private List<EnemyDiscoveryButton> buttons = new List<EnemyDiscoveryButton>();

		[Space]
		[SerializeField]
		private float buttonLifeTime = 60f;

		private static EnemyDiscoveryUIManager instance;
		[UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			instance = null;
		}
	}
}
