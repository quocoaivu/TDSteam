using System.Collections.Generic;
using Items;
using TMPro;
using UnityEditor;
using UnityEditor.Events;
using UnityEngine;
using UnityEngine.UI;

namespace ItemsEditor
{
	// One-click builder for the in-match "tree" item shop. Select the GameObject that has an ItemShopPanel
	// component (it becomes the always-on tree button), then run Tools > Tower Item > Build Item Shop. It
	// turns the root into the tree button and creates TreeImage + ItemContainer (5 radial item slots) +
	// reroll + a gold label, wiring every reference (the tree's OnClick -> ToggleShop, reroll -> OnReroll).
	// Fully undoable. The tree stays on the HUD; tapping it blooms/collapses the items. Assign a tree sprite
	// to the TreeImage after building and position the root where you want it on the HUD.
	public static class ItemShopBuilder
	{
		private const int OFFER_COUNT = 5;

		// The tree button's rect (keep its on-screen position; only the size is normalized).
		private static readonly Vector2 TreeSize = new Vector2(160f, 200f);

		// Compact radial slots: just an icon + a price label below it.
		private static readonly Vector2 OfferSize = new Vector2(72f, 72f);

		// Radial arc params (must match RadialShopLayout defaults so edit-time and runtime positions agree).
		private const float RADIUS = 110f;

		private const float START_ANGLE = 20f;

		private const float END_ANGLE = 160f;

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

			Transform existing = selected.transform.Find("ItemContainer");
			if (existing != null)
			{
				EditorUtility.DisplayDialog("Item Shop Builder",
					"Đã tồn tại 'ItemContainer'. Xóa các con cũ (TreeImage / ItemContainer / RerollButton / Gold) " +
					"nếu muốn dựng lại.", "OK");
				return;
			}

			// The root must be a UI object (RectTransform under a Canvas) since it becomes the tree button.
			RectTransform rootRect = selected.GetComponent<RectTransform>();
			if (rootRect == null)
			{
				EditorUtility.DisplayDialog("Item Shop Builder",
					"GameObject này không phải UI object (thiếu RectTransform). Hãy đặt ItemShopPanel làm con của một Canvas " +
					"(chuột phải Canvas > UI > ... hoặc Create Empty rồi Add Component > Rect Transform) rồi chạy lại.", "OK");
				return;
			}

			// The root IS the always-on tree button. Give it a fixed centered rect of tree size (anchored to a
			// point so the size is absolute, not stretched). Designer repositions it on the HUD afterwards. It
			// needs a CanvasGroup so clicks can fall through to a tower while an item is being carried.
			rootRect.localScale = Vector3.one;
			rootRect.anchorMin = new Vector2(0.5f, 0.5f);
			rootRect.anchorMax = new Vector2(0.5f, 0.5f);
			rootRect.pivot = new Vector2(0.5f, 0.5f);
			rootRect.sizeDelta = TreeSize;
			if (selected.GetComponent<CanvasGroup>() == null)
			{
				selected.AddComponent<CanvasGroup>();
			}

			// Tree art = the visible tree and the Button's raycast target. Assign your tree sprite here.
			GameObject treeImage = NewUIObject("TreeImage", selected.transform);
			Undo.RegisterCreatedObjectUndo(treeImage, "Build Item Shop");
			StretchFull(treeImage.GetComponent<RectTransform>());
			Image treeImg = treeImage.AddComponent<Image>();
			treeImg.color = new Color(0.20f, 0.45f, 0.22f, 1f);

			Button treeButton = selected.GetComponent<Button>();
			if (treeButton == null)
			{
				treeButton = selected.AddComponent<Button>();
			}
			treeButton.targetGraphic = treeImg;
			// Tapping the tree toggles the items (persistent listener, like a designer would wire it).
			UnityEventTools.AddPersistentListener(treeButton.onClick, panel.ToggleShop);

