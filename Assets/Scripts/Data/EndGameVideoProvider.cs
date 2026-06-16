using UnityEngine;

namespace Data
{
	public class EndGameVideoProvider : MonoBehaviour
	{
        [SerializeField]
        private EndGameClip endGameVideoParam;

        public int GetCountdownTime()
		{
			return endGameVideoParam.param.countDownTimeSecond;
		}

		public int GetEndGameVideoReward()
		{
			return endGameVideoParam.param.lifeReward;
		}
	}
}
