using System;
using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class InfiniteFormatDirector : MonoBehaviour
	{
        [Header("Wave láº·p báº¯t Ä‘áº§u táº¡i: ")]
        public int waveLoopBegin;

        [Header("Wave láº·p káº¿t thÃºc táº¡i: ")]
        public int waveLoopEnd;

        [Header("Chá»‰ sá»‘ mÃ¡u tÄƒng qua má»—i wave - Ä‘Æ¡n vá»‹ % ")]
        public float healthIncreasePercentage;

        [Header("Chá»‰ sá»‘ damage tÄƒng qua má»—i wave - Ä‘Æ¡n vá»‹ % ")]
        public float damageIncreasePercentage;

        [Header("Sá»‘ láº§n loop")]
        public int LoopAmount;

        [Header("Sá»‘ wave Ä‘Ã£ vÆ°á»£t qua")]
        public int TotalWavesPassed;

        [Header("Enemy cuá»‘i cÃ¹ng cá»§a normal wave")]
        public bool IsLastEnemyInNormalWave;

        private int currentWaveEndless;


        private bool increased;
        public int CurrentWaveEndless
		{
			get
			{
				return currentWaveEndless;
			}
			set
			{
				currentWaveEndless = value;
				if (value > waveLoopEnd)
				{
					IncreaseLoopAmount();
					currentWaveEndless = waveLoopBegin;
				}
			}
		}

		public void FirstTimeIncreaseLoopAmount()
		{
			if (!increased)
			{
				IncreaseLoopAmount();
				increased = true;
			}
		}

		public void IncreaseLoopAmount()
		{
			LoopAmount++;
		}

		public void IncreaseWavePassed()
		{
			TotalWavesPassed++;
		}

		public void IncreaseCurrentWaveEndless()
		{
			CurrentWaveEndless++;
		}

		public void SetLastEnemyInNormalWave(bool lastEnemyInBattle)
		{
			IsLastEnemyInNormalWave = lastEnemyInBattle;
		}

		public void Init()
		{
			waveLoopBegin = ZoneRuleSpec.Instance.GetEndlessWaveLoopBegin();
			waveLoopEnd = ZoneRuleSpec.Instance.GetEndlessWaveLoopEnd();
			healthIncreasePercentage = (float)ZoneRuleSpec.Instance.GetEndlessHealthIncreasePercentage();
			damageIncreasePercentage = (float)ZoneRuleSpec.Instance.GetEndlessDamageIncreasePercentage();
			TotalWavesPassed = 0;
			IsLastEnemyInNormalWave = false;
			LoopAmount = 0;
			CurrentWaveEndless = waveLoopBegin;
		}


	}
}
