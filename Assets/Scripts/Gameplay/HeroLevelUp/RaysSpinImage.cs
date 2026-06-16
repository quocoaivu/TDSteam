using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
	// Temporary DOTween replacement for the Spine "Rays" light-burst: a UI Image of the rays
	// sprite spinning continuously. Subclasses Image so a single component swap replaces the old
	// SkeletonGraphic without touching the GameObject's component list. Spins while enabled.
	public class RaysSpinImage : Image
	{
		// Degrees per second. Sign sets spin direction.
		[SerializeField]
		private float rotateSpeed = 40f;

		private Tween spinTween;

		protected override void OnEnable()
		{
			base.OnEnable();
			if (!Application.isPlaying)
			{
				return;
			}
			StartSpin();
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			KillSpin();
		}

		private void StartSpin()
		{
			KillSpin();
			if (Mathf.Approximately(rotateSpeed, 0f))
			{
				return;
			}
			float direction = rotateSpeed >= 0f ? 1f : -1f;
			float secondsPerTurn = 360f / Mathf.Abs(rotateSpeed);
			spinTween = rectTransform
				.DOLocalRotate(new Vector3(0f, 0f, direction * 360f), secondsPerTurn, RotateMode.LocalAxisAdd)
				.SetEase(Ease.Linear)
				.SetLoops(-1, LoopType.Incremental)
				.SetLink(gameObject);
		}

		private void KillSpin()
		{
			if (spinTween != null)
			{
				spinTween.Kill();
				spinTween = null;
			}
		}
	}
}
