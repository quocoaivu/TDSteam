using System.Collections.Generic;
using Items;
using TMPro;
using UnityEditor;
using UnityEditor.Events;
using UnityEngine;
using UnityEngine.UI;

namespace ItemsEditor
{
	// One-click builder for the in-match item shop UI. Select the GameObject that has an ItemShopPanel
	// component, then run Tools > Tower Item > Build Item Shop. It creates 3 offer buttons + a reroll
	// button + a gold label, wires every reference (and the reroll button's OnClick), and registers them
	// in the panel. Fully undoable. Mirrors ItemSlotBuilder. You still wire a gameplay button's OnClick
	// to ItemShopPanel.OpenShop() by hand to open the shop.
	public static class ItemShopBuilder
	{
		private const int OFFER_COUNT = 3;

		// Fixed window size (canvas reference units) so the shop is a contained popup, not full-screen.
		private static readonly Vector2 WindowSize = new Vector2(700f, 520f);

		private static readonly Vector2 OfferSize = new Vector2(180f, 220f);

		// Anchored x for the three offers (centered row).
		private static readonly float[] OfferX = { -200f, 0f, 200f };

		[MenuItem("Tools/Tower Item/Build Item Shop")]
		private static void BuildItemShop()
		{
			GameObject selected = Selection.activeGameObject;
			if (selected == null)
			{
				EditorUtility.DisplayDialog("Item Shop Builder",
					"Chọn GameObject có component ItemShopPanel rồi chạy lại.", "OK");
				return;
			}
			ItemShopPanel panel = selected.GetComponent<ItemShopPanel>();
			if (panel == null)
			{
				EditorUtility.DisplayDialog("Item Shop Builder",
					"GameObject đang chọn không có ItemShopPanel. Hãy chọn đúng panel.", "OK");
				return;
			}

			Transform existing = selected.transform.Find("ShopContent");
			if (existing != null)
			{
				EditorUtility.DisplayDialog("Item Shop Builder",
					"Đã tồn tại 'ShopContent'. Xóa nó trước nếu muốn dựng lại.", "OK");
				return;
			}

			// The panel is a UI dialog (GameplayDialogHandler) so its root must be a RectTransform under a
			// Canvas. A plain GameObject (only Transform) means it wasn't created as a UI element.
			RectTransform rootRect = selected.GetComponent<RectTransform>();
			if (rootRect == null)
			{
				EditorUtility.DisplayDialog("Item Shop Builder",
					"GameObject này không phải UI object (thiếu RectTransform). Hãy đặt ItemShopPanel làm con của một Canvas " +
					"(chuột phải Canvas > UI > ... hoặc Create Empty rồi Add Component > Rect Transform) rồi chạy lại.", "OK");
				return;
			}

			// Normalize the panel root so it exactly covers the canvas; otherwise a mis-sized root (or a
			// non-1 localScale) makes the whole shop look huge relative to the map.
			StretchFull(rootRect);
			rootRect.localScale = Vector3.one;

			GameObject container = NewUIObject("ShopContent", selected.transform);
			Undo.RegisterCreatedObjectUndo(container, "Build Item Shop");
			StretchFull(container.GetComponent<RectTransform>());

			// Dim full-screen backdrop (also blocks clicks on the map behind the shop).
			GameObject backdrop = NewUIObject("Backdrop", container.transform);
			StretchFull(backdrop.GetComponent<RectTransform>());
			Image backdropImg = backdrop.AddComponent<Image>();
			backdropImg.color = new Color(0f, 0f, 0f, 0.6f);

			// Fixed-size centered window that holds everything visible.
			GameObject window = NewUIObject("Window", container.transform);
			RectTransform windowRect = window.GetComponent<RectTransform>();
			windowRect.anchorMin = new Vector2(0.5f, 0.5f);
			windowRect.anchorMax = new Vector2(0.5f, 0.5f);
			windowRect.pivot = new Vector2(0.5f, 0.5f);
			windowRect.sizeDelta = WindowSize;
			windowRect.anchoredPosition = Vector2.zero;
			Image windowImg = window.AddComponent<Image>();
			windowImg.color = new Color(0.12f, 0.13f, 0.17f, 1f);

			List<ItemShopOfferButton> offers = new List<ItemShopOfferButton>();
			for (int i = 0; i < OFFER_COUNT; i++)
			{
				offers.Add(BuildOffer(i, window.transform));
			}

			TMP_Text goldText = NewText(window.transform, "Gold", "0", 24f, TextAlignmentOptions.Right);
			RectTransform goldRect = goldText.rectTransform;
			goldRect.anchoredPosition = new Vector2(0f, 200f);
			goldRect.sizeDelta = new Vector2(300f, 40f);

			Button rerollButton;
			TMP_Text rerollCostText;
			BuildReroll(window.transform, panel, out rerollButton, out rerollCostText);

			BuildClose(window.transform, panel);

			WirePanel(panel, offers, rerollButton, rerollCostText, goldText, window);

			// Start hidden: the panel's full-screen backdrop would otherwise eat all clicks on the HUD.
			// OpenShop() reactivates it on demand.
			selected.SetActive(false);

			EditorUtility.SetDirty(panel);
			EditorSceneMarkDirty();
			Debug.Log("ItemShopBuilder: dựng xong 3 offer + reroll + close + gold, wire vào panel. Panel đã được TẮT " +
				"(inactive) để không chặn click HUD. Nhớ Save scene. " +
				"Còn lại THỦ CÔNG: 1 nút gameplay trên HUD OnClick -> ItemShopPanel.OpenShop().");
		}

