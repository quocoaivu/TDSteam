using System;
using Data;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay
{
	public class GameplayRecordLoader : MonoSingleton<GameplayRecordLoader>
	{
        [SerializeField]
        [FormerlySerializedAs("readDataLuckyChest")]
        private LuckyChestDataLoader luckyChestLoader;

        [SerializeField]
        [FormerlySerializedAs("readDataEndGameVideo")]
        private EndGameVideoProvider endGameVideoProvider;

        public LuckyChestDataLoader LuckyChestLoader
		{
			get
			{
				return luckyChestLoader;
			}
			set
			{
				luckyChestLoader = value;
			}
		}

		public EndGameVideoProvider EndGameVideoProvider
		{
			get
			{
				return endGameVideoProvider;
			}
			set
			{
				endGameVideoProvider = value;
			}
		}
	}
}