			// Centered container that holds the radial slots and carries the bloom animation. Nudged up so the
			// upper arc of items sits over the top of the tree.
			GameObject itemContainer = NewUIObject("ItemContainer", selected.transform);
			Undo.RegisterCreatedObjectUndo(itemContainer, "Build Item Shop");
			RectTransform containerRect = itemContainer.GetComponent<RectTransform>();
			containerRect.anchorMin = new Vector2(0.5f, 0.5f);
			containerRect.anchorMax = new Vector2(0.5f, 0.5f);
			containerRect.pivot = new Vector2(0.5f, 0.5f);
			containerRect.sizeDelta = Vector2.zero;
			containerRect.anchoredPosition = new Vector2(0f, 40f);
			RadialShopLayout radialLayout = itemContainer.AddComponent<RadialShopLayout>();

			List<ItemShopOfferButton> offers = new List<ItemShopOfferButton>();
			for (int i = 0; i < OFFER_COUNT; i++)
			{
				offers.Add(BuildOffer(i, itemContainer.transform));
			}

			// Chrome holder (reroll + gold) shown only while the shop is open. Stretched over the root so its
			// children anchor relative to the tree; starts inactive so a closed tree shows nothing but the tree.
			GameObject chrome = NewUIObject("ShopChrome", selected.transform);
			Undo.RegisterCreatedObjectUndo(chrome, "Build Item Shop");
			StretchFull(chrome.GetComponent<RectTransform>());

			// Small gold readout above the tree.
			TMP_Text goldText = NewText(chrome.transform, "Gold", "0", 20f, TextAlignmentOptions.Center);
			RectTransform goldRect = goldText.rectTransform;
			goldRect.anchorMin = new Vector2(0.5f, 1f);
			goldRect.anchorMax = new Vector2(0.5f, 1f);
			goldRect.pivot = new Vector2(0.5f, 0f);
			goldRect.anchoredPosition = new Vector2(0f, 6f);
			goldRect.sizeDelta = new Vector2(140f, 28f);

			// Reroll node at the tree base.
			Button rerollButton;
			TMP_Text rerollCostText;
			BuildReroll(chrome.transform, panel, out rerollButton, out rerollCostText);

			chrome.SetActive(false);

			WirePanel(panel, offers, rerollButton, rerollCostText, goldText, radialLayout, treeImage.transform, chrome);

			EditorUtility.SetDirty(panel);
			EditorSceneMarkDirty();
			Debug.Log("ItemShopBuilder: dựng xong cây shop (" + OFFER_COUNT + " item radial + reroll + gold). Root đã thành " +
				"tree button (OnClick -> ToggleShop), LUÔN hiển thị. Gán tree sprite cho 'TreeImage', đặt vị trí cây trên HUD, " +
				"rồi Save scene. Không cần nút mở riêng nữa.");
		}

