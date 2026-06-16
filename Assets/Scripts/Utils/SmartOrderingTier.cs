using System;
using UnityEngine;

public class SmartOrderingTier : MonoBehaviour
{
    [SerializeField]
    private bool setOneTime;

    [SerializeField]
    private bool setRealTime;

    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private bool higherIsCover;

    [SerializeField]
    private bool lowerIsCover;

    [SerializeField]
    private bool flyObject;

    [Space]
    [Header("Set layer order depend on other")]
    [SerializeField]
    private SpriteRenderer targetGameObject;

    [SerializeField]
    private bool isDependOnOther;

    [SerializeField]
    private int offset;

    private void Awake()
	{
		spriteRenderer = base.GetComponent<SpriteRenderer>();
	}

	private void Start()
	{
		if (setOneTime)
		{
			SetSortingOrder();
		}
		if (isDependOnOther && setOneTime)
		{
			SetSortingOrderDepend();
		}
	}

	private void OnEnable()
	{
		if (setOneTime)
		{
			SetSortingOrder();
		}
		if (isDependOnOther && setOneTime)
		{
			SetSortingOrderDepend();
		}
	}

	private void Update()
	{
		if (spriteRenderer)
		{
			if (setRealTime)
			{
				SetSortingOrder();
			}
			if (isDependOnOther && setRealTime)
			{
				SetSortingOrderDepend();
			}
		}
	}

	private void SetSortingOrder()
	{
		if (lowerIsCover)
		{
			spriteRenderer.sortingOrder = -Mathf.RoundToInt(base.transform.position.y * 100f);
		}
		if (higherIsCover)
		{
			spriteRenderer.sortingOrder = Mathf.RoundToInt(base.transform.position.y * 100f);
		}
		if (flyObject)
		{
			SetSortingOrder(40);
		}
	}

	private void SetSortingOrderDepend()
	{
		spriteRenderer.sortingOrder = targetGameObject.sortingOrder + offset;
	}

	private void SetSortingOrder(int value)
	{
		spriteRenderer.sortingOrder = value;
	}

}
