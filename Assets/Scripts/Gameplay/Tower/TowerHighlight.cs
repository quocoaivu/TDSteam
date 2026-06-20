using System.Collections.Generic;
using DG.Tweening;
using MetaGame;
using UnityEngine;

namespace Gameplay
{
	// Pulses a tower's sprites between their normal color and a highlight color, used to point out which
	// towers a shop item fits. Added to a tower at runtime (no per-prefab wiring), like TowerEquipment.
	// Static helpers highlight every active tower of a given class at once and clear them again; the shop
	// item button drives them on hover. SetUpdate(true) so the pulse animates while the shop pauses the game.
	public class TowerHighlight : MonoBehaviour
	{
		// Highlights all active towers whose Id matches towerID. Clears any previous highlight first.
		public static void HighlightClass(int towerID)
		{
			ClearAll();
			List<TurretEntity> towers = MonoSingleton<GameRecord>.Instance.ListActiveTower;
			for (int i = 0; i < towers.Count; i++)
			{
				TurretEntity tower = towers[i];
				if (tower == null || tower.Id != towerID)
				{
					continue;
				}
				TowerHighlight highlight = tower.GetComponent<TowerHighlight>();
				if (highlight == null)
				{
					highlight = tower.gameObject.AddComponent<TowerHighlight>();
				}
				highlight.Show();
				activeHighlights.Add(highlight);
			}
		}

		// Stops and restores every currently highlighted tower.
		public static void ClearAll()
		{
			for (int i = 0; i < activeHighlights.Count; i++)
			{
				if (activeHighlights[i] != null)
				{
					activeHighlights[i].Hide();
				}
			}
			activeHighlights.Clear();
		}

		// Starts the color pulse. Re-reads the current colors as the originals each time (so it never restores
		// to a stale tint), then loops a tween that lerps toward the highlight color and back.
		public void Show()
		{
			if (renderers == null)
			{
				renderers = GetComponentsInChildren<SpriteRenderer>(true);
			}
			if (isHighlighting)
			{
				return;
			}
			if (originalColors == null || originalColors.Length != renderers.Length)
			{
				originalColors = new Color[renderers.Length];
			}
			for (int i = 0; i < renderers.Length; i++)
			{
				originalColors[i] = renderers[i] != null ? renderers[i].color : Color.white;
			}
			isHighlighting = true;
			tween = DOVirtual.Float(0f, 1f, pulseDuration, ApplyTint)
				.SetLoops(-1, LoopType.Yoyo)
				.SetEase(Ease.InOutSine)
				.SetUpdate(true);
		}

		// Stops the pulse and restores the original colors.
		public void Hide()
		{
			if (!isHighlighting)
			{
				return;
			}
			isHighlighting = false;
			if (tween != null)
			{
				tween.Kill();
				tween = null;
			}
			RestoreColors();
		}

		private void ApplyTint(float t)
		{
			for (int i = 0; i < renderers.Length; i++)
			{
				if (renderers[i] != null)
				{
					renderers[i].color = Color.Lerp(originalColors[i], highlightColor, t);
				}
			}
		}

		private void RestoreColors()
		{
			if (renderers == null || originalColors == null)
			{
				return;
			}
			for (int i = 0; i < renderers.Length; i++)
			{
				if (renderers[i] != null)
				{
					renderers[i].color = originalColors[i];
				}
			}
		}

		// Pooled tower despawn disables the GameObject: restore colors so a reused tower never keeps the tint.
		private void OnDisable()
		{
			Hide();
			activeHighlights.Remove(this);
		}

		[SerializeField]
		private Color highlightColor = new Color(1f, 0.95f, 0.4f, 1f);

		[SerializeField]
		private float pulseDuration = 0.5f;

		private SpriteRenderer[] renderers;

		private Color[] originalColors;

		private Tween tween;

		private bool isHighlighting;

		private static readonly List<TowerHighlight> activeHighlights = new List<TowerHighlight>();

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			activeHighlights.Clear();
		}
	}
}
