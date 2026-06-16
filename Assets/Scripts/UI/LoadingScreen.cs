using System.Collections;
using DG.Tweening;
using MetaGame;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField]
    private GameObject leftDoor;

    [SerializeField]
    private GameObject rightDoor;

    [SerializeField]
    private GameObject leftLightning;

    [SerializeField]
    private GameObject rightLightning;

    [SerializeField]
    [FormerlySerializedAs("timeToClose")]
    private float doorCloseDuration;

    [SerializeField]
    [FormerlySerializedAs("timeToOpen")]
    private float doorOpenDuration;

    private bool _isLoading;

    [SerializeField]
    [FormerlySerializedAs("showTextTips")]
    private TextTipsView textTips;

    public bool IsLoading
	{
		get
		{
			return _isLoading;
		}
		set
		{
			_isLoading = value;
		}
	}

	public static LoadingScreen Instance { get; private set; }

	private void Awake()
	{
		if (LoadingScreen.Instance)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		LoadingScreen.Instance = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		IsLoading = false;
		SceneManager.sceneLoaded += OnSceneLoaded;
		if (textTips != null)
		{
			textTips.DisableTextTip();
		}
		Screen.sleepTimeout = -1;
	}

	private void OnDestroy()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
		if (LoadingScreen.Instance == this)
		{
			LoadingScreen.Instance = null;
		}
	}

	public void LoadSceneCompleted()
	{
		if (CrossSceneData.Instance.OpenSceneCount > 0)
		{
			Resources.UnloadUnusedAssets();
			base.StartCoroutine(ReloadCompleteRoutine());
		}
		CrossSceneData.Instance.IncreaseOpenSceneCount();
	}

	private void OnSceneLoaded(Scene loadedScene, LoadSceneMode loadMode)
	{
		if (GameSceneLoader.Instance.GetCurrentSceneName() != GameSceneLoader.GameplaySceneName)
		{
			LoadSceneCompleted();
		}
	}

	public void ShowLoading()
	{
		if (IsLoading)
		{
			return;
		}
		base.StopAllCoroutines();
		leftDoor.transform.DORestart(true);
		leftDoor.transform.DOLocalMoveX(-480f, doorCloseDuration, false).SetEase(Ease.OutQuad);
		rightDoor.transform.DORestart(true);
		rightDoor.transform.DOLocalMoveX(480f, doorCloseDuration, false).SetEase(Ease.OutQuad).OnComplete(new TweenCallback(OnDoorsClosed));
		IsLoading = true;
		UISfxDirector.Instance.PlayOpenLoading();
	}

	private void OnDoorsClosed()
	{
		ShowLightning();
		if (textTips != null)
		{
			textTips.EnableTextTip();
			textTips.GetContentTextTip();
			textTips.ShowContentTextTip();
		}
	}

	public void HideLoading()
	{
		if (!IsLoading)
		{
			return;
		}
		leftDoor.transform.DOLocalMoveX(-1450f, doorOpenDuration, false).SetEase(Ease.InCubic);
		rightDoor.transform.DOLocalMoveX(1450f, doorOpenDuration, false).SetEase(Ease.InCubic).OnComplete(new TweenCallback(OnDoorsOpened));
		UISfxDirector.Instance.PlayCloseLoading();
	}

	private void OnDoorsOpened()
	{
		IsLoading = false;
	}

	private void ShowLightning()
	{
		leftLightning.SetActive(true);
		rightLightning.SetActive(true);
	}

	private void HideLightning()
	{
		leftLightning.SetActive(false);
		rightLightning.SetActive(false);
	}

	private IEnumerator ReloadCompleteRoutine()
	{
		yield return new WaitForEndOfFrame();
		HideLoading();
		HideLightning();
		if (textTips != null)
		{
			textTips.DisableTextTip();
		}
		yield break;
	}


}
