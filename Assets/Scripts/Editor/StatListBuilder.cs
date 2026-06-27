using Gameplay;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace GameplayEditor
{
	// One-click builder for the tower stat list UI. Select the GameObject that has a
	// CurrentLevelOverviewDialog component (CurrentLevelInformationPopup in the Gameplay scene),
	// then run Tools > Tower Info > Build Stat List. It creates:
	//   - "StatContainer" (VerticalLayoutGroup + ContentSizeFitter) where rows spawn,
	//   - "StatRowTemplate" (StatRowView + 2 TMP_Text, inactive, OUTSIDE the container),
	// then wires statContainer / statRowTemplate on the dialog and labelText / valueText on the row.
	// Re-run safe (refuses if the objects already exist). Mirrors ItemSlotBuilder.
	public static class StatListBuilder
	{
		[MenuItem("Tools/Tower Info/Build Stat List")]
		private static void BuildStatList()
		{
			GameObject selected = Selection.activeGameObject;
			if (selected == null)
			{
				EditorUtility.DisplayDialog("Stat List Builder",
					"Chọn GameObject có component CurrentLevelOverviewDialog rồi chạy lại.", "OK");
				return;
			}
			CurrentLevelOverviewDialog dialog = selected.GetComponent<CurrentLevelOverviewDialog>();
			if (dialog == null)
			{
				EditorUtility.DisplayDialog("Stat List Builder",
					"GameObject đang chọn không có CurrentLevelOverviewDialog. Chọn đúng CurrentLevelInformationPopup.", "OK");
				return;
			}
			if (selected.transform.Find("StatContainer") != null || selected.transform.Find("StatRowTemplate") != null)
			{
				EditorUtility.DisplayDialog("Stat List Builder",
					"Đã tồn tại 'StatContainer' hoặc 'StatRowTemplate'. Xóa chúng trước nếu muốn dựng lại.", "OK");
				return;
			}

			Transform container = BuildContainer(selected.transform);
			StatRowView template = BuildTemplate(selected.transform);

			WireDialog(dialog, container, template);

			EditorUtility.SetDirty(dialog);
			MarkSceneDirty();
			Debug.Log("StatListBuilder: dựng xong StatContainer + StatRowTemplate và wire vào dialog. Nhớ Save scene.");
		}

		private static Transform BuildContainer(Transform parent)
		{
			GameObject go = NewUIObject("StatContainer", parent);
			Undo.RegisterCreatedObjectUndo(go, "Build Stat List");
			StretchFull(go.GetComponent<RectTransform>());

			VerticalLayoutGroup layout = go.AddComponent<VerticalLayoutGroup>();
			layout.childControlWidth = true;
			layout.childControlHeight = true;
			layout.childForceExpandWidth = true;
			layout.childForceExpandHeight = false;
			layout.spacing = 4f;

			ContentSizeFitter fitter = go.AddComponent<ContentSizeFitter>();
			fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

			return go.transform;
		}

		private static StatRowView BuildTemplate(Transform parent)
		{
			GameObject go = NewUIObject("StatRowTemplate", parent);
			Undo.RegisterCreatedObjectUndo(go, "Build Stat List");
			RectTransform rect = go.GetComponent<RectTransform>();
			rect.sizeDelta = new Vector2(0f, 30f);

			HorizontalLayoutGroup layout = go.AddComponent<HorizontalLayoutGroup>();
			layout.childControlWidth = true;
			layout.childControlHeight = true;
			layout.childForceExpandWidth = true;
			layout.childForceExpandHeight = true;

			LayoutElement element = go.AddComponent<LayoutElement>();
			element.minHeight = 28f;
			element.preferredHeight = 30f;

			TMP_Text label = NewText(go.transform, "Label", "Stat", 18f, TextAlignmentOptions.MidlineLeft);
			TMP_Text value = NewText(go.transform, "Value", "0", 18f, TextAlignmentOptions.MidlineRight);

			StatRowView row = go.AddComponent<StatRowView>();
			SerializedObject so = new SerializedObject(row);
			so.FindProperty("labelText").objectReferenceValue = label;
			so.FindProperty("valueText").objectReferenceValue = value;
			so.ApplyModifiedPropertiesWithoutUndo();

			// Template must stay inactive and outside the container so layout never counts it.
			go.SetActive(false);
			return row;
		}

		private static void WireDialog(CurrentLevelOverviewDialog dialog, Transform container, StatRowView template)
		{
			SerializedObject so = new SerializedObject(dialog);
			so.FindProperty("statContainer").objectReferenceValue = container;
			so.FindProperty("statRowTemplate").objectReferenceValue = template;
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

		private static void MarkSceneDirty()
		{
			if (!Application.isPlaying)
			{
				UnityEditor.SceneManagement.EditorSceneManager.MarkAllScenesDirty();
			}
		}
	}
}
