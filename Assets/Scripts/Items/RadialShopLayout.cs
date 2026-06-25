using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Items
{
	// Lays out and animates its child slots in a radial arc (a "tree" that blooms items from its center).
	// Pure presentation: it positions/scales/fades the slots and knows nothing about prices or buying.
	// The slots are the existing ItemShopOfferButton objects; ItemShopPanel still drives all shop logic.
	// Sits on the ItemContainer (the slots' parent); collects every child RectTransform as a slot at Awake.
	// All tweens use SetUpdate(true) so they keep running even if another popup pauses the game (timeScale 0).
	public class RadialShopLayout : MonoBehaviour
	{
		// Default arc is the UPPER arc (items bloom above the tree center). The spec's literal numbers
		// (-160..-20) produce a LOWER arc because sin is negative there; flip these two in the Inspector
		// if you want the items to fan out downward instead.
		[SerializeField]
		private float radius = 110f;

		[SerializeField]
		private float startAngle = 20f;

		[SerializeField]
		private float endAngle = 160f;

		[SerializeField]
		private float staggerDelay = 0.035f;

		[Header("Open tween")]
		[SerializeField]
		private float openMoveDuration = 0.25f;

		[SerializeField]
		private float openScaleDuration = 0.2f;

		[SerializeField]
		private float openFadeDuration = 0.15f;

		[Header("Close tween")]
		[SerializeField]
		private float closeMoveDuration = 0.15f;

		[SerializeField]
		private float closeScaleDuration = 0.12f;

		[SerializeField]
		private float closeFadeDuration = 0.1f;

		private readonly List<RectTransform> slots = new List<RectTransform>();

		private readonly List<CanvasGroup> groups = new List<CanvasGroup>();

		private void Awake()
		{
			Collect();
			// Start collapsed: the tree is visible on the HUD, the items are hidden until the tree is tapped.
			CloseInstant();
		}

		// Snaps every slot to the hidden state with no animation (used for the initial closed shop).
		private void CloseInstant()
		{
			for (int i = 0; i < slots.Count; i++)
			{
				slots[i].anchoredPosition = Vector2.zero;
				slots[i].localScale = Vector3.zero;
				groups[i].alpha = 0f;
				groups[i].blocksRaycasts = false;
				groups[i].interactable = false;
			}
		}

		// Caches each child slot and its CanvasGroup (adds one if missing) so the tweens have no per-frame
		// GetComponent cost.
		private void Collect()
		{
			slots.Clear();
			groups.Clear();
			foreach (Transform child in transform)
			{
				RectTransform rect = child as RectTransform;
				if (rect == null)
				{
					continue;
				}
				CanvasGroup group = child.GetComponent<CanvasGroup>();
				if (group == null)
				{
					group = child.gameObject.AddComponent<CanvasGroup>();
				}
				slots.Add(rect);
				groups.Add(group);
			}
		}

		// Radial position for slot i of count, spread evenly from startAngle to endAngle.
		public Vector2 SlotPosition(int i, int count)
		{
			float t = (count <= 1) ? 0f : (float)i / (count - 1);
			float angle = Mathf.Lerp(startAngle, endAngle, t) * Mathf.Deg2Rad;
			return new Vector2(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius);
		}

		// Bloom open: every slot starts at the center (scale 0, invisible) then flies to its radial spot
		// with a staggered OutBack, scaling up and fading in.
		public void PlayOpen()
		{
			KillTweens();
			int count = slots.Count;
			for (int i = 0; i < count; i++)
			{
				RectTransform rect = slots[i];
				CanvasGroup group = groups[i];
				rect.anchoredPosition = Vector2.zero;
				rect.localScale = Vector3.zero;
				group.alpha = 0f;
				group.blocksRaycasts = true;
				group.interactable = true;
				float delay = i * staggerDelay;
				rect.DOAnchorPos(SlotPosition(i, count), openMoveDuration).SetEase(Ease.OutBack).SetDelay(delay).SetUpdate(true);
				rect.DOScale(1f, openScaleDuration).SetEase(Ease.OutBack).SetDelay(delay).SetUpdate(true);
				group.DOFade(1f, openFadeDuration).SetDelay(delay).SetUpdate(true);
			}
		}

		// Collapse close: all slots fly back to the center at once (no stagger) with an InBack, shrinking
		// and fading out. onComplete (optional) runs after the last tween.
		public void PlayClose(TweenCallback onComplete)
		{
			KillTweens();
			Sequence sequence = DOTween.Sequence().SetUpdate(true);
			for (int i = 0; i < slots.Count; i++)
			{
				RectTransform rect = slots[i];
				CanvasGroup group = groups[i];
				// Stop catching clicks immediately as they collapse, so a tap during close doesn't pick up.
				group.blocksRaycasts = false;
				group.interactable = false;
				sequence.Join(rect.DOAnchorPos(Vector2.zero, closeMoveDuration).SetEase(Ease.InBack));
				sequence.Join(rect.DOScale(0f, closeScaleDuration).SetEase(Ease.InBack));
				sequence.Join(group.DOFade(0f, closeFadeDuration));
			}
			if (onComplete != null)
			{
				sequence.OnComplete(onComplete);
			}
		}

		private void KillTweens()
		{
			for (int i = 0; i < slots.Count; i++)
			{
				slots[i].DOKill();
				groups[i].DOKill();
			}
		}
	}
}
