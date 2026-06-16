using System;
using GameCore;
using UnityEngine;

namespace Gameplay
{
	public class TrooperVitalityView : BaseMonoBehaviour
	{
		private void Update()
		{
			if (isReadyToUse && healthBarPosition)
			{
				UpdatePositionFollowUnit();
			}
		}

		public void SetupHealth(CharacterKind _characterType, GameObject _target, Transform _healthBarPosition)
		{
			characterType = _characterType;
			switch (characterType)
			{
			case CharacterKind.Ally:
				healthStatus.sprite = greenHealthBar;
				blackBar.sprite = smallBlackBar;
				healthBarStatus.localPosition = new Vector3(smallOffset, 0f, 0f);
				Hide();
				break;
			case CharacterKind.Enemy:
				healthStatus.sprite = redHealthBar;
				blackBar.sprite = smallBlackBar;
				healthBarStatus.localPosition = new Vector3(smallOffset, 0f, 0f);
				Hide();
				break;
			case CharacterKind.Boss:
				healthStatus.sprite = bigRedHealthBar;
				blackBar.sprite = bigBlackBar;
				healthBarStatus.localPosition = new Vector3(bigOffset, -0.003f, 0f);
				Show();
				break;
			}
			target = _target;
			healthBarPosition = _healthBarPosition;
			healthBarStatus.transform.localScale = Vector3.one;
			isReadyToUse = true;
		}

		public void UpdateHealth(int _currentHealth, int _originHealth)
		{
			int num = Mathf.Clamp(_currentHealth, 0, _originHealth);
			if (healthBarStatus)
			{
				healthBarStatus.localScale = new Vector3((float)num / (float)_originHealth, 1f, 1f);
			}
			if (isReadyToUse)
			{
				if (characterType == CharacterKind.Boss)
				{
					return;
				}
				if (_currentHealth == _originHealth)
				{
					Hide();
				}
				else
				{
					Show();
				}
			}
		}

		private void UpdatePositionFollowUnit()
		{
			base.transform.position = healthBarPosition.position;
		}

		public void OnReturnPool()
		{
			Hide();
			isReadyToUse = false;
			MonoSingleton<UnitHealthBarPool>.Instance.Despawn(this);
		}

		private void Show()
		{
			// Snap to the unit before activating: Update() (the follow) doesn't run while the
			// object is inactive, so without this the bar would flash 1 frame at its pooled
			// position before catching up to the enemy.
			if (healthBarPosition)
			{
				base.transform.position = healthBarPosition.position;
			}
			base.gameObject.SetActive(true);
		}

		public void Hide()
		{
			base.gameObject.SetActive(false);
		}

		[SerializeField]
		private Transform healthBarStatus;

		[Space]
		[Header("Reference")]
		[SerializeField]
		private SpriteRenderer healthStatus;

		[SerializeField]
		private SpriteRenderer blackBar;

		[Space]
		[Header("Assets")]
		[SerializeField]
		private Sprite smallBlackBar;

		[Space]
		[Header("Assets")]
		[SerializeField]
		private Sprite bigBlackBar;

		[SerializeField]
		private Sprite redHealthBar;

		[SerializeField]
		private Sprite bigRedHealthBar;

		[SerializeField]
		private Sprite greenHealthBar;

		private GameObject target;

		private Transform healthBarPosition;

		private bool isReadyToUse;

		[Space]
		[SerializeField]
		private float smallOffset;

		[Space]
		[SerializeField]
		private float bigOffset;

		private CharacterKind characterType;
	}
}
