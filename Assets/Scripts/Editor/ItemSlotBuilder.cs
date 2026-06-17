using System.Collections.Generic;
using Items;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace ItemsEditor
{
	// One-click builder for the tower item-equip UI. Select the GameObject that has a TowerItemPanel
	// component (the panel you created under "Upgrade Tower" in the Gameplay scene), then run
	// Tools > Tower Item > Build Item Slots. It creates 2 ItemSlotButtons (one per ability the tower's
	// canonical prefab carries), lays them out, wires every reference, and registers them in the panel.
	// Fully undoable. Mirrors SkillTreeBuilder.
	public static class ItemSlotBuilder
	{
		private const int SLOT_COUNT = 2;

		private static readonly Vector2 SlotSize = new Vector2(140f, 140f);

		// Horizontal spacing between the two slots (anchored x).
		private static readonly float[] SlotX = { -90f, 90f };

		[MenuItem("Tools/Tower Item/Build Item Slots")]
		private static void BuildItemSlots()
		{
			GameObject selected = Selection.activeGameObject;
			if (selected == null)
			{
				EditorUtility.DisplayDialog("Item Slot Builder",
					"Chọn GameObject có component TowerItemPanel rồi chạy lại.", "OK");
				return;
			}
			TowerItemPanel panel = selected.GetComponent<TowerItemPanel>();
			if (panel == null)
			{
				EditorUtility.DisplayDialog("Item Slot Builder",
					"GameObject đang chọn không có TowerItemPanel. Hãy chọn đúng panel.", "OK");
				return;
			}

			Transform existing = selected.transform.Find("ItemSlots");
			if (existing != null)
			{
				EditorUtility.DisplayDialog("Item Slot Builder",
					"Đã tồn tại 'ItemSlots'. Xóa nó trước nếu muốn dựng lại.", "OK");
				return;
			}

			GameObject container = NewUIObject("ItemSlots", selected.transform);
			Undo.RegisterCreatedObjectUndo(container, "Build Item Slots");
			StretchFull(container.GetComponent<RectTransform>());

			List<ItemSlotButton> built = new List<ItemSlotButton>();
			for (int i = 0; i < SLOT_COUNT; i++)
			{
				built.Add(BuildSlot(i, container.transform));
			}

			WirePanel(panel, built);

			EditorUtility.SetDirty(panel);
			EditorSceneMarkDirty();
			Debug.Log("ItemSlotBuilder: dựng xong 2 item slot + wire vào panel. Nhớ Save scene.");
		}

		private static ItemSlotButton BuildSlot(int slotIndex, Transform parent)
		{
			GameObject slotGo = NewUIObject("Slot_" + slotIndex, parent);
			Undo.RegisterCreatedObjectUndo(slotGo, "Build Item Slots");
			RectTransform rect = slotGo.GetComponent<RectTransform>();
			rect.sizeDelta = SlotSize;
			rect.anchoredPosition = new Vector2(SlotX[slotIndex], 0f);

			// Background image = the clickable graphic.
			Image bg = slotGo.AddComponent<Image>();
			bg.color = new Color(0.20f, 0.22f, 0.28f, 1f);
			Button button = slotGo.AddComponent<Button>();
			button.targetGraphic = bg;

			// Item name label (runtime fills it; "—" when no item owned).
			TMP_Text nameText = NewText(slotGo.transform, "Name", "—", 16f, TextAlignmentOptions.Center);
			StretchFull(nameText.rectTransform);

			// Empty overlay: dim cover shown when no compatible item is owned.
			GameObject empty = NewUIObject("Empty", slotGo.transform);
			StretchFull(empty.GetComponent<RectTransform>());
			Image emptyImg = empty.AddComponent<Image>();
			emptyImg.color = new Color(0f, 0f, 0f, 0.6f);
			emptyImg.raycastTarget = false;

			// Equipped overlay: green tint shown once the item is equipped on the tower.
			GameObject equipped = NewUIObject("Equipped", slotGo.transform);
			StretchFull(equipped.GetComponent<RectTransform>());
			Image equippedImg = equipped.AddComponent<Image>();
			equippedImg.color = new Color(0.2f, 0.8f, 0.2f, 0.35f);
			equippedImg.raycastTarget = false;
			equipped.SetActive(false);

			ItemSlotButton slotButton = slotGo.AddComponent<ItemSlotButton>();
			SerializedObject so = new SerializedObject(slotButton);
			so.FindProperty("slotIndex").intValue = slotIndex;
			so.FindProperty("button").objectReferenceValue = button;
			so.FindProperty("nameText").objectReferenceValue = nameText;
			so.FindProperty("emptyOverlay").objectReferenceValue = empty;
			so.FindProperty("equippedOverlay").objectReferenceValue = equipped;
			so.ApplyModifiedPropertiesWithoutUndo();

			return slotButton;
		}

		private static void WirePanel(TowerItemPanel panel, List<ItemSlotButton> buttons)
		{
			SerializedObject so = new SerializedObject(panel);
			SerializedProperty list = so.FindProperty("slotButtons");
			list.ClearArray();
			for (int i = 0; i < buttons.Count; i++)
			{
				list.InsertArrayElementAtIndex(i);
				list.GetArrayElementAtIndex(i).objectReferenceValue = buttons[i];
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
