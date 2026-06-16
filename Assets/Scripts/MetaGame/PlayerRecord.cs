using System;
using UnityEngine;

public class PlayerRecord : MonoBehaviour
{
	public static PlayerRecord Instance { get; set; }

	private void Awake()
	{
		if (PlayerRecord.Instance)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		PlayerRecord.Instance = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}
}