		private static ItemShopOfferButton BuildOffer(int index, Transform parent)
		{
			GameObject offerGo = NewUIObject("Offer_" + index, parent);
			Undo.RegisterCreatedObjectUndo(offerGo, "Build Item Shop");
			RectTransform rect = offerGo.GetComponent<RectTransform>();
			rect.anchorMin = new Vector2(0.5f, 0.5f);
			rect.anchorMax = new Vector2(0.5f, 0.5f);
			rect.pivot = new Vector2(0.5f, 0.5f);
			rect.sizeDelta = OfferSize;
			// Edit-time position on the arc (runtime PlayOpen re-drives this from the same formula).
			rect.anchoredPosition = RadialPos(index, OFFER_COUNT);

			// CanvasGroup so the bloom animation can fade the whole slot in/out.
			offerGo.AddComponent<CanvasGroup>();

			Image bg = offerGo.AddComponent<Image>();
			bg.color = new Color(0.20f, 0.22f, 0.28f, 1f);
			Button button = offerGo.AddComponent<Button>();
			button.targetGraphic = bg;

			// Item icon (runtime loads the sprite from the spec's icon key; hidden when sold). 60x60 per spec.
			GameObject iconGo = NewUIObject("Icon", offerGo.transform);
			RectTransform iconRect = iconGo.GetComponent<RectTransform>();
			iconRect.anchorMin = new Vector2(0.5f, 0.5f);
			iconRect.anchorMax = new Vector2(0.5f, 0.5f);
			iconRect.pivot = new Vector2(0.5f, 0.5f);
			iconRect.sizeDelta = new Vector2(60f, 60f);
			iconRect.anchoredPosition = Vector2.zero;
			Image iconImage = iconGo.AddComponent<Image>();
			iconImage.raycastTarget = false;
			iconImage.preserveAspect = true;

			// Price label centered just below the icon (runtime fills it; hidden once sold).
			TMP_Text costText = NewText(offerGo.transform, "Price", "0", 18f, TextAlignmentOptions.Center);
			RectTransform costRect = costText.rectTransform;
			costRect.anchorMin = new Vector2(0.5f, 0f);
			costRect.anchorMax = new Vector2(0.5f, 0f);
			costRect.pivot = new Vector2(0.5f, 1f);
			costRect.sizeDelta = new Vector2(80f, 24f);
			costRect.anchoredPosition = new Vector2(0f, -2f);

			// Sold overlay: dim cover shown after purchase.
			GameObject sold = NewUIObject("Sold", offerGo.transform);
			StretchFull(sold.GetComponent<RectTransform>());
			Image soldImg = sold.AddComponent<Image>();
			soldImg.color = new Color(0f, 0f, 0f, 0.6f);
			soldImg.raycastTarget = false;
			sold.SetActive(false);

			// nameText is intentionally left unwired: the hover tooltip shows the item name, so the compact
			// slot only needs an icon + price. ItemShopOfferButton.Refresh null-checks nameText.
			ItemShopOfferButton offer = offerGo.AddComponent<ItemShopOfferButton>();
			SerializedObject so = new SerializedObject(offer);
			so.FindProperty("button").objectReferenceValue = button;
			so.FindProperty("costText").objectReferenceValue = costText;
			so.FindProperty("iconImage").objectReferenceValue = iconImage;
			so.FindProperty("soldOverlay").objectReferenceValue = sold;
			so.ApplyModifiedPropertiesWithoutUndo();

			return offer;
		}

		// Radial position for slot i of count, matching RadialShopLayout.SlotPosition.
		private static Vector2 RadialPos(int i, int count)
		{
			float t = (count <= 1) ? 0f : (float)i / (count - 1);
			float angle = Mathf.Lerp(START_ANGLE, END_ANGLE, t) * Mathf.Deg2Rad;
			return new Vector2(Mathf.Cos(angle) * RADIUS, Mathf.Sin(angle) * RADIUS);
		}

		private static void BuildReroll(Transform parent, ItemShopPanel panel, out Button button, out TMP_Text costText)
		{
			GameObject rerollGo = NewUIObject("RerollButton", parent);
			Undo.RegisterCreatedObjectUndo(rerollGo, "Build Item Shop");
			RectTransform rect = rerollGo.GetComponent<RectTransform>();
			rect.anchorMin = new Vector2(0.5f, 0f);
			rect.anchorMax = new Vector2(0.5f, 0f);
			rect.pivot = new Vector2(0.5f, 1f);
			rect.sizeDelta = new Vector2(150f, 44f);
			rect.anchoredPosition = new Vector2(0f, -4f);

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

		private static void WirePanel(ItemShopPanel panel, List<ItemShopOfferButton> offers, Button rerollButton, TMP_Text rerollCostText, TMP_Text goldText, RadialShopLayout radialLayout, Transform treePunchTarget, GameObject shopChrome)
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
			so.FindProperty("radialLayout").objectReferenceValue = radialLayout;
			so.FindProperty("treePunchTarget").objectReferenceValue = treePunchTarget;
			so.FindProperty("shopChrome").objectReferenceValue = shopChrome;
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
