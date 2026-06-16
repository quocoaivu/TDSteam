using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneLoader : MonoBehaviour
{
	public const string GameplaySceneName = "Gameplay";

	public const string WorldMapSceneName = "WorldMap";

	public const string MainMenuSceneName = "MainMenu";

	public static GameSceneLoader Instance { get; private set; }

	private void Awake()
	{
		if (GameSceneLoader.Instance)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		GameSceneLoader.Instance = this;
		base.transform.SetParent(null);
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	public void LoadScene(string levelName)
	{
		base.StartCoroutine(DoLoad(levelName));
	}

	private IEnumerator DoLoad(string levelName)
	{
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(levelName);
		yield return asyncLoad;
	}

	public void ReloadGameplayScene()
	{
		LoadScene(GameSceneLoader.GameplaySceneName);
	}

	public string GetCurrentSceneName()
	{
		return SceneManager.GetActiveScene().name;
	}
}
