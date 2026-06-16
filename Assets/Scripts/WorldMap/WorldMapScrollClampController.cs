using Data;
using GameCore;
using UnityEngine;
using UnityEngine.Serialization;

namespace WorldMap
{
	public class WorldMapScrollClampController : BaseMonoBehaviour
	{
        [SerializeField]
        private RectTransform Content;

        [SerializeField]
        private float contentMinValue;

        [SerializeField]
        private float contentMaxValue;

        [Space]
        [Header("Clamp scrollview")]
        [SerializeField]
        private RectTransform viewPort;

        [SerializeField]
        [FormerlySerializedAs("scrollViewMapController")]
        private MapScrollViewController mapScrollViewController;

        private Vector2 currentPosition;

        private float scaleRatio;

        private float clampedYValue;

        private const float REFERENCE_WIDTH = 1280f;

		private void Update()
		{
			ClampPosition();
		}

		private void ClampPosition()
		{
			currentPosition = Content.anchoredPosition3D;
			currentPosition.x = Mathf.Clamp(currentPosition.x, contentMinValue, contentMaxValue);
			scaleRatio = mapScrollViewController.OriginWidth / REFERENCE_WIDTH;
			clampedYValue = (Content.sizeDelta.y - viewPort.sizeDelta.y / scaleRatio) / 2f;
			currentPosition.y = Mathf.Clamp(currentPosition.y, -clampedYValue, clampedYValue);
			Content.anchoredPosition3D = currentPosition;
		}
	}
}