		private static ItemShopOfferButton BuildOffer(int index, Transform parent)
		{
			GameObject offerGo = NewUIObject("Offer_" + index, parent);
			Undo.RegisterCreatedObjectUndo(offerGo, "Build Item Shop");
			RectTransform rect = offerGo.GetComponent<RectTransform>();
			rect.sizeDelta = OfferSize;
			rect.anchoredPosition = new Vector2(OfferX[index], 0f);

			Image bg = offerGo.AddComponent<Image>();
			bg.color = new Color(0.20f, 0.22f, 0.28f, 1f);
			Button button = offerGo.AddComponent<Button>();
			button.targetGraphic = bg;

			// Item name (runtime fills it; "Sold" once bought).
			TMP_Text nameText = NewText(offerGo.transform, "Name", "—", 18f, TextAlignmentOptions.Top);
			StretchFull(nameText.rectTransform);

			// Item icon (runtime loads the sprite from the spec's icon key; hidden when sold).
			GameObject iconGo = NewUIObject("Icon", offerGo.transform);
			RectTransform iconRect = iconGo.GetComponent<RectTransform>();
			iconRect.anchorMin = new Vector2(0.5f, 0.5f);
			iconRect.anchorMax = new Vector2(0.5f, 0.5f);
			iconRect.pivot = new Vector2(0.5f, 0.5f);
			iconRect.sizeDelta = new Vector2(120f, 120f);
			iconRect.anchoredPosition = Vector2.zero;
			Image iconImage = iconGo.AddComponent<Image>();
			iconImage.raycastTarget = false;
			iconImage.preserveAspect = true;

			// Gold cost (hidden once sold).
			TMP_Text costText = NewText(offerGo.transform, "Cost", "0", 22f, TextAlignmentOptions.Bottom);
			StretchFull(costText.rectTransform);

			// Sold overlay: dim cover shown after purchase.
			GameObject sold = NewUIObject("Sold", offerGo.transform);
			StretchFull(sold.GetComponent<RectTransform>());
			Image soldImg = sold.AddComponent<Image>();
			soldImg.color = new Color(0f, 0f, 0f, 0.6f);
			soldImg.raycastTarget = false;
			sold.SetActive(false);

			ItemShopOfferButton offer = offerGo.AddComponent<ItemShopOfferButton>();
			SerializedObject so = new SerializedObject(offer);
			so.FindProperty("button").objectReferenceValue = button;
			so.FindProperty("nameText").objectReferenceValue = nameText;
			so.FindProperty("costText").objectReferenceValue = costText;
			so.FindProperty("iconImage").objectReferenceValue = iconImage;
			so.FindProperty("soldOverlay").objectReferenceValue = sold;
			so.ApplyModifiedPropertiesWithoutUndo();

			return offer;
		}

