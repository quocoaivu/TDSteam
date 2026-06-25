using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Items
{
	// Hover tooltip that shows a TowerItem's details (name, stat bonus, rarity) next to the hovered offer.
	// Self-building at runtime so no scene wiring is needed: the first Show() lazily creates the tooltip UI
	// under the hovered offer's root Canvas. Call ItemTooltip.Show(item, offerRect) on pointer enter and
	// ItemTooltip.Hide() on pointer exit. Fades with DOTween + SetUpdate(true) so it animates while the shop
	// pauses the game (matches ItemShopOfferButton's hover scale).
	public class ItemTooltip : MonoBehaviour
	{
		// Fixed size so content changes can't trigger a layout rebuild (UI rule: no Layout Groups on dynamic UI).
		// Height is tall enough for 3 stat lines (3 × ~24px) plus name and rarity rows.
		private static readonly Vector2 PanelSize = new Vector2(260f, 160f);

		// Gap between the hovered offer and the tooltip edge.
		private const float EDGE_GAP = 12f;

		private const float FADE_DURATION = 0.12f;

		// Shows the tooltip for this item, placed beside the given offer. Builds the UI on first use.
		public static void Show(TowerItem item, RectTransform anchor)
		{
			if (item == null || anchor == null)
			{
				return;
			}
			// The offer's NEAREST canvas (may be a nested canvas with its own sorting order), not the root: the
			// shop can live on a higher-sorted canvas, so a tooltip on the root canvas would draw behind it.
			Canvas canvas = anchor.GetComponentInParent<Canvas>();
			if (canvas == null)
			{
				return;
			}
			if (instance == null || instance.canvas != canvas)
			{
				Build(canvas);
			}
			// Parent directly under that canvas (not the shop window) so the tooltip keeps the shop's exact sort
			// layer yet bypasses any Mask/RectMask2D on the window/content that would clip a tooltip overflowing
			// the window. As the last sibling it draws on top of everything in that canvas.
			instance.rect.SetParent(canvas.transform, false);
			instance.rect.SetAsLastSibling();
			instance.Populate(item);
			instance.PositionBeside(anchor);
			instance.FadeIn();
		}

		// Hides the tooltip if it exists.
		public static void Hide()
		{
			if (instance != null)
			{
				instance.FadeOut();
			}
		}

		private void Populate(TowerItem item)
		{
			Color color = RarityColor(item.rarity);
			nameText.SetText(item.name);
			nameText.color = color;
			SetStatLine(item);
			rarityText.SetText(RarityName(item.rarity));
			rarityText.color = color;
		}

		// Places the tooltip beside the offer, working in the canvas's local space (anchoredPosition) so it is
		// correct for any render mode (this canvas is Screen Space - Camera, where world units are tiny and a
		// pixel-scale world offset would throw the tooltip off-screen). Flips to the offer's left if it would
		// overflow the canvas right edge.
		private void PositionBeside(RectTransform anchor)
		{
			RectTransform canvasRect = canvas.transform as RectTransform;
			Camera cam = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;
			Vector3[] corners = new Vector3[4];
			anchor.GetWorldCorners(corners); // 0=BL 1=TL 2=TR 3=BR

			// Anchor to the canvas centre so anchoredPosition is measured in canvas reference pixels (same unit
			// as EDGE_GAP / PanelSize), regardless of where the tooltip sits in the hierarchy.
			rect.anchorMin = new Vector2(0.5f, 0.5f);
			rect.anchorMax = new Vector2(0.5f, 0.5f);

			// Default: to the right of the offer's top-right corner (tooltip grows right/down).
			Vector2 rightOf = ToCanvasLocal(canvasRect, cam, corners[2]);
			rect.pivot = new Vector2(0f, 1f);
			rect.anchoredPosition = rightOf + new Vector2(EDGE_GAP, 0f);

			// Flip to the offer's left side if the tooltip's right edge runs past the canvas.
			float halfWidth = canvasRect.rect.width * 0.5f;
			if (rect.anchoredPosition.x + PanelSize.x > halfWidth)
			{
				Vector2 leftOf = ToCanvasLocal(canvasRect, cam, corners[1]);
				rect.pivot = new Vector2(1f, 1f);
				rect.anchoredPosition = leftOf + new Vector2(-EDGE_GAP, 0f);
			}
		}

		// Converts a world point to a position in the canvas's local space (relative to its centre pivot).
		private static Vector2 ToCanvasLocal(RectTransform canvasRect, Camera cam, Vector3 worldPoint)
		{
			Vector2 screen = RectTransformUtility.WorldToScreenPoint(cam, worldPoint);
			Vector2 local;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screen, cam, out local);
			return local;
		}

		private void FadeIn()
		{
			gameObject.SetActive(true);
			group.DOKill();
			group.alpha = 0f;
			group.DOFade(1f, FADE_DURATION).SetUpdate(true);
		}

		private void FadeOut()
		{
			group.DOKill();
			group.DOFade(0f, FADE_DURATION).SetUpdate(true)
				.OnComplete(() => gameObject.SetActive(false));
		}

		private void SetStatLine(TowerItem item)
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			for (int i = 0; i < item.statTypes.Length; i++)
			{
				if (i > 0)
				{
					sb.Append('\n');
				}
				switch (item.statTypes[i])
				{
				case StatType.AttackSpeed:
					sb.AppendFormat("+{0}% Attack Speed", item.statValues[i]);
					break;
				case StatType.Crit:
					sb.AppendFormat("+{0}% Crit", item.statValues[i]);
					break;
				default:
					sb.AppendFormat("+{0} Damage", item.statValues[i]);
					break;
				}
			}
			statText.SetText(sb.ToString());
		}

		private static string RarityName(int rarity)
		{
			switch (rarity)
			{
			case 1: return "Uncommon";
			case 2: return "Rare";
			case 3: return "Epic";
			case 4: return "Legendary";
			default: return "Common";
			}
		}

		private static Color RarityColor(int rarity)
		{
			switch (rarity)
			{
			case 1: return new Color(0.45f, 0.85f, 0.40f); // green
			case 2: return new Color(0.35f, 0.60f, 0.95f); // blue
			case 3: return new Color(0.70f, 0.45f, 0.90f); // purple
			case 4: return new Color(0.95f, 0.65f, 0.25f); // orange
			default: return new Color(0.85f, 0.85f, 0.85f); // gray
			}
		}

		// Builds the tooltip GameObject + its texts once, parented to the shop's root canvas.
		private static void Build(Canvas canvas)
		{
			GameObject go = new GameObject("ItemTooltip", typeof(RectTransform));
			go.transform.SetParent(canvas.transform, false);

			instance = go.AddComponent<ItemTooltip>();
			instance.canvas = canvas;
			instance.rect = go.GetComponent<RectTransform>();
			instance.rect.sizeDelta = PanelSize;

			Image bg = go.AddComponent<Image>();
			bg.color = new Color(0.08f, 0.09f, 0.12f, 0.96f);
			bg.raycastTarget = false; // tooltip must never eat clicks meant for the shop/map

			instance.group = go.AddComponent<CanvasGroup>();
			instance.group.interactable = false;
			instance.group.blocksRaycasts = false;

			instance.nameText = NewText(go.transform, "Name", 22f, TextAlignmentOptions.TopLeft,
				new Vector2(0f, -10f), 30f);
			instance.statText = NewText(go.transform, "Stat", 18f, TextAlignmentOptions.TopLeft,
				new Vector2(0f, -48f), 72f);
			instance.statText.color = new Color(0.95f, 0.92f, 0.70f);
			instance.statText.overflowMode = TMPro.TextOverflowModes.Overflow;
			instance.rarityText = NewText(go.transform, "Rarity", 16f, TextAlignmentOptions.BottomLeft,
				new Vector2(0f, 10f), 24f);

			go.SetActive(false);
		}

		// Creates one TMP line stretched across the panel width with fixed height, offset from the top (or
		// bottom for bottom-aligned lines). PADDING insets the text from the panel edges.
		private static TMP_Text NewText(Transform parent, string name, float size, TextAlignmentOptions align,
			Vector2 anchoredPos, float height)
		{
			const float PADDING = 14f;
			GameObject go = new GameObject(name, typeof(RectTransform));
			go.transform.SetParent(parent, false);
			RectTransform r = go.GetComponent<RectTransform>();
			bool bottom = align == TextAlignmentOptions.BottomLeft;
			r.anchorMin = new Vector2(0f, bottom ? 0f : 1f);
			r.anchorMax = new Vector2(1f, bottom ? 0f : 1f);
			r.pivot = new Vector2(0f, bottom ? 0f : 1f);
			r.sizeDelta = new Vector2(-PADDING * 2f, height);
			r.anchoredPosition = new Vector2(PADDING, anchoredPos.y);

			TextMeshProUGUI tmp = go.AddComponent<TextMeshProUGUI>();
			tmp.fontSize = size;
			tmp.alignment = align;
			tmp.raycastTarget = false;
			tmp.enableWordWrapping = false;
			tmp.overflowMode = TextOverflowModes.Ellipsis;
			if (TMP_Settings.defaultFontAsset != null)
			{
				tmp.font = TMP_Settings.defaultFontAsset;
			}
			return tmp;
		}

		private Canvas canvas;

		private RectTransform rect;

		private CanvasGroup group;

		private TMP_Text nameText;

		private TMP_Text statText;

		private TMP_Text rarityText;

		// Rebuilt if the canvas changes (e.g. scene reload); otherwise reused across hovers.
		private static ItemTooltip instance;
	}
}
