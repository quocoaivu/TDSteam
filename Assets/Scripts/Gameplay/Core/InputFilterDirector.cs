using System;
using System.Diagnostics;
using GeneralVariable;
using Tutorial;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

namespace Gameplay
{
	public class InputFilterDirector : MonoBehaviour
	{
        [Space]
        [Header("Threshold")]
        [SerializeField]
        private float movementThresHole;

        private const float Vector2DZ = 10f;

        [Space]
        [Header("Mouse click threshold")]
        [SerializeField]
        private float deltaTimeClick;

        [SerializeField]
        private float deltaDistanceClick = 40f;

        private float timeMouseDown;

        private float timeMouseUp;

        private Vector2 distanceMouseDown;

        private Vector2 distanceMouseUp;

        private float timeTracking;

        private bool isClickUI;

        private bool isMovingCamera;

        private RaycastHit2D hit;

        private RaycastHit2D mapHit;

        private int mapLayerIndex;

        private VisualEffectSpawner effectCaster;

        private int lastTouchCount;

        private GameInput input;
        
		public event Action onMouseStayEvent;

		public event Action onMouseClickEvent;

		public event Action onMouseDownEvent;

		public event Action onMouseUpEvent;

		public event Action onMoveCamera;

		public event Action onZoomCamera;

		public event Action onClickBuildRegion;

		public static InputFilterDirector Instance { get; set; }

		private void Awake()
		{
			InputFilterDirector.Instance = this;
			effectCaster = base.GetComponent<VisualEffectSpawner>();
			mapLayerIndex = 1 << LayerMask.NameToLayer("Map");
			input = new GameInput();
		}

		private void OnEnable()
		{
			if (input == null)
			{
				input = new GameInput();
			}
			input.Gameplay.Enable();
		}

		private void OnDisable()
		{
			if (input == null)
			{
				return;
			}
			input.Gameplay.Disable();
		}

		private void OnDestroy()
		{
			if (input != null)
			{
				input.Dispose();
				input = null;
			}
		}

		private void Update()
		{
			if (MonoSingleton<GameRecord>.Instance.IsGameOver)
			{
				return;
			}
			if (MonoSingleton<GameRecord>.Instance.IsAnyPopupOpen)
			{
				return;
			}
			if (MonoSingleton<GameRecord>.Instance.IsAnyTutorialPopupOpen)
			{
				return;
			}

			Touchscreen touchscreen = Touchscreen.current;
			int activeTouches = 0;
			TouchControl primaryTouch = null;
			if (touchscreen != null)
			{
				for (int i = 0; i < touchscreen.touches.Count; i++)
				{
					if (touchscreen.touches[i].press.isPressed)
					{
						if (activeTouches == 0)
						{
							primaryTouch = touchscreen.touches[i];
						}
						activeTouches++;
					}
				}
			}

			if (activeTouches == 1 && primaryTouch.phase.ReadValue() == TouchPhase.Moved && GameplayTutorialDirector.Instance.IsTutorialDone())
			{
				Vector2 deltaWorldPosition = GetDeltaWorldPosition(primaryTouch.delta.ReadValue(), Camera.main);
				if (Mathf.Abs(deltaWorldPosition.x) > movementThresHole || Mathf.Abs(deltaWorldPosition.y) > movementThresHole)
				{
					isMovingCamera = true;
					if (onMoveCamera != null)
					{
						onMoveCamera();
						return;
					}
				}
			}
			else if (activeTouches == 2 && GameplayTutorialDirector.Instance.IsTutorialDone() && onZoomCamera != null)
			{
				onZoomCamera();
				SetIsClickingUI();
				return;
			}

			if (input.Gameplay.Click.WasPressedThisFrame() && InputFilterDirector.IsPointerOverUI())
			{
				SetIsClickingUI();
				return;
			}

			if (input.Gameplay.Click.WasReleasedThisFrame())
			{
				OnClick();
				if (isClickUI)
				{
					return;
				}
				if (isMovingCamera)
				{
					isMovingCamera = false;
					return;
				}
				if (InputFilterDirector.IsPointerOverUI())
				{
					SetIsClickingUI();
					return;
				}
				hit = Physics2D.Raycast(getTargetVector(), Vector3.back, 5f);
				mapHit = Physics2D.Raycast(getTargetVector(), Vector3.back, 5f, mapLayerIndex);
				HandleInput(new TapInputRecord(TapInputPhase.Up, mapHit, hit));
			}
			if (timeTracking == 0f)
			{
				ResetTimeout();
			}
			timeTracking = Mathf.MoveTowards(timeTracking, 0f, Time.deltaTime);
		}

