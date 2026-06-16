using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
	// Temporary DOTween confetti burst that replaces the Spine "Phao hoa" effect on the
	// Hero Level Up popup. Plays automatically on enable (the popup does levelUpEffect
	// SetActive(true) on level-up). Confetti pieces are simple colored UI quads with no
	// sprite, so this has zero asset dependency.
	public class ConfettiTweenEffect : MonoBehaviour
	{
		[SerializeField]
		private int pieceCount = 24;

		[SerializeField]
		private Vector2 pieceSize = new Vector2(14f, 22f);

		[SerializeField]
		private float spreadX = 220f;

		[SerializeField]
		private float riseHeight = 160f;

		[SerializeField]
		private float fallDistance = 320f;

		[SerializeField]
		private float duration = 1.4f;

		[SerializeField]
		private Color[] colors = new Color[]
		{
			new Color(1f, 0.85f, 0.2f),
			new Color(0.95f, 0.3f, 0.3f),
			new Color(0.3f, 0.6f, 1f),
			new Color(0.4f, 0.85f, 0.4f),
			new Color(0.9f, 0.5f, 0.9f)
		};

		// Pieces are created once and reused on every replay (no Instantiate per level-up).
		private RectTransform[] pieces;

		private Image[] pieceImages;

		private Sequence sequence;

		private void OnEnable()
		{
			Play();
		}

		private void OnDisable()
		{
			KillSequence();
		}

		public void Play()
		{
			EnsurePieces();
			KillSequence();
			sequence = DOTween.Sequence();
			sequence.SetLink(gameObject);
			for (int i = 0; i < pieces.Length; i++)
			{
				AnimatePiece(i);
			}
		}

		private void EnsurePieces()
		{
			if (pieces != null)
			{
				return;
			}
			pieces = new RectTransform[pieceCount];
			pieceImages = new Image[pieceCount];
			for (int i = 0; i < pieceCount; i++)
			{
				GameObject go = new GameObject("Confetti", typeof(RectTransform), typeof(Image));
				RectTransform rt = go.GetComponent<RectTransform>();
				rt.SetParent(transform, false);
				rt.sizeDelta = pieceSize;
				Image img = go.GetComponent<Image>();
				img.raycastTarget = false;
				pieces[i] = rt;
				pieceImages[i] = img;
			}
		}

		// One arc per piece: rise then fall, horizontal drift, spin, fade out near the end.
		private void AnimatePiece(int i)
		{
			RectTransform rt = pieces[i];
			Image img = pieceImages[i];

			img.color = (colors != null && colors.Length > 0) ? colors[Random.Range(0, colors.Length)] : Color.white;
			SetAlpha(img, 1f);

			rt.anchoredPosition = Vector2.zero;
			rt.localScale = Vector3.one;
			rt.localEulerAngles = new Vector3(0f, 0f, Random.Range(0f, 360f));

			float xEnd = Random.Range(-spreadX, spreadX);
			float peakY = Random.Range(riseHeight * 0.5f, riseHeight);
			float endY = -Random.Range(fallDistance * 0.6f, fallDistance);
			float riseT = duration * 0.35f;
			float fallT = duration - riseT;

			sequence.Insert(0f, rt.DOAnchorPosY(peakY, riseT).SetEase(Ease.OutQuad));
			sequence.Insert(riseT, rt.DOAnchorPosY(endY, fallT).SetEase(Ease.InQuad));
			sequence.Insert(0f, rt.DOAnchorPosX(xEnd, duration).SetEase(Ease.OutQuad));
			sequence.Insert(0f, rt.DOLocalRotate(new Vector3(0f, 0f, Random.Range(-720f, 720f)), duration, RotateMode.LocalAxisAdd).SetEase(Ease.Linear));
			sequence.Insert(duration * 0.7f, DOFade(img, 0f, duration * 0.3f));
		}

		private Tween DOFade(Image img, float alpha, float fadeDuration)
		{
			return DOTween.To(() => img.color.a, a => SetAlpha(img, a), alpha, fadeDuration);
		}

		private void SetAlpha(Image img, float a)
		{
			Color c = img.color;
			c.a = a;
			img.color = c;
		}

		private void KillSequence()
		{
			if (sequence != null)
			{
				sequence.Kill();
				sequence = null;
			}
		}
	}
}
