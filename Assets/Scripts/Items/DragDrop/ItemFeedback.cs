using DG.Tweening;
using Gameplay;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Items
{
	// Self-building floating message shown near the cursor when an item can't be placed (not enough gold,
	// wrong tower type, no free slot) so the failure isn't silent. Builds its own overlay canvas + TMP label
	// on first use like DragLayer, so no scene wiring is needed and it works even when Play starts straight in
	// the gameplay scene (the shared Toast/LifespanSurface popup isn't present there).
	public class ItemFeedback : MonoBehaviour
	{
		public static void NotEnoughGold()
		{
			Show("Không đủ vàng");
		}

		public static void InventoryFull()
		{
			Show("Túi đồ đã đầy");
		}

		public static void InventoryNoRoom()
		{
			Show("Túi đồ không đủ chỗ");
		}

		// Maps a tower equip rejection to a player-facing message.
		public static void Equip(TowerEquipment.EquipBlock block)
		{
			switch (block)
			{
			case TowerEquipment.EquipBlock.WrongTower:
				Show("Sai loại tháp");
				break;
			case TowerEquipment.EquipBlock.AlreadyEquipped:
				Show("Đã trang bị item này");
				break;
			case TowerEquipment.EquipBlock.NoFreeSlot:
				Show("Hết ô trang bị");
				break;
			}
		}

		public static void Show(string message)
		{
			if (!string.IsNullOrEmpty(message))
			{
				Instance.ShowMessage(message);
			}
		}

		private static ItemFeedback Instance
		{
			get
			{
				if (instance == null)
				{
					GameObject go = new GameObject("ItemFeedback");
					DontDestroyOnLoad(go);
					instance = go.AddComponent<ItemFeedback>();
					instance.Build();
				}
				return instance;
			}
		}

		private void ShowMessage(string message)
		{
			label.SetText(message);
			root.SetActive(true);
			rect.position = PointerPosition() + new Vector2(0f, 70f);
			float startY = rect.position.y;
			group.alpha = 1f;
			if (sequence != null)
			{
				sequence.Kill();
			}
			// Float up a little while fading out. SetUpdate(true) so it still animates while the shop pauses
			// the game.
			sequence = DOTween.Sequence().SetUpdate(true);
			sequence.Append(rect.DOMoveY(startY + 50f, DURATION).SetEase(Ease.OutCubic));
			sequence.Join(group.DOFade(0f, DURATION).SetEase(Ease.InQuad));
			sequence.OnComplete(OnDone);

			if (UISfxDirector.Instance != null)
			{
				UISfxDirector.Instance.PlayClosePopup();
			}
		}

		private void OnDone()
		{
			root.SetActive(false);
		}

		private void Build()
		{
			Canvas canvas = base.gameObject.AddComponent<Canvas>();
			canvas.renderMode = RenderMode.ScreenSpaceOverlay;
			// Above the DragLayer ghost (9999) so the message reads on top of the carried item.
			canvas.sortingOrder = 10000;
			base.gameObject.AddComponent<CanvasScaler>();

			root = new GameObject("Label");
			root.transform.SetParent(base.transform, false);
			rect = root.AddComponent<RectTransform>();
			rect.sizeDelta = new Vector2(420f, 60f);
			group = root.AddComponent<CanvasGroup>();

			label = root.AddComponent<TextMeshProUGUI>();
			label.alignment = TextAlignmentOptions.Center;
			label.fontStyle = FontStyles.Bold;
			label.fontSize = 30f;
			label.color = new Color(1f, 0.45f, 0.4f);
			label.raycastTarget = false;
			if (TMP_Settings.defaultFontAsset != null)
			{
				label.font = TMP_Settings.defaultFontAsset;
			}

			root.SetActive(false);
		}

		private static Vector2 PointerPosition()
		{
			Pointer pointer = Pointer.current;
			return pointer != null
				? pointer.position.ReadValue()
				: new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
		}

		private const float DURATION = 1f;

		private GameObject root;

		private RectTransform rect;

		private CanvasGroup group;

		private TextMeshProUGUI label;

		private Sequence sequence;

		private static ItemFeedback instance;

		[UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			instance = null;
		}
	}
}