		private void ResetTimeout()
		{
			isClickUI = false;
		}

		public void SetIsClickingUI()
		{
			isClickUI = true;
			timeTracking = deltaTimeClick;
		}

		private static bool IsPointerOverUI()
		{
			return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
		}

		public static bool IsPointerOverGUI()
		{
			if (EventSystem.current == null)
			{
				return false;
			}
			if (EventSystem.current.IsPointerOverGameObject())
			{
				return true;
			}
			Touchscreen touchscreen = Touchscreen.current;
			if (touchscreen == null)
			{
				return false;
			}
			for (int i = 0; i < touchscreen.touches.Count; i++)
			{
				TouchControl touch = touchscreen.touches[i];
				if (touch.press.isPressed && EventSystem.current.IsPointerOverGameObject(touch.touchId.ReadValue()))
				{
					return true;
				}
			}
			return false;
		}

		private Vector2 getTargetVector()
		{
			Vector2 screenPosition = input.Gameplay.Point.ReadValue<Vector2>();
			Vector3 vector = Camera.main.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, 0f));
			return new Vector2(vector.x, vector.y);
		}

		private Vector2 GetDeltaWorldPosition(Vector2 deltaScreen, Camera camera)
		{
			Vector3 position = deltaScreen;
			position.z = 10f;
			Vector3 position2 = new Vector3(0f, 0f, 10f);
			return camera.ScreenToWorldPoint(position) - camera.ScreenToWorldPoint(position2);
		}

		public void OnAButtonClicked(TappedObjectRecord clickedObjData)
		{
			SetIsClickingUI();
		}

		private void OnClick()
		{
			GameSignalCenter.Instance.Trigger(GameSignalKind.OnSelectEnemy, -1);
			GameSignalCenter.Instance.Trigger(GameSignalKind.OnSelectPet, -1);
			GameSignalCenter.Instance.Trigger(GameSignalKind.OnSelectAlly, null);
		}

		private void Start()
		{
			GameSignalCenter.Instance.Subscribe(GameSignalKind.OnClickButton, new TapSwitchListenerRecord(GameKit.GetUniqueId(), new GameSignalCenter.ClickButtonMethod(OnAButtonClicked)));
		}

		private void HandleInput(TapInputRecord inputData)
		{
			TapInputPhase clickInputPhase = inputData.clickInputPhase;
			if (clickInputPhase == TapInputPhase.Up)
			{
				if (HerosDirector.Instance.HeroIDChoosing != -1)
				{
					OnHandleInput_ClickMapToMoveHero(inputData);
				}
				else if (HerosDirector.Instance.HeroSkillIDChoosing != -1)
				{
					OnHandleInput_ClickMapToCastSkill(inputData);
				}
				else if (MonoSingleton<PowerUpItemDialogHandler>.Instance.selectingItemID >= 0)
				{
					OnHandleInput_ClickMapToCastPowerupItem(inputData);
				}
				else if (MonoSingleton<GameRecord>.Instance.RecordingPosition)
				{
					OnHandleInput_AssignAllyPosition(inputData);
				}
				else if (inputData.CompareTag(inputData.entityHit, GeneralVariable.GeneralDefine.DROPPED_GOLD))
				{
					OnHandleInput_CollectDroppedGold(inputData);
				}
				else if (inputData.CompareTag(inputData.entityHit, GeneralVariable.GeneralDefine.HERO_TAG))
				{
					OnHandleInput_ClickSelectHero(inputData);
				}
				else if (inputData.CompareTag(inputData.entityHit, GeneralVariable.GeneralDefine.BUILD_REGION_TAG))
				{
					OnHandleInput_ClickEmptyBuildArea(inputData);
				}
				else if (inputData.CompareTag(inputData.entityHit, GeneralVariable.GeneralDefine.TOWER_TAG))
				{
					OnHandleInput_ClickTower(inputData);
				}
				else if (inputData.CompareTag(inputData.entityHit, GeneralVariable.GeneralDefine.ENEMY_TAG))
				{
					OnHandleInput_ClickSelectEnemy(inputData);
				}
				else if (inputData.CompareTag(inputData.entityHit, GeneralVariable.GeneralDefine.PET_TAG))
				{
					OnHandleInput_ClickSelectPet(inputData);
				}
				else if (inputData.CompareTag(inputData.entityHit, GeneralVariable.GeneralDefine.ALLY_TAG))
				{
					OnHandleInput_ClickSelectAlly(inputData);
				}
			}
		}

		public void OnHandleInput_ClickMapToMoveHero(TapInputRecord inputData)
		{
			bool flag = inputData.CompareTag(inputData.mapHit, GeneralVariable.GeneralDefine.UN_MOVEABLE_TAG);
			if (!flag)
			{
				HerosDirector.Instance.MoveHeroToAssignedPosition(HerosDirector.Instance.HeroIDChoosing, getTargetVector());
			}
			if (effectCaster)
			{
				if (flag)
				{
					effectCaster.CastEffect(FXPool.ICON_UNMOVEABLE, 1f, getTargetVector());
				}
				else
				{
					effectCaster.CastEffect(FXPool.ICON_MOVEABLE_HERO, 2f, getTargetVector());
				}
			}
			HerosDirector.Instance.UnChooseHero(HerosDirector.Instance.HeroIDChoosing);
		}

		public void OnHandleInput_ClickMapToCastSkill(TapInputRecord inputData)
		{
			if (!inputData.CompareTag(inputData.mapHit, GeneralVariable.GeneralDefine.UN_MOVEABLE_TAG))
			{
				HerosDirector.Instance.CastHeroSkillToAssignedPosition(HerosDirector.Instance.HeroSkillIDChoosing, getTargetVector());
			}
			else if (effectCaster)
			{
				effectCaster.CastEffect(FXPool.ICON_UNMOVEABLE, 1f, getTargetVector());
			}
			HerosDirector.Instance.UnChooseHeroSkill(HerosDirector.Instance.HeroSkillIDChoosing);
		}

		public void OnHandleInput_ClickMapToCastPowerupItem(TapInputRecord inputData)
		{
			if (!inputData.CompareTag(inputData.mapHit, GeneralVariable.GeneralDefine.UN_MOVEABLE_TAG))
			{
				MonoSingleton<PowerUpItemDialogHandler>.Instance.CastItemSkill();
			}
		}

		public void OnHandleInput_AssignAllyPosition(TapInputRecord inputData)
		{
			bool flag = inputData.CompareTag(inputData.mapHit, GeneralVariable.GeneralDefine.ROAD_TAG);
			if (flag)
			{
				MonoSingleton<MinionsDirector>.Instance.MoveAlliesToAssignedPosition(MonoSingleton<GameRecord>.Instance.CurrentTowerSelected, getTargetVector());
			}
			if (effectCaster && !flag)
			{
				effectCaster.CastEffect(FXPool.ICON_UNMOVEABLE, 1f, getTargetVector());
			}
			MonoSingleton<MinionsDirector>.Instance.UnChooseTower(MonoSingleton<GameRecord>.Instance.CurrentTowerSelected);
		}

		public void OnHandleInput_CollectDroppedGold(TapInputRecord inputData)
		{
			inputData.entityHit.collider.gameObject.GetComponent<ProducedBullionHandler>().TapOnGold();
		}

		public void OnHandleInput_ClickEmptyBuildArea(TapInputRecord inputData)
		{
			inputData.entityHit.transform.gameObject.GetComponent<ConstructSectorHandler>().TryToClick();
			if (onClickBuildRegion != null)
			{
				onClickBuildRegion();
			}
		}

		public void OnHandleInput_ClickTower(TapInputRecord inputData)
		{
			inputData.entityHit.transform.gameObject.GetComponent<TurretEntity>().ChooseTower();
		}

		public void OnHandleInput_ClickSelectHero(TapInputRecord inputData)
		{
			HeroEntity component = inputData.entityHit.collider.GetComponent<HeroEntity>();
			if (component != null)
			{
				GameSignalCenter.Instance.Trigger(GameSignalKind.OnSelectHero, component.HeroID);
			}
		}

		public void OnHandleInput_ClickSelectEnemy(TapInputRecord inputData)
		{
			EnemyData component = inputData.entityHit.collider.GetComponent<EnemyData>();
			component.OnSelected();
			if (component != null)
			{
				GameSignalCenter.Instance.Trigger(GameSignalKind.OnSelectEnemy, component.OriginalParameter.id);
			}
		}

		public void OnHandleInput_ClickSelectPet(TapInputRecord inputData)
		{
			HeroEntity component = inputData.entityHit.collider.GetComponent<HeroEntity>();
			if (component != null)
			{
				GameSignalCenter.Instance.Trigger(GameSignalKind.OnSelectPet, component.HeroID);
			}
		}

		public void OnHandleInput_ClickSelectAlly(TapInputRecord inputData)
		{
			MinionEntity component = inputData.entityHit.collider.GetComponent<MinionEntity>();
			if (component != null)
			{
				GameSignalCenter.Instance.Trigger(GameSignalKind.OnSelectAlly, component);
			}
		}
	}
}
