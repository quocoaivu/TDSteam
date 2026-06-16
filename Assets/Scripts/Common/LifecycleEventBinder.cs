using System;
using Common;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Common
{
	public class LifecycleEventBinder : MonoBehaviour
	{
        [Header("Activation")]
        [SerializeField]
        private OrderedUnityEvent onAwake = new OrderedUnityEvent();

        [SerializeField]
        private OrderedUnityEvent onEnable = new OrderedUnityEvent();

        [SerializeField]
        private OrderedUnityEvent onStart = new OrderedUnityEvent();

        [SerializeField]
        private OrderedUnityEvent onDisable = new OrderedUnityEvent();

        [Header("Touch input")]
        [SerializeField]
        private OrderedUnityEvent onMouseDown = new OrderedUnityEvent();

        [SerializeField]
        private OrderedUnityEvent onMouseUp = new OrderedUnityEvent();

        [SerializeField]
        private OrderedUnityEvent onMouseUpAsButton = new OrderedUnityEvent();

        [Header("Rendering")]
        [SerializeField]
        private OrderedUnityEvent onVisible = new OrderedUnityEvent();

        [SerializeField]
        private OrderedUnityEvent onInvisible = new OrderedUnityEvent();

        [Header("Others")]
        [SerializeField]
        private OrderedUnityEvent onSceneLoaded = new OrderedUnityEvent();

        [Header("Customized")]
        [SerializeField]
        private OrderedUnityEvent onOneTimeFixedUpdate = new OrderedUnityEvent();

        [SerializeField]
        private OrderedUnityEvent onOneTimeUpdate = new OrderedUnityEvent();

        [SerializeField]
        private OrderedUnityEvent onOneTimeLateUpdate = new OrderedUnityEvent();

        private bool isOneTimeFixedUpdateCalled;

        private bool isOneTimeUpdateCalled;

        private bool isOneTimeLateUpdateCalled;

        public void Awake()
		{
			onAwake.Dispatch();
		}

		public void OnEnable()
		{
			onEnable.Dispatch();
			SceneManager.sceneLoaded += OnSceneLoadedHandler;
		}

		public void Start()
		{
			onStart.Dispatch();
		}

		public void OnDisable()
		{
			onDisable.Dispatch();
			SceneManager.sceneLoaded -= OnSceneLoadedHandler;
		}

		public void OnMouseDown()
		{
			onMouseDown.Dispatch();
		}

		public void OnMouseUp()
		{
			onMouseUp.Dispatch();
		}

		public void OnMouseUpAsButton()
		{
			onMouseUpAsButton.Dispatch();
		}

		public void OnBecameInvisible()
		{
			onInvisible.Dispatch();
		}

		public void OnBecameVisible()
		{
			onVisible.Dispatch();
		}

		private void OnSceneLoadedHandler(Scene scene, LoadSceneMode mode)
		{
			onSceneLoaded.Dispatch();
		}

		public void LateUpdate()
		{
			if (!isOneTimeLateUpdateCalled)
			{
				isOneTimeLateUpdateCalled = true;
				onOneTimeLateUpdate.Dispatch();
			}
		}

		public void Update()
		{
			if (!isOneTimeUpdateCalled)
			{
				isOneTimeUpdateCalled = true;
				onOneTimeUpdate.Dispatch();
			}
		}

		public void FixedUpdate()
		{
			if (!isOneTimeFixedUpdateCalled)
			{
				isOneTimeFixedUpdateCalled = true;
				onOneTimeFixedUpdate.Dispatch();
			}
		}


	}
}
