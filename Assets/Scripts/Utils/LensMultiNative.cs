using System;
using UnityEngine;

public class LensMultiNative : MonoBehaviour
{

    [SerializeField]
    private bool isOrthographic;

    private float lastScreenAspectRatio;

    [SerializeField]
    private float offsetForShake = 0.1f;

    private void Awake()
	{
		SmartSize();
		lastScreenAspectRatio = Camera.main.aspect;
	}

	private void Update()
	{
		if (lastScreenAspectRatio != Camera.main.aspect)
		{
			SmartSize();
			lastScreenAspectRatio = Camera.main.aspect;
		}
	}

	[ContextMenu("SmartSize")]
	private void SmartSize()
	{
		Camera component = base.GetComponent<Camera>();
		if (isOrthographic)
		{
			component.orthographicSize = CameraOrthographicSizeCalculator() - offsetForShake;
		}
		else
		{
			component.fieldOfView = CameraPerspectiveFieldCalculator();
		}
	}

	private float CameraOrthographicSizeCalculator()
	{
		return 1f / Camera.main.aspect * 6.39999962f;
	}

	private float CameraPerspectiveFieldCalculator()
	{
		float num = 1280f / Camera.main.aspect;
		float num2 = Mathf.Abs(Camera.main.transform.position.z);
		float num3 = 100f;
		float f = num / 2f / (num2 * num3);
		return (float)Math.Round((double)(Mathf.Atan(f) * 57.29578f * 2f - 0.1f), 1);
	}
}
