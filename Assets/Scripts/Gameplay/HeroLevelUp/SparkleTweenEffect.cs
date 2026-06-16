using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
	// Temporary DOTween replacement for the Spine "Sparks_2" twinkle: small diamond quads that
	// scale + fade in and out at random spots inside this object's RectTransform, looping forever.
	// Plays on enable (matches the auto-playing SkeletonGraphic it replaces). No asset dependency.
	public class SparkleTweenEffect : MonoBehaviour
	{
		[SerializeField]
		private int pieceCount = 10;

		[SerializeField]
		private Vector2 pieceSize = new Vector2(10f, 10f);

		// Used only if this object's RectTransform has no real size (half-extent of the spawn box).
		[SerializeField]
		private Vector2 fallbackSpread = new Vector2(80f, 80f);

		[SerializeField]
		private float minTwinkle = 0.4f;

		[SerializeField]
		private float maxTwinkle = 0.9f;

		[SerializeField]
		private Color color = new Color(1f, 0.97f, 0.7f);

		// Created once, reused on every enable (no Instantiate per show).
		private RectTransform[] pieces;

		private Image[] images;

		private Tween[] tweens;

		private void OnEnable()
		{
			Play();
		}

		private void OnDisable()
		{
			KillAll();
		}

		public void Play()
		{
			EnsurePieces();
			for (int i = 0; i < pieces.Length; i++)
			{
				AnimatePiece(i);
			}
		}

		// Half-extent of the spawn box, from this RectTransform's rect (falls back if unsized).
		private Vector2 HalfArea()
		{
			Rect r = ((RectTransform)transform).rect;
			float x = r.width > 1f ? r.width * 0.5f : fallbackSpread.x;
			float y = r.height > 1f ? r.height * 0.5f : fallbackSpread.y;
			return new Vector2(x, y);
		}

		private void EnsurePieces()
		{
			if (pieces != null)
			{
				return;
			}
			pieces = new RectTransform[pieceCount];
			images = new Image[pieceCount];
			tweens = new Tween[pieceCount];
			for (int i = 0; i < pieceCount; i++)
			{
				GameObject go = new GameObject("Sparkle", typeof(RectTransform), typeof(Image));
				RectTransform rt = go.GetComponent<RectTransform>();
				rt.SetParent(transform, false);
				rt.sizeDelta = pieceSize;
				rt.localEulerAngles = new Vector3(0f, 0f, 45f); // diamond
				Image img = go.GetComponent<Image>();
				img.raycastTarget = false;
				img.color = color;
				pieces[i] = rt;
				images[i] = img;
			}
		}

		private void AnimatePiece(int i)
		{
			RectTransform rt = pieces[i];
			Image img = images[i];
			if (tweens[i] != null)
			{
				tweens[i].Kill();
			}

			Reposition(rt);
			rt.localScale = Vector3.zero;
			SetAlpha(img, 0f);

			float half = Random.Range(minTwinkle, maxTwinkle) * 0.5f;
			float pause = Random.Range(0.1f, 0.5f);

			Sequence seq = DOTween.Sequence();
			seq.SetLink(gameObject);
			seq.Append(rt.DOScale(1f, half).SetEase(Ease.OutBack));
			seq.Join(DOFade(img, 1f, half));
			seq.Append(rt.DOScale(0f, half).SetEase(Ease.InQuad));
			seq.Join(DOFade(img, 0f, half));
			seq.AppendInterval(pause);
			seq.SetDelay(Random.Range(0f, maxTwinkle));
			seq.SetLoops(-1, LoopType.Restart);
			seq.OnStepComplete(() => Reposition(rt));
			tweens[i] = seq;
		}

		private void Reposition(RectTransform rt)
		{
			Vector2 a = HalfArea();
			rt.anchoredPosition = new Vector2(Random.Range(-a.x, a.x), Random.Range(-a.y, a.y));
		}

		private Tween DOFade(Image img, float alpha, float duration)
		{
			return DOTween.To(() => img.color.a, value => SetAlpha(img, value), alpha, duration);
		}

		private void SetAlpha(Image img, float alpha)
		{
			Color c = img.color;
			c.a = alpha;
			img.color = c;
		}

		private void KillAll()
		{
			if (tweens == null)
			{
				return;
			}
			for (int i = 0; i < tweens.Length; i++)
			{
				if (tweens[i] != null)
				{
					tweens[i].Kill();
					tweens[i] = null;
				}
			}
		}
	}
}
