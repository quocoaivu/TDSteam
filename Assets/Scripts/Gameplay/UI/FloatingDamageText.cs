using DG.Tweening;
using GameCore;
using TMPro;
using UnityEngine;

namespace Gameplay
{
	// Floating damage number. Pooled (see FloatingDamageTextPool), TMP world-space text,
	// DOTween pop + rise + fade. STATIC: it captures the hit position and animates from there
	// independently, so it survives the enemy dying / returning to pool. No Instantiate per hit.
	public class FloatingDamageText : BaseMonoBehaviour
	{
		[SerializeField]
		private TMP_Text text;

		[Space]
		[Header("Motion")]
		[SerializeField]
		private Vector3 offset = new Vector3(0f, 0.4f, 0f);

		[SerializeField]
		private float randomizeIntensity = 0.15f;

		[SerializeField]
		private float floatUpDistance = 0.4f;

		[SerializeField]
		private float lifeTime = 0.7f;

		[SerializeField]
		private float startScale = 1.2f;

		// The prefab's authored scale (small in world units for a 2D game). Captured once so the
		// pop animation scales RELATIVE to it instead of forcing 1.0 and blowing the text up.
		private Vector3 baseScale = Vector3.one;

		private bool baseScaleCaptured;

		private Tween tween;

		public void Setup(int amount, Vector3 worldPosition)
		{
			if (!baseScaleCaptured)
			{
				baseScale = transform.localScale;
				baseScaleCaptured = true;
			}
			KillTween();
			text.SetText("{0}", amount);
			// Pooled TMP objects don't always regenerate their mesh after SetActive(true)+SetText
			// in the same frame, leaving an empty (zero-bounds) mesh = invisible. Force it now.
			text.ForceMeshUpdate();

			Vector3 jitter = new Vector3(Random.Range(-randomizeIntensity, randomizeIntensity), Random.Range(-randomizeIntensity, randomizeIntensity), 0f);
			Vector3 startPos = worldPosition + offset + jitter;
			transform.position = startPos;
			transform.localScale = baseScale * startScale;
			SetAlpha(1f);

			tween = BuildAnimation(startPos);
		}

		// Bouncy pop + rise + fade out, then return to pool.
		private Sequence BuildAnimation(Vector3 startPos)
		{
			Sequence seq = DOTween.Sequence();
			// Auto-kill if this object is destroyed (scene change / domain reload) so the tween
			// never touches a destroyed RectTransform.
			seq.SetLink(gameObject);
			seq.Append(transform.DOScale(baseScale, lifeTime * 0.3f).SetEase(Ease.OutBack));
			seq.Join(transform.DOMoveY(startPos.y + floatUpDistance, lifeTime).SetEase(Ease.OutCubic));
			seq.Insert(lifeTime * 0.6f, DOFadeText(0f, lifeTime * 0.4f));
			seq.OnComplete(ReturnToPool);
			return seq;
		}

		private Tween DOFadeText(float alpha, float duration)
		{
			return DOTween.To(GetAlpha, SetAlpha, alpha, duration);
		}

		private float GetAlpha()
		{
			return text.alpha;
		}

		private void SetAlpha(float alpha)
		{
			text.alpha = alpha;
		}

		private void ReturnToPool()
		{
			FloatingDamageTextPool.Instance.Despawn(this);
		}

		private void KillTween()
		{
			if (tween != null)
			{
				tween.Kill();
				tween = null;
			}
		}

		// Called by the pool right before the object is parked. Stop any running tween.
		public void OnReturnPool()
		{
			KillTween();
		}
	}
}
