using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Items
{
	// Full-screen overlay that draws a single "ghost" of the item being dragged, following the cursor.
	// Self-builds its canvas + ghost on first use so no scene wiring is required. The ghost never blocks
	// raycasts, so drop targets underneath still receive the pointer. One ghost, reused (pooled) across
	// drags per the project's no-Instantiate-in-gameplay rule.
	public class DragLayer : MonoBehaviour
	{
		public static DragLayer Instance
		{
			get
			{
				if (instance == null)
				{
					GameObject go = new GameObject("DragLayer");
					DontDestroyOnLoad(go);
					instance = go.AddComponent<DragLayer>();
					instance.Build();
				}
				return instance;
			}
		}

		public void BeginDrag(TowerItem item)
		{
			ghost.SetActive(true);
			ghostLabel.SetText(item != null ? item.name : string.Empty);
		}

		public void MoveTo(Vector2 screenPosition)
		{
			ghostRect.position = screenPosition;
		}

		public void EndDrag()
		{
			ghost.SetActive(false);
		}

		private void Build()
		{
			Canvas canvas = base.gameObject.AddComponent<Canvas>();
			canvas.renderMode = RenderMode.ScreenSpaceOverlay;
			canvas.sortingOrder = 9999;
			base.gameObject.AddComponent<CanvasScaler>();

			ghost = new GameObject("Ghost");
			ghost.transform.SetParent(base.transform, false);
			ghostRect = ghost.AddComponent<RectTransform>();
			ghostRect.sizeDelta = new Vector2(220f, 56f);

			Image bg = ghost.AddComponent<Image>();
			bg.color = new Color(0f, 0f, 0f, 0.6f);
			bg.raycastTarget = false;

			GameObject labelGo = new GameObject("Label");
			labelGo.transform.SetParent(ghost.transform, false);
			RectTransform labelRect = labelGo.AddComponent<RectTransform>();
			labelRect.anchorMin = Vector2.zero;
			labelRect.anchorMax = Vector2.one;
			labelRect.sizeDelta = Vector2.zero;
			ghostLabel = labelGo.AddComponent<TextMeshProUGUI>();
			ghostLabel.alignment = TextAlignmentOptions.Center;
			ghostLabel.fontSize = 24f;
			ghostLabel.raycastTarget = false;

			ghost.SetActive(false);
		}

		private static DragLayer instance;

		private GameObject ghost;

		private RectTransform ghostRect;

		private TextMeshProUGUI ghostLabel;

		[UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			instance = null;
		}
	}
}
