using UnityEngine;
using UnityEngine.InputSystem;

namespace MainMenu
{
	public class UIRootHandler : MonoSingleton<UIRootHandler>
	{
        [Space]
        [Header("Prefab Panels")]
        [SerializeField]
        private GameObject languageChoosePrefab;

        [SerializeField]
        private GameObject creditPrefab;

        [SerializeField]
        private GameObject policyPrefab;

        [SerializeField]
        private GameObject askToQuitPrefab;

        [Header("Button Language")]
        [SerializeField]
        private TongueSwitchHandler languageButtonController;

        private TongueChooseBoardHandler _languageChoosePanelController;

        private CreditDialogHandler _creditPopupController;

        private PolicyDialogHandler _policyPopupController;

        private AskToExitDialogHandler _askToQuitPopupController;

        // Esc/back: event-driven thay vÃ¬ poll trong Update.
        private InputAction _backAction;

        public TongueSwitchHandler LanguageButtonController
		{
			get
			{
				return languageButtonController;
			}
			set
			{
				languageButtonController = value;
			}
		}

		public TongueChooseBoardHandler LanguageChoosePanelController
		{
			get
			{
				if (_languageChoosePanelController == null)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(languageChoosePrefab);
					gameObject.transform.SetParent(base.transform);
					gameObject.transform.localPosition = Vector3.zero;
					gameObject.transform.localScale = Vector3.one;
					_languageChoosePanelController = gameObject.GetComponent<TongueChooseBoardHandler>();
				}
				return _languageChoosePanelController;
			}
			set
			{
				_languageChoosePanelController = value;
			}
		}

		public CreditDialogHandler CreditPopupController
		{
			get
			{
				if (_creditPopupController == null)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(creditPrefab);
					gameObject.transform.SetParent(base.transform);
					gameObject.transform.localPosition = Vector3.zero;
					gameObject.transform.localScale = Vector3.one;
					_creditPopupController = gameObject.GetComponent<CreditDialogHandler>();
				}
				return _creditPopupController;
			}
			set
			{
				_creditPopupController = value;
			}
		}

		public PolicyDialogHandler PolicyPopupController
		{
			get
			{
				if (_policyPopupController == null)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(policyPrefab);
					gameObject.transform.SetParent(base.transform);
					gameObject.transform.localPosition = Vector3.zero;
					gameObject.transform.localScale = Vector3.one;
					_policyPopupController = gameObject.GetComponent<PolicyDialogHandler>();
				}
				return _policyPopupController;
			}
			set
			{
				_policyPopupController = value;
			}
		}

		public AskToExitDialogHandler AskToExitDialogHandler
		{
			get
			{
				if (_askToQuitPopupController == null)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(askToQuitPrefab);
					gameObject.transform.SetParent(base.transform);
					gameObject.transform.localPosition = Vector3.zero;
					gameObject.transform.localScale = Vector3.one;
					_askToQuitPopupController = gameObject.GetComponent<AskToExitDialogHandler>();
				}
				return _askToQuitPopupController;
			}
			set
			{
				_askToQuitPopupController = value;
			}
		}

		private void OnEnable()
		{
			_backAction = new InputAction("Back", InputActionType.Button, "<Keyboard>/escape");
			// canceled = nháº£ phÃ­m, khá»›p wasReleasedThisFrame cÅ©.
			_backAction.canceled += OnBackReleased;
			_backAction.Enable();
		}

		private void OnDisable()
		{
			if (_backAction != null)
			{
				_backAction.canceled -= OnBackReleased;
				_backAction.Disable();
				_backAction.Dispose();
				_backAction = null;
			}
		}

		private void OnBackReleased(InputAction.CallbackContext context)
		{
			HandleBack();
		}

		private void HandleBack()
		{
			// Äá»c tháº³ng backing field Ä‘á»ƒ khÃ´ng kÃ­ch hoáº¡t getter táº¡o prefab.
			if (_languageChoosePanelController != null && _languageChoosePanelController.isOpen)
			{
				_languageChoosePanelController.Close();
				return;
			}
			if (_policyPopupController != null && _policyPopupController.isOpen)
			{
				_policyPopupController.Close();
				return;
			}
			if (_creditPopupController != null && _creditPopupController.isOpen)
			{
				_creditPopupController.Close();
				return;
			}
			// KhÃ´ng cÃ³ popup nÃ o má»Ÿ -> má»Ÿ popup thoÃ¡t (getter táº¡o prefab náº¿u chÆ°a cÃ³).
			if (_askToQuitPopupController == null || !_askToQuitPopupController.isOpen)
			{
				AskToExitDialogHandler.Init();
				return;
			}
			_askToQuitPopupController.Close();
		}
	}
}
