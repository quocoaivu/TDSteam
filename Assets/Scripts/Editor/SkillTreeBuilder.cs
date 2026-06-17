using System.Collections.Generic;
using TMPro;
using Upgrade;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace UpgradeEditor
{
	// One-click builder for the Archer skill-tree UI. Select the panel GameObject that has a
	// TowerSkillTreePanel component (open PanelArcherSkillTree in prefab mode and select its root),
	// then run Tools > Tower Skill Tree > Build Archer Nodes. It creates the 14 node buttons,
	// lays them out, wires every reference, and registers them in the panel. Fully undoable.
	public static class SkillTreeBuilder
	{
		// id, display name, anchored position (UI units, y up). Matches the designed Archer tree.
		private struct NodeLayout
		{
			public int id;
			public string name;
			public Vector2 pos;
			public NodeLayout(int id, string name, float x, float y)
			{
				this.id = id;
				this.name = name;
				this.pos = new Vector2(x, y);
			}
		}

		private static readonly NodeLayout[] Nodes =
		{
			new NodeLayout(0, "Sharpened I", 0f, 280f),
			new NodeLayout(1, "Sharpened II", -300f, 160f),
			new NodeLayout(6, "Eagle Eye I", 0f, 160f),
			new NodeLayout(3, "Quick Draw I", 300f, 160f),
			new NodeLayout(2, "Sharpened III", -360f, 40f),
			new NodeLayout(9, "Deadly Aim", -180f, 40f),
			new NodeLayout(7, "Eagle Eye II", 0f, 40f),
			new NodeLayout(4, "Quick Draw II", 300f, 40f),
			new NodeLayout(11, "Crit Mastery", -180f, -80f),
			new NodeLayout(8, "Marksman", -40f, -80f),
			new NodeLayout(10, "Armor Pierce", 100f, -80f),
			new NodeLayout(5, "Rapid Fire", 300f, -80f),
			new NodeLayout(12, "Hawkeye Volley", -140f, -210f),
			new NodeLayout(13, "Storm of Arrows", 200f, -210f),
		};

		private static readonly Vector2 NodeSize = new Vector2(110f, 110f);

		[MenuItem("Tools/Tower Skill Tree/Build Archer Nodes")]
		private static void BuildArcherNodes()
		{
			GameObject selected = Selection.activeGameObject;
			if (selected == null)
			{
				EditorUtility.DisplayDialog("Skill Tree Builder",
					"Chọn GameObject gốc của panel (có component TowerSkillTreePanel) rồi chạy lại.", "OK");
				return;
			}
			TowerSkillTreePanel panel = selected.GetComponent<TowerSkillTreePanel>();
			if (panel == null)
			{
				EditorUtility.DisplayDialog("Skill Tree Builder",
					"GameObject đang chọn không có TowerSkillTreePanel. Hãy chọn đúng panel gốc.", "OK");
				return;
			}

			Transform existing = selected.transform.Find("SkillTreeNodes");
			if (existing != null)
			{
				EditorUtility.DisplayDialog("Skill Tree Builder",
					"Đã tồn tại 'SkillTreeNodes'. Xóa nó trước nếu muốn dựng lại.", "OK");
				return;
			}

			GameObject container = NewUIObject("SkillTreeNodes", selected.transform);
			Undo.RegisterCreatedObjectUndo(container, "Build Archer Nodes");
			StretchFull(container.GetComponent<RectTransform>());

			List<SkillTreeNodeButton> built = new List<SkillTreeNodeButton>();
			foreach (NodeLayout layout in Nodes)
			{
				built.Add(BuildNode(layout, container.transform));
			}

			WirePanel(panel, built);

			EditorUtility.SetDirty(panel);
			EditorSceneMarkDirty();
			Debug.Log("SkillTreeBuilder: dựng xong 14 node Archer + wire vào panel. Nhớ Save prefab.");
		}

		private static SkillTreeNodeButton BuildNode(NodeLayout layout, Transform parent)
		{
			GameObject nodeGo = NewUIObject("Node_" + layout.id, parent);
			Undo.RegisterCreatedObjectUndo(nodeGo, "Build Archer Nodes");
			RectTransform rect = nodeGo.GetComponent<RectTransform>();
			rect.sizeDelta = NodeSize;
			rect.anchoredPosition = layout.pos;

			// Background image = the clickable graphic.
			Image bg = nodeGo.AddComponent<Image>();
			bg.color = new Color(0.20f, 0.22f, 0.28f, 1f);
			Button button = nodeGo.AddComponent<Button>();
			button.targetGraphic = bg;

			// Name label (static, just for readability in the editor / player).
			TMP_Text nameText = NewText(nodeGo.transform, "Name", layout.name, 14f, TextAlignmentOptions.Top);
			StretchFull(nameText.rectTransform);

			// Cost text (runtime fills the number; hidden when owned).
			TMP_Text costText = NewText(nodeGo.transform, "Cost", "0", 20f, TextAlignmentOptions.Bottom);
			StretchFull(costText.rectTransform);

			// Locked overlay: dim cover shown while prerequisites aren't met.
			GameObject locked = NewUIObject("Locked", nodeGo.transform);
			StretchFull(locked.GetComponent<RectTransform>());
			Image lockedImg = locked.AddComponent<Image>();
			lockedImg.color = new Color(0f, 0f, 0f, 0.6f);
			lockedImg.raycastTarget = false;

			// Unlocked overlay: green tint shown once owned.
			GameObject unlocked = NewUIObject("Unlocked", nodeGo.transform);
			StretchFull(unlocked.GetComponent<RectTransform>());
			Image unlockedImg = unlocked.AddComponent<Image>();
			unlockedImg.color = new Color(0.2f, 0.8f, 0.2f, 0.35f);
			unlockedImg.raycastTarget = false;
			unlocked.SetActive(false);

			SkillTreeNodeButton nodeButton = nodeGo.AddComponent<SkillTreeNodeButton>();
			SerializedObject so = new SerializedObject(nodeButton);
			so.FindProperty("nodeID").intValue = layout.id;
			so.FindProperty("button").objectReferenceValue = button;
			so.FindProperty("costText").objectReferenceValue = costText;
			so.FindProperty("lockedOverlay").objectReferenceValue = locked;
			so.FindProperty("unlockedOverlay").objectReferenceValue = unlocked;
			so.ApplyModifiedPropertiesWithoutUndo();

			return nodeButton;
		}

		private static void WirePanel(TowerSkillTreePanel panel, List<SkillTreeNodeButton> buttons)
		{
			SerializedObject so = new SerializedObject(panel);
			SerializedProperty list = so.FindProperty("nodeButtons");
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
