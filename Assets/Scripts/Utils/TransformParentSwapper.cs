using System;
using System.Collections;
using UnityEngine;

public class TransformParentSwapper : MonoBehaviour
{
    [SerializeField]
    private Transform temporaryParent;

    private Transform originalParent;

    private int originalSiblingIndex;


    [ContextMenu("SwitchToTemporaryParent")]
	public void SwitchToTemporaryParent()
	{
		if (base.transform.parent == temporaryParent)
		{
			return;
		}
		originalParent = base.transform.parent;
		originalSiblingIndex = base.transform.GetSiblingIndex();
		base.transform.SetParent(temporaryParent);
	}

	public void SwitchToTemporaryParent(int delayTimeMilisecond)
	{
		base.StartCoroutine(iSwitchToTemporaryParent((float)delayTimeMilisecond / 1000f));
	}

	private IEnumerator iSwitchToTemporaryParent(float delayTime)
	{
		yield return new WaitForSeconds(delayTime);
		if (base.transform.parent == temporaryParent)
		{
		}
		originalParent = base.transform.parent;
		originalSiblingIndex = base.transform.GetSiblingIndex();
		base.transform.SetParent(temporaryParent);
		yield break;
	}

	[ContextMenu("SwitchBackToOriginalParent")]
	public void SwitchBackToOriginalParent()
	{
		if (originalParent)
		{
			base.transform.SetParent(originalParent);
			base.transform.SetSiblingIndex(originalSiblingIndex);
			base.transform.localPosition = Vector3.zero;
		}
	}
}
