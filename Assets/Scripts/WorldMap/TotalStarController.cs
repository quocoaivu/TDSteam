using Data;
using UnityEngine;
using UnityEngine.UI;

namespace WorldMap
{
	public class TotalStarController : MonoBehaviour
	{
        [SerializeField]
        private Text textStar;

        private int totalMap;

        private int maxStarPerMap = 3;

        private void Start()
		{
			totalMap = MapProgressStore.Instance.GetTotalMap();
			UpdateStar();
		}

		private void UpdateStar()
		{
			textStar.text = PlayerCurrencyStore.Instance.GetCurrentStar().ToString() + "/" + (totalMap * maxStarPerMap).ToString();
		}
	}
}
