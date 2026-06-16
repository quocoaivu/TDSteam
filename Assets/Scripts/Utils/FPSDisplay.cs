using System;
using UnityEngine;
using UnityEngine.UI;

public class FPSDisplay : MonoBehaviour
{
    [SerializeField]
    private Text currentFPS;

    private int _fpsAccumulator;

    private float _fpsNextPeriod;

    private int _currentFps;

    private float lastUpdateTime;


    private void Awake()
	{
		lastUpdateTime = Time.realtimeSinceStartup;
		_fpsNextPeriod = lastUpdateTime + 0.1f;
	}

	private void Update()
	{
		_fpsAccumulator++;
		if (Time.realtimeSinceStartup > _fpsNextPeriod)
		{
			_currentFps = (int)((float)_fpsAccumulator / (Time.realtimeSinceStartup - lastUpdateTime));
			_fpsAccumulator = 0;
			currentFPS.text = _currentFps.ToFixedString();
			lastUpdateTime = Time.realtimeSinceStartup;
			_fpsNextPeriod = lastUpdateTime + 0.1f;
		}
	}
}
