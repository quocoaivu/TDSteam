using UnityEngine;
using UnityEngine.UI;

namespace Items
{
	// Full-screen overlay that draws a single "ghost" of the item being dragged, following the cursor.
	// The ghost shows the item's icon sprite. Self-builds its canvas + ghost on first use so no scene
	// wiring is required. The ghost never blocks raycasts, so drop targets underneath still receive the
	// pointer. One ghost, reused (pooled) across drags per the project's no-Instantiate-in-gameplay rule.
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
			Sprite sprite = (item != null && !string.IsNullOrEmpty(item.icon))
				? Common.AssetLoader.Load<Sprite>(item.icon) : null;
			ghostImage.sprite = sprite;
			ghostImage.enabled = sprite != null;
			ghost.SetActive(true);
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
			ghostRect.sizeDelta = new Vector2(100f, 100f);

			ghostImage = ghost.AddComponent<Image>();
			ghostImage.raycastTarget = false;
			ghostImage.preserveAspect = true;
			// Slightly translucent so it reads as a held "ghost".
			ghostImage.color = new Color(1f, 1f, 1f, 0.85f);

			ghost.SetActive(false);
		}

		private static DragLayer instance;

		private GameObject ghost;

		private RectTransform ghostRect;

		private Image ghostImage;

		[UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			instance = null;
		}
	}
}
