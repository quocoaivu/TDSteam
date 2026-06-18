using TMPro;
using Upgrade;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace UpgradeEditor
{
	// One-click builder for the shared skill-tree panel skeleton. Select the panel GameObject that
	// has a TowerSkillTreePanel component (open the tree prefab in prefab mode and select its root),
	// then run Tools > Tower Skill Tree > Build Skill Tree Template. It creates the two containers
	// (nodes + lines) plus one inactive node template and one inactive line template, and wires them
	// into the panel. At runtime the panel clones these templates once per node/edge of whichever
	// tower it is opened for (positions/names/costs come from the CSV). Fully undoable.
	public static class SkillTreeBuilder
	{
		private static readonly Vector2 NodeSize = new Vector2(110f, 110f);
		private const string UndoLabel = "Build Skill Tree Template";

		[MenuItem("Tools/Tower Skill Tree/Build Skill Tree Template")]
		private static void BuildTemplate()
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
			if (selected.transform.Find("SkillTreeLines") != null ||
				selected.transform.Find("SkillTreeNodes") != null ||
				selected.transform.Find("NodeTemplate") != null ||
				selected.transform.Find("LineTemplate") != null)
			{
				EditorUtility.DisplayDialog("Skill Tree Builder",
					"Đã tồn tại container/template cũ. Xóa nó trước nếu muốn dựng lại.", "OK");
				return;
			}

			// Line container nằm dưới node container để đường vẽ phía sau node.
			GameObject lineContainer = NewUIObject("SkillTreeLines", selected.transform);
			Undo.RegisterCreatedObjectUndo(lineContainer, UndoLabel);
			StretchFull(lineContainer.GetComponent<RectTransform>());

			GameObject nodeContainer = NewUIObject("SkillTreeNodes", selected.transform);
			Undo.RegisterCreatedObjectUndo(nodeContainer, UndoLabel);
			StretchFull(nodeContainer.GetComponent<RectTransform>());

			SkillTreeNodeButton nodeTemplate = BuildNodeTemplate(selected.transform);
			SkillTreeLine lineTemplate = BuildLineTemplate(selected.transform);

			WirePanel(panel, nodeTemplate, lineTemplate,
				nodeContainer.GetComponent<RectTransform>(), lineContainer.GetComponent<RectTransform>());

			EditorUtility.SetDirty(panel);
			EditorSceneMarkDirty();
			Debug.Log("SkillTreeBuilder: dựng xong template + container + wire vào panel. Nhớ Save prefab.");
		}

		private static SkillTreeNodeButton BuildNodeTemplate(Transform parent)
		{
			GameObject nodeGo = NewUIObject("NodeTemplate", parent);
			Undo.RegisterCreatedObjectUndo(nodeGo, UndoLabel);
			RectTransform rect = nodeGo.GetComponent<RectTransform>();
			rect.sizeDelta = NodeSize;
			rect.anchoredPosition = Vector2.zero;

			// Background image = the clickable graphic.
			Image bg = nodeGo.AddComponent<Image>();
			bg.color = new Color(0.20f, 0.22f, 0.28f, 1f);
			Button button = nodeGo.AddComponent<Button>();
			button.targetGraphic = bg;

			// Name label (runtime fills the text per node).
			TMP_Text nameText = NewText(nodeGo.transform, "Name", "Node", 14f, TextAlignmentOptions.Top);
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
			so.FindProperty("nodeID").intValue = 0;
			so.FindProperty("nameText").objectReferenceValue = nameText;
			so.FindProperty("button").objectReferenceValue = button;
			so.FindProperty("costText").objectReferenceValue = costText;
			so.FindProperty("lockedOverlay").objectReferenceValue = locked;
			so.FindProperty("unlockedOverlay").objectReferenceValue = unlocked;
			so.ApplyModifiedPropertiesWithoutUndo();

			nodeGo.SetActive(false); // template: chỉ dùng để clone, luôn tắt
			return nodeButton;
		}

		private static SkillTreeLine BuildLineTemplate(Transform parent)
		{
			Sprite uiSprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");

			GameObject go = NewUIObject("LineTemplate", parent);
			Undo.RegisterCreatedObjectUndo(go, UndoLabel);
			Image baseImg = go.AddComponent<Image>();
			baseImg.sprite = uiSprite;
			baseImg.raycastTarget = false;

			// Phần đổ đầy: Image kiểu Filled, fill ngang từ đầu prereq -> phía node.
			GameObject fillGo = NewUIObject("Fill", go.transform);
			StretchFull(fillGo.GetComponent<RectTransform>());
			Image fillImg = fillGo.AddComponent<Image>();
			fillImg.sprite = uiSprite;
			fillImg.type = Image.Type.Filled;
			fillImg.fillMethod = Image.FillMethod.Horizontal;
			fillImg.fillOrigin = (int)Image.OriginHorizontal.Left;
			fillImg.fillAmount = 0f;
			fillImg.color = new Color(0.5f, 0.9f, 0.4f, 1f); // khớp unlockedColor mặc định
			fillImg.raycastTarget = false;

			// Đốm sáng chạy dọc đường khi node đang ở trạng thái Ready. Ẩn mặc định.
			GameObject glowGo = NewUIObject("Glow", go.transform);
			RectTransform glowRect = glowGo.GetComponent<RectTransform>();
			glowRect.anchorMin = new Vector2(0.5f, 0.5f);
			glowRect.anchorMax = new Vector2(0.5f, 0.5f);
			glowRect.pivot = new Vector2(0.5f, 0.5f);
			glowRect.sizeDelta = new Vector2(28f, 10f);
			Image glowImg = glowGo.AddComponent<Image>();
			glowImg.sprite = uiSprite;
			glowImg.color = new Color(1f, 1f, 0.7f, 0.8f);
			glowImg.raycastTarget = false;
			glowGo.SetActive(false);

			SkillTreeLine line = go.AddComponent<SkillTreeLine>();
			SerializedObject so = new SerializedObject(line);
			so.FindProperty("baseImage").objectReferenceValue = baseImg;
			so.FindProperty("fillImage").objectReferenceValue = fillImg;
			so.FindProperty("glow").objectReferenceValue = glowRect;
			so.ApplyModifiedPropertiesWithoutUndo();

			go.SetActive(false); // template: chỉ dùng để clone, luôn tắt
			return line;
		}

		private static void WirePanel(TowerSkillTreePanel panel, SkillTreeNodeButton nodeTemplate,
			SkillTreeLine lineTemplate, RectTransform nodeContainer, RectTransform lineContainer)
		{
			SerializedObject so = new SerializedObject(panel);
			so.FindProperty("nodeTemplate").objectReferenceValue = nodeTemplate;
			so.FindProperty("lineTemplate").objectReferenceValue = lineTemplate;
			so.FindProperty("nodeContainer").objectReferenceValue = nodeContainer;
			so.FindProperty("lineContainer").objectReferenceValue = lineContainer;
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