		private static void BuildReroll(Transform parent, ItemShopPanel panel, out Button button, out TMP_Text costText)
		{
			GameObject rerollGo = NewUIObject("RerollButton", parent);
			Undo.RegisterCreatedObjectUndo(rerollGo, "Build Item Shop");
			RectTransform rect = rerollGo.GetComponent<RectTransform>();
			rect.sizeDelta = new Vector2(220f, 60f);
			rect.anchoredPosition = new Vector2(0f, -200f);

			Image bg = rerollGo.AddComponent<Image>();
			bg.color = new Color(0.30f, 0.26f, 0.18f, 1f);
			button = rerollGo.AddComponent<Button>();
			button.targetGraphic = bg;

			TMP_Text label = NewText(rerollGo.transform, "Label", "Reroll", 20f, TextAlignmentOptions.Left);
			StretchFull(label.rectTransform);

			costText = NewText(rerollGo.transform, "Cost", "0", 20f, TextAlignmentOptions.Right);
			StretchFull(costText.rectTransform);

			// Wire the button to the panel like a designer would (persistent listener).
			UnityEventTools.AddPersistentListener(button.onClick, panel.OnReroll);
		}

		private static void BuildClose(Transform parent, ItemShopPanel panel)
		{
			GameObject closeGo = NewUIObject("CloseButton", parent);
			Undo.RegisterCreatedObjectUndo(closeGo, "Build Item Shop");
			RectTransform rect = closeGo.GetComponent<RectTransform>();
			// Top-right corner of the panel.
			rect.anchorMin = Vector2.one;
			rect.anchorMax = Vector2.one;
			rect.pivot = Vector2.one;
			rect.sizeDelta = new Vector2(56f, 56f);
			rect.anchoredPosition = new Vector2(-10f, -10f);

			Image bg = closeGo.AddComponent<Image>();
			bg.color = new Color(0.55f, 0.18f, 0.18f, 1f);
			Button button = closeGo.AddComponent<Button>();
			button.targetGraphic = bg;

			TMP_Text label = NewText(closeGo.transform, "X", "X", 28f, TextAlignmentOptions.Center);
			StretchFull(label.rectTransform);

			// Wire to the inherited close animation like a designer would.
			UnityEventTools.AddPersistentListener(button.onClick, panel.CloseWithScaleAnimation);
		}

		private static void WirePanel(ItemShopPanel panel, List<ItemShopOfferButton> offers, Button rerollButton, TMP_Text rerollCostText, TMP_Text goldText, GameObject contentHolder)
		{
			SerializedObject so = new SerializedObject(panel);
			SerializedProperty list = so.FindProperty("offerButtons");
			list.ClearArray();
			for (int i = 0; i < offers.Count; i++)
			{
				list.InsertArrayElementAtIndex(i);
				list.GetArrayElementAtIndex(i).objectReferenceValue = offers[i];
			}
			so.FindProperty("rerollButton").objectReferenceValue = rerollButton;
			so.FindProperty("rerollCostText").objectReferenceValue = rerollCostText;
			so.FindProperty("goldText").objectReferenceValue = goldText;
			// contentHolder is the centered window GameplayDialogHandler scales for the open/close animation.
			SerializedProperty content = so.FindProperty("contentHolder");
			if (content != null)
			{
				content.objectReferenceValue = contentHolder;
			}
			so.ApplyModifiedPropertiesWithoutUndo();
		}

		private static GameObject NewUIObject(string name, Transform parent)
		{
			GameObject go = new GameObject(name, typeof(RectTransform));
			go.transform.SetParent(parent, false);
			return go;
		}

		private static TMP_Text NewText(Transform parent, string name, string text, float size, TextAlignmentOptions align)
		{
			GameObject go = NewUIObject(name, parent);
			TextMeshProUGUI tmp = go.AddComponent<TextMeshProUGUI>();
			tmp.text = text;
			tmp.fontSize = size;
			tmp.alignment = align;
			tmp.raycastTarget = false;
			if (TMP_Settings.defaultFontAsset != null)
			{
				tmp.font = TMP_Settings.defaultFontAsset;
			}
			return tmp;
		}

		private static void StretchFull(RectTransform rect)
		{
			rect.anchorMin = Vector2.zero;
			rect.anchorMax = Vector2.one;
			rect.offsetMin = Vector2.zero;
			rect.offsetMax = Vector2.zero;
		}

		private static void EditorSceneMarkDirty()
		{
			if (!Application.isPlaying)
			{
				UnityEditor.SceneManagement.EditorSceneManager.MarkAllScenesDirty();
			}
		}
	}
}
