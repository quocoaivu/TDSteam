using System;
using UnityEngine;

public class WarlordW3StatueDirector : MonoBehaviour
{
	private void Start()
	{
		WarlordW3StatueDirector.instance = this;
	}

	public void BreakIce()
	{
		iceAnimator.Play("IceBreakAnim");
	}

	public static WarlordW3StatueDirector instance;

	public Animator iceAnimator;

	public GameObject bossStatue;
}
