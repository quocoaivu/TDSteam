using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class Reporter : MonoBehaviour
{
	public float TotalMemUsage
	{
		get
		{
			return logsMemUsage + graphMemUsage;
		}
	}

	private void Awake()
	{
		if (!Initialized)
		{
			Initialize();
		}
	}

	private void OnEnable()
	{
		if (logs.Count == 0)
		{
			clear();
		}
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	private void OnDisable()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		if (clearOnNewSceneLoaded)
		{
			clear();
		}
		currentScene = scene.name;
		UnityEngine.Debug.Log("Scene " + scene.name + " is loaded");
	}

	private void addSample()
	{
		Reporter.Sample sample = new Reporter.Sample();
		sample.fps = fps;
		sample.fpsText = fpsText;
		sample.loadedScene = (byte)SceneManager.GetActiveScene().buildIndex;
		sample.time = Time.realtimeSinceStartup;
		sample.memory = gcTotalMemory;
		samples.Add(sample);
		graphMemUsage = (float)samples.Count * Reporter.Sample.MemSize() / 1024f / 1024f;
	}

	public void Initialize()
	{
		if (!Reporter.created)
		{
			try
			{
				base.gameObject.SendMessage("OnPreStart");
			}
			catch (Exception exception)
			{
				UnityEngine.Debug.LogException(exception);
			}
			Reporter.scenes = new string[SceneManager.sceneCountInBuildSettings];
			currentScene = SceneManager.GetActiveScene().name;
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			Application.logMessageReceivedThreaded += CaptureLogThread;
			Reporter.created = true;
			clearContent = new GUIContent(string.Empty, images.clearImage, "Clear logs");
			collapseContent = new GUIContent(string.Empty, images.collapseImage, "Collapse logs");
			clearOnNewSceneContent = new GUIContent(string.Empty, images.clearOnNewSceneImage, "Clear logs on new scene loaded");
			showTimeContent = new GUIContent(string.Empty, images.showTimeImage, "Show Hide Time");
			showSceneContent = new GUIContent(string.Empty, images.showSceneImage, "Show Hide Scene");
			showMemoryContent = new GUIContent(string.Empty, images.showMemoryImage, "Show Hide Memory");
			softwareContent = new GUIContent(string.Empty, images.softwareImage, "Software");
			dateContent = new GUIContent(string.Empty, images.dateImage, "Date");
			showFpsContent = new GUIContent(string.Empty, images.showFpsImage, "Show Hide fps");
			infoContent = new GUIContent(string.Empty, images.infoImage, "Information about application");
			searchContent = new GUIContent(string.Empty, images.searchImage, "Search for logs");
			closeContent = new GUIContent(string.Empty, images.closeImage, "Hide logs");
			userContent = new GUIContent(string.Empty, images.userImage, "User");
			buildFromContent = new GUIContent(string.Empty, images.buildFromImage, "Build From");
			systemInfoContent = new GUIContent(string.Empty, images.systemInfoImage, "System Info");
			graphicsInfoContent = new GUIContent(string.Empty, images.graphicsInfoImage, "Graphics Info");
			backContent = new GUIContent(string.Empty, images.backImage, "Back");
			logContent = new GUIContent(string.Empty, images.logImage, "show or hide logs");
			warningContent = new GUIContent(string.Empty, images.warningImage, "show or hide warnings");
			errorContent = new GUIContent(string.Empty, images.errorImage, "show or hide errors");
			currentView = (Reporter.ReportView)PlayerPrefs.GetInt("Reporter_currentView", 1);
			show = (PlayerPrefs.GetInt("Reporter_show") == 1);
			collapse = (PlayerPrefs.GetInt("Reporter_collapse") == 1);
			clearOnNewSceneLoaded = (PlayerPrefs.GetInt("Reporter_clearOnNewSceneLoaded") == 1);
			showTime = (PlayerPrefs.GetInt("Reporter_showTime") == 1);
			showScene = (PlayerPrefs.GetInt("Reporter_showScene") == 1);
			showMemory = (PlayerPrefs.GetInt("Reporter_showMemory") == 1);
			showFps = (PlayerPrefs.GetInt("Reporter_showFps") == 1);
			showGraph = (PlayerPrefs.GetInt("Reporter_showGraph") == 1);
			showLog = (PlayerPrefs.GetInt("Reporter_showLog", 1) == 1);
			showWarning = (PlayerPrefs.GetInt("Reporter_showWarning", 1) == 1);
			showError = (PlayerPrefs.GetInt("Reporter_showError", 1) == 1);
			filterText = PlayerPrefs.GetString("Reporter_filterText");
			size.x = (size.y = PlayerPrefs.GetFloat("Reporter_size", 32f));
			showClearOnNewSceneLoadedButton = (PlayerPrefs.GetInt("Reporter_showClearOnNewSceneLoadedButton", 1) == 1);
			showTimeButton = (PlayerPrefs.GetInt("Reporter_showTimeButton", 1) == 1);
			showSceneButton = (PlayerPrefs.GetInt("Reporter_showSceneButton", 1) == 1);
			showMemButton = (PlayerPrefs.GetInt("Reporter_showMemButton", 1) == 1);
			showFpsButton = (PlayerPrefs.GetInt("Reporter_showFpsButton", 1) == 1);
			showSearchText = (PlayerPrefs.GetInt("Reporter_showSearchText", 1) == 1);
			initializeStyle();
			Initialized = true;
			if (show)
			{
				doShow();
			}
			deviceModel = SystemInfo.deviceModel.ToString();
			deviceType = SystemInfo.deviceType.ToString();
			deviceName = SystemInfo.deviceName.ToString();
			graphicsMemorySize = SystemInfo.graphicsMemorySize.ToString();
			maxTextureSize = SystemInfo.maxTextureSize.ToString();
			systemMemorySize = SystemInfo.systemMemorySize.ToString();
			return;
		}
		UnityEngine.Debug.LogWarning("tow manager is exists delete the second");
		UnityEngine.Object.DestroyImmediate(base.gameObject, true);
	}

	private void initializeStyle()
	{
		int num = (int)(size.x * 0.2f);
		int num2 = (int)(size.y * 0.2f);
		nonStyle = new GUIStyle();
		nonStyle.clipping = TextClipping.Clip;
		nonStyle.border = new RectOffset(0, 0, 0, 0);
		nonStyle.normal.background = null;
		nonStyle.fontSize = (int)(size.y / 2f);
		nonStyle.alignment = TextAnchor.MiddleCenter;
		lowerLeftFontStyle = new GUIStyle();
		lowerLeftFontStyle.clipping = TextClipping.Clip;
		lowerLeftFontStyle.border = new RectOffset(0, 0, 0, 0);
		lowerLeftFontStyle.normal.background = null;
		lowerLeftFontStyle.fontSize = (int)(size.y / 2f);
		lowerLeftFontStyle.fontStyle = FontStyle.Bold;
		lowerLeftFontStyle.alignment = TextAnchor.LowerLeft;
		barStyle = new GUIStyle();
		barStyle.border = new RectOffset(1, 1, 1, 1);
		barStyle.normal.background = images.barImage;
		barStyle.active.background = images.button_activeImage;
		barStyle.alignment = TextAnchor.MiddleCenter;
		barStyle.margin = new RectOffset(1, 1, 1, 1);
		barStyle.clipping = TextClipping.Clip;
		barStyle.fontSize = (int)(size.y / 2f);
		buttonActiveStyle = new GUIStyle();
		buttonActiveStyle.border = new RectOffset(1, 1, 1, 1);
		buttonActiveStyle.normal.background = images.button_activeImage;
		buttonActiveStyle.alignment = TextAnchor.MiddleCenter;
		buttonActiveStyle.margin = new RectOffset(1, 1, 1, 1);
		buttonActiveStyle.fontSize = (int)(size.y / 2f);
		backStyle = new GUIStyle();
		backStyle.normal.background = images.even_logImage;
		backStyle.clipping = TextClipping.Clip;
		backStyle.fontSize = (int)(size.y / 2f);
		evenLogStyle = new GUIStyle();
		evenLogStyle.normal.background = images.even_logImage;
		evenLogStyle.fixedHeight = size.y;
		evenLogStyle.clipping = TextClipping.Clip;
		evenLogStyle.alignment = TextAnchor.UpperLeft;
		evenLogStyle.imagePosition = ImagePosition.ImageLeft;
		evenLogStyle.fontSize = (int)(size.y / 2f);
		oddLogStyle = new GUIStyle();
		oddLogStyle.normal.background = images.odd_logImage;
		oddLogStyle.fixedHeight = size.y;
		oddLogStyle.clipping = TextClipping.Clip;
		oddLogStyle.alignment = TextAnchor.UpperLeft;
		oddLogStyle.imagePosition = ImagePosition.ImageLeft;
		oddLogStyle.fontSize = (int)(size.y / 2f);
		logButtonStyle = new GUIStyle();
		logButtonStyle.fixedHeight = size.y;
		logButtonStyle.clipping = TextClipping.Clip;
		logButtonStyle.alignment = TextAnchor.UpperLeft;
		logButtonStyle.fontSize = (int)(size.y / 2f);
		logButtonStyle.padding = new RectOffset(num, num, num2, num2);
		selectedLogStyle = new GUIStyle();
		selectedLogStyle.normal.background = images.selectedImage;
		selectedLogStyle.fixedHeight = size.y;
		selectedLogStyle.clipping = TextClipping.Clip;
		selectedLogStyle.alignment = TextAnchor.UpperLeft;
		selectedLogStyle.normal.textColor = Color.white;
		selectedLogStyle.fontSize = (int)(size.y / 2f);
		selectedLogFontStyle = new GUIStyle();
		selectedLogFontStyle.normal.background = images.selectedImage;
		selectedLogFontStyle.fixedHeight = size.y;
		selectedLogFontStyle.clipping = TextClipping.Clip;
		selectedLogFontStyle.alignment = TextAnchor.UpperLeft;
		selectedLogFontStyle.normal.textColor = Color.white;
		selectedLogFontStyle.fontSize = (int)(size.y / 2f);
		selectedLogFontStyle.padding = new RectOffset(num, num, num2, num2);
		stackLabelStyle = new GUIStyle();
		stackLabelStyle.wordWrap = true;
		stackLabelStyle.fontSize = (int)(size.y / 2f);
		stackLabelStyle.padding = new RectOffset(num, num, num2, num2);
		scrollerStyle = new GUIStyle();
		scrollerStyle.normal.background = images.barImage;
		searchStyle = new GUIStyle();
		searchStyle.clipping = TextClipping.Clip;
		searchStyle.alignment = TextAnchor.LowerCenter;
		searchStyle.fontSize = (int)(size.y / 2f);
		searchStyle.wordWrap = true;
		sliderBackStyle = new GUIStyle();
		sliderBackStyle.normal.background = images.barImage;
		sliderBackStyle.fixedHeight = size.y;
		sliderBackStyle.border = new RectOffset(1, 1, 1, 1);
		sliderThumbStyle = new GUIStyle();
		sliderThumbStyle.normal.background = images.selectedImage;
		sliderThumbStyle.fixedWidth = size.x;
		GUISkin reporterScrollerSkin = images.reporterScrollerSkin;
		toolbarScrollerSkin = UnityEngine.Object.Instantiate<GUISkin>(reporterScrollerSkin);
		toolbarScrollerSkin.verticalScrollbar.fixedWidth = 0f;
		toolbarScrollerSkin.horizontalScrollbar.fixedHeight = 0f;
		toolbarScrollerSkin.verticalScrollbarThumb.fixedWidth = 0f;
		toolbarScrollerSkin.horizontalScrollbarThumb.fixedHeight = 0f;
		logScrollerSkin = UnityEngine.Object.Instantiate<GUISkin>(reporterScrollerSkin);
		logScrollerSkin.verticalScrollbar.fixedWidth = size.x * 2f;
		logScrollerSkin.horizontalScrollbar.fixedHeight = 0f;
		logScrollerSkin.verticalScrollbarThumb.fixedWidth = size.x * 2f;
		logScrollerSkin.horizontalScrollbarThumb.fixedHeight = 0f;
		graphScrollerSkin = UnityEngine.Object.Instantiate<GUISkin>(reporterScrollerSkin);
		graphScrollerSkin.verticalScrollbar.fixedWidth = 0f;
		graphScrollerSkin.horizontalScrollbar.fixedHeight = size.x * 2f;
		graphScrollerSkin.verticalScrollbarThumb.fixedWidth = 0f;
		graphScrollerSkin.horizontalScrollbarThumb.fixedHeight = size.x * 2f;
	}

	private void Start()
	{
		logDate = DateTime.Now.ToString();
		base.StartCoroutine("readInfo");
	}

	private void clear()
	{
		logs.Clear();
		collapsedLogs.Clear();
		currentLog.Clear();
		logsDic.Clear();
		selectedLog = null;
		numOfLogs = 0;
		numOfLogsWarning = 0;
		numOfLogsError = 0;
		numOfCollapsedLogs = 0;
		numOfCollapsedLogsWarning = 0;
		numOfCollapsedLogsError = 0;
		logsMemUsage = 0f;
		graphMemUsage = 0f;
		samples.Clear();
		GC.Collect();
		selectedLog = null;
	}

	private void calculateCurrentLog()
	{
		bool flag = !string.IsNullOrEmpty(filterText);
		string value = string.Empty;
		if (flag)
		{
			value = filterText.ToLower();
		}
		currentLog.Clear();
		if (collapse)
		{
			for (int i = 0; i < collapsedLogs.Count; i++)
			{
				Reporter.Log log = collapsedLogs[i];
				if (log.logType != Reporter._LogType.Log || showLog)
				{
					if (log.logType != Reporter._LogType.Warning || showWarning)
					{
						if (log.logType != Reporter._LogType.Error || showError)
						{
							if (log.logType != Reporter._LogType.Assert || showError)
							{
								if (log.logType != Reporter._LogType.Exception || showError)
								{
									if (flag)
									{
										if (log.condition.ToLower().Contains(value))
										{
											currentLog.Add(log);
										}
									}
									else
									{
										currentLog.Add(log);
									}
								}
							}
						}
					}
				}
			}
		}
		else
		{
			for (int j = 0; j < logs.Count; j++)
			{
				Reporter.Log log2 = logs[j];
				if (log2.logType != Reporter._LogType.Log || showLog)
				{
					if (log2.logType != Reporter._LogType.Warning || showWarning)
					{
						if (log2.logType != Reporter._LogType.Error || showError)
						{
							if (log2.logType != Reporter._LogType.Assert || showError)
							{
								if (log2.logType != Reporter._LogType.Exception || showError)
								{
									if (flag)
									{
										if (log2.condition.ToLower().Contains(value))
										{
											currentLog.Add(log2);
										}
									}
									else
									{
										currentLog.Add(log2);
									}
								}
							}
						}
					}
				}
			}
		}
		if (selectedLog != null)
		{
			int num = currentLog.IndexOf(selectedLog);
			if (num == -1)
			{
				Reporter.Log item = logsDic[selectedLog.condition][selectedLog.stacktrace];
				num = currentLog.IndexOf(item);
				if (num != -1)
				{
					scrollPosition.y = (float)num * size.y;
				}
			}
			else
			{
				scrollPosition.y = (float)num * size.y;
			}
		}
	}

	private void DrawInfo()
	{
		GUILayout.BeginArea(screenRect, backStyle);
		Vector2 drag = getDrag();
		if (drag.x != 0f && downPos != Vector2.zero)
		{
			infoScrollPosition.x = infoScrollPosition.x - (drag.x - oldInfoDrag.x);
		}
		if (drag.y != 0f && downPos != Vector2.zero)
		{
			infoScrollPosition.y = infoScrollPosition.y + (drag.y - oldInfoDrag.y);
		}
		oldInfoDrag = drag;
		GUI.skin = toolbarScrollerSkin;
		infoScrollPosition = GUILayout.BeginScrollView(infoScrollPosition, new GUILayoutOption[0]);
		GUILayout.Space(size.x);
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Space(size.x);
		GUILayout.Box(buildFromContent, nonStyle, new GUILayoutOption[]
		{
			GUILayout.Width(size.x),
			GUILayout.Height(size.y)
		});
		GUILayout.Space(size.x);
		GUILayout.Label(buildDate, nonStyle, new GUILayoutOption[]
		{
			GUILayout.Height(size.y)
		});
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Space(size.x);
		GUILayout.Box(systemInfoContent, nonStyle, new GUILayoutOption[]
		{
			GUILayout.Width(size.x),
			GUILayout.Height(size.y)
		});
		GUILayout.Space(size.x);
		GUILayout.Label(deviceModel, nonStyle, new GUILayoutOption[]
		{
			GUILayout.Height(size.y)
		});
		GUILayout.Space(size.x);
		GUILayout.Label(deviceType, nonStyle, new GUILayoutOption[]
		{
			GUILayout.Height(size.y)
		});
		GUILayout.Space(size.x);
		GUILayout.Label(deviceName, nonStyle, new GUILayoutOption[]
		{
			GUILayout.Height(size.y)
		});
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Space(size.x);
		GUILayout.Box(graphicsInfoContent, nonStyle, new GUILayoutOption[]
		{
			GUILayout.Width(size.x),
			GUILayout.Height(size.y)
		});
		GUILayout.Space(size.x);
		GUILayout.Label(SystemInfo.graphicsDeviceName, nonStyle, new GUILayoutOption[]
		{
			GUILayout.Height(size.y)
		});
		GUILayout.Space(size.x);
		GUILayout.Label(graphicsMemorySize, nonStyle, new GUILayoutOption[]
		{
			GUILayout.Height(size.y)
		});
		GUILayout.Space(size.x);
		GUILayout.Label(maxTextureSize, nonStyle, new GUILayoutOption[]
		{
			GUILayout.Height(size.y)
		});
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Space(size.x);
		GUILayout.Space(size.x);
		GUILayout.Space(size.x);
		GUILayout.Label("Screen Width " + Screen.width, nonStyle, new GUILayoutOption[]
		{
			GUILayout.Height(size.y)
		});
		GUILayout.Space(size.x);
		GUILayout.Label("Screen Height " + Screen.height, nonStyle, new GUILayoutOption[]
		{
			GUILayout.Height(size.y)
		});
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Space(size.x);
		GUILayout.Box(showMemoryContent, nonStyle, new GUILayoutOption[]
		{
			GUILayout.Width(size.x),
			GUILayout.Height(size.y)
		});
		GUILayout.Space(size.x);
		GUILayout.Label(systemMemorySize + " mb", nonStyle, new GUILayoutOption[]
		{
			GUILayout.Height(size.y)
		});
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Space(size.x);
		GUILayout.Space(size.x);
		GUILayout.Space(size.x);
		GUILayout.Label("Mem Usage Of Logs " + logsMemUsage.ToString("0.000") + " mb", nonStyle, new GUILayoutOption[]
		{
			GUILayout.Height(size.y)
		});
		GUILayout.Space(size.x);
		GUILayout.Label("GC Memory " + gcTotalMemory.ToString("0.000") + " mb", nonStyle, new GUILayoutOption[]
		{
			GUILayout.Height(size.y)
		});
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Space(size.x);
		GUILayout.Box(softwareContent, nonStyle, new GUILayoutOption[]
		{
			GUILayout.Width(size.x),
			GUILayout.Height(size.y)
		});
		GUILayout.Space(size.x);
		GUILayout.Label(SystemInfo.operatingSystem, nonStyle, new GUILayoutOption[]
		{
			GUILayout.Height(size.y)
		});
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Space(size.x);
		GUILayout.Box(dateContent, nonStyle, new GUILayoutOption[]
		{
			GUILayout.Width(size.x),
			GUILayout.Height(size.y)
		});
		GUILayout.Space(size.x);
		GUILayout.Label(DateTime.Now.ToString(), nonStyle, new GUILayoutOption[]
		{
			GUILayout.Height(size.y)
		});
		GUILayout.Label(" - Application Started At " + logDate, nonStyle, new GUILayoutOption[]
		{
			GUILayout.Height(size.y)
		});
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Space(size.x);
		GUILayout.Box(showTimeContent, nonStyle, new GUILayoutOption[]
		{
			GUILayout.Width(size.x),
			GUILayout.Height(size.y)
		});
		GUILayout.Space(size.x);
		GUILayout.Label(Time.realtimeSinceStartup.ToString("000"), nonStyle, new GUILayoutOption[]
		{
			GUILayout.Height(size.y)
		});
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Space(size.x);
		GUILayout.Box(showFpsContent, nonStyle, new GUILayoutOption[]
		{
			GUILayout.Width(size.x),
			GUILayout.Height(size.y)
		});
		GUILayout.Space(size.x);
		GUILayout.Label(fpsText, nonStyle, new GUILayoutOption[]
		{
			GUILayout.Height(size.y)
		});
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Space(size.x);
		GUILayout.Box(userContent, nonStyle, new GUILayoutOption[]
		{
			GUILayout.Width(size.x),
			GUILayout.Height(size.y)
		});
		GUILayout.Space(size.x);
		GUILayout.Label(UserData, nonStyle, new GUILayoutOption[]
		{
			GUILayout.Height(size.y)
		});
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Space(size.x);
		GUILayout.Box(showSceneContent, nonStyle, new GUILayoutOption[]
		{
			GUILayout.Width(size.x),
			GUILayout.Height(size.y)
		});
		GUILayout.Space(size.x);
		GUILayout.Label(currentScene, nonStyle, new GUILayoutOption[]
		{
			GUILayout.Height(size.y)
		});
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Space(size.x);
		GUILayout.Box(showSceneContent, nonStyle, new GUILayoutOption[]
		{
			GUILayout.Width(size.x),
			GUILayout.Height(size.y)
		});
		GUILayout.Space(size.x);
		GUILayout.Label("Unity Version = " + Application.unityVersion, nonStyle, new GUILayoutOption[]
		{
			GUILayout.Height(size.y)
		});
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		drawInfo_enableDisableToolBarButtons();
		GUILayout.FlexibleSpace();
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Space(size.x);
		GUILayout.Label("Size = " + size.x.ToString("0.0"), nonStyle, new GUILayoutOption[]
		{
			GUILayout.Height(size.y)
		});
		GUILayout.Space(size.x);
		float num = GUILayout.HorizontalSlider(size.x, 16f, 64f, sliderBackStyle, sliderThumbStyle, new GUILayoutOption[]
		{
			GUILayout.Width((float)Screen.width * 0.5f)
		});
		if (size.x != num)
		{
			size.x = (size.y = num);
			initializeStyle();
		}
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Space(size.x);
		if (GUILayout.Button(backContent, barStyle, new GUILayoutOption[]
		{
			GUILayout.Width(size.x * 2f),
			GUILayout.Height(size.y * 2f)
		}))
		{
			currentView = Reporter.ReportView.Logs;
		}
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.EndScrollView();
		GUILayout.EndArea();
	}

	private void drawInfo_enableDisableToolBarButtons()
	{
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Space(size.x);
		GUILayout.Label("Hide or Show tool bar buttons", nonStyle, new GUILayoutOption[]
		{
			GUILayout.Height(size.y)
		});
		GUILayout.Space(size.x);
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Space(size.x);
		if (GUILayout.Button(clearOnNewSceneContent, (!showClearOnNewSceneLoadedButton) ? barStyle : buttonActiveStyle, new GUILayoutOption[]
		{
			GUILayout.Width(size.x * 2f),
			GUILayout.Height(size.y * 2f)
		}))
		{
			showClearOnNewSceneLoadedButton = !showClearOnNewSceneLoadedButton;
		}
		if (GUILayout.Button(showTimeContent, (!showTimeButton) ? barStyle : buttonActiveStyle, new GUILayoutOption[]
		{
			GUILayout.Width(size.x * 2f),
			GUILayout.Height(size.y * 2f)
		}))
		{
			showTimeButton = !showTimeButton;
		}
		tempRect = GUILayoutUtility.GetLastRect();
		GUI.Label(tempRect, Time.realtimeSinceStartup.ToString("0.0"), lowerLeftFontStyle);
		if (GUILayout.Button(showSceneContent, (!showSceneButton) ? barStyle : buttonActiveStyle, new GUILayoutOption[]
		{
			GUILayout.Width(size.x * 2f),
			GUILayout.Height(size.y * 2f)
		}))
		{
			showSceneButton = !showSceneButton;
		}
		tempRect = GUILayoutUtility.GetLastRect();
		GUI.Label(tempRect, currentScene, lowerLeftFontStyle);
		if (GUILayout.Button(showMemoryContent, (!showMemButton) ? barStyle : buttonActiveStyle, new GUILayoutOption[]
		{
			GUILayout.Width(size.x * 2f),
			GUILayout.Height(size.y * 2f)
		}))
		{
			showMemButton = !showMemButton;
		}
		tempRect = GUILayoutUtility.GetLastRect();
		GUI.Label(tempRect, gcTotalMemory.ToString("0.0"), lowerLeftFontStyle);
		if (GUILayout.Button(showFpsContent, (!showFpsButton) ? barStyle : buttonActiveStyle, new GUILayoutOption[]
		{
			GUILayout.Width(size.x * 2f),
			GUILayout.Height(size.y * 2f)
		}))
		{
			showFpsButton = !showFpsButton;
		}
		tempRect = GUILayoutUtility.GetLastRect();
		GUI.Label(tempRect, fpsText, lowerLeftFontStyle);
		if (GUILayout.Button(searchContent, (!showSearchText) ? barStyle : buttonActiveStyle, new GUILayoutOption[]
		{
			GUILayout.Width(size.x * 2f),
			GUILayout.Height(size.y * 2f)
		}))
		{
			showSearchText = !showSearchText;
		}
		tempRect = GUILayoutUtility.GetLastRect();
		GUI.TextField(tempRect, filterText, searchStyle);
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
	}

	private void DrawReport()
	{
		screenRect.x = 0f;
		screenRect.y = 0f;
		screenRect.width = (float)Screen.width;
		screenRect.height = (float)Screen.height;
		GUILayout.BeginArea(screenRect, backStyle);
		GUILayout.BeginVertical(new GUILayoutOption[0]);
		GUILayout.FlexibleSpace();
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.FlexibleSpace();
		GUILayout.Label("Select Photo", nonStyle, new GUILayoutOption[]
		{
			GUILayout.Height(size.y)
		});
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Label("Coming Soon", nonStyle, new GUILayoutOption[]
		{
			GUILayout.Height(size.y)
		});
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.FlexibleSpace();
		if (GUILayout.Button(backContent, barStyle, new GUILayoutOption[]
		{
			GUILayout.Width(size.x),
			GUILayout.Height(size.y)
		}))
		{
			currentView = Reporter.ReportView.Logs;
		}
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.EndVertical();
		GUILayout.EndArea();
	}

	private void drawToolBar()
	{
		toolBarRect.x = 0f;
		toolBarRect.y = 0f;
		toolBarRect.width = (float)Screen.width;
		toolBarRect.height = size.y * 2f;
		GUI.skin = toolbarScrollerSkin;
		Vector2 drag = getDrag();
		if (drag.x != 0f && downPos != Vector2.zero && downPos.y > (float)Screen.height - size.y * 2f)
		{
			toolbarScrollPosition.x = toolbarScrollPosition.x - (drag.x - toolbarOldDrag);
		}
		toolbarOldDrag = drag.x;
		GUILayout.BeginArea(toolBarRect);
		toolbarScrollPosition = GUILayout.BeginScrollView(toolbarScrollPosition, new GUILayoutOption[0]);
		GUILayout.BeginHorizontal(barStyle, new GUILayoutOption[0]);
		if (GUILayout.Button(clearContent, barStyle, new GUILayoutOption[]
		{
			GUILayout.Width(size.x * 2f),
			GUILayout.Height(size.y * 2f)
		}))
		{
			clear();
		}
		if (GUILayout.Button(collapseContent, (!collapse) ? barStyle : buttonActiveStyle, new GUILayoutOption[]
		{
			GUILayout.Width(size.x * 2f),
			GUILayout.Height(size.y * 2f)
		}))
		{
			collapse = !collapse;
			calculateCurrentLog();
		}
		if (showClearOnNewSceneLoadedButton && GUILayout.Button(clearOnNewSceneContent, (!clearOnNewSceneLoaded) ? barStyle : buttonActiveStyle, new GUILayoutOption[]
		{
			GUILayout.Width(size.x * 2f),
			GUILayout.Height(size.y * 2f)
		}))
		{
			clearOnNewSceneLoaded = !clearOnNewSceneLoaded;
		}
		if (showTimeButton && GUILayout.Button(showTimeContent, (!showTime) ? barStyle : buttonActiveStyle, new GUILayoutOption[]
		{
			GUILayout.Width(size.x * 2f),
			GUILayout.Height(size.y * 2f)
		}))
		{
			showTime = !showTime;
		}
		if (showSceneButton)
		{
			tempRect = GUILayoutUtility.GetLastRect();
			GUI.Label(tempRect, Time.realtimeSinceStartup.ToString("0.0"), lowerLeftFontStyle);
			if (GUILayout.Button(showSceneContent, (!showScene) ? barStyle : buttonActiveStyle, new GUILayoutOption[]
			{
				GUILayout.Width(size.x * 2f),
				GUILayout.Height(size.y * 2f)
			}))
			{
				showScene = !showScene;
			}
			tempRect = GUILayoutUtility.GetLastRect();
			GUI.Label(tempRect, currentScene, lowerLeftFontStyle);
		}
		if (showMemButton)
		{
			if (GUILayout.Button(showMemoryContent, (!showMemory) ? barStyle : buttonActiveStyle, new GUILayoutOption[]
			{
				GUILayout.Width(size.x * 2f),
				GUILayout.Height(size.y * 2f)
			}))
			{
				showMemory = !showMemory;
			}
			tempRect = GUILayoutUtility.GetLastRect();
			GUI.Label(tempRect, gcTotalMemory.ToString("0.0"), lowerLeftFontStyle);
		}
		if (showFpsButton)
		{
			if (GUILayout.Button(showFpsContent, (!showFps) ? barStyle : buttonActiveStyle, new GUILayoutOption[]
			{
				GUILayout.Width(size.x * 2f),
				GUILayout.Height(size.y * 2f)
			}))
			{
				showFps = !showFps;
			}
			tempRect = GUILayoutUtility.GetLastRect();
			GUI.Label(tempRect, fpsText, lowerLeftFontStyle);
		}
		if (showSearchText)
		{
			GUILayout.Box(searchContent, barStyle, new GUILayoutOption[]
			{
				GUILayout.Width(size.x * 2f),
				GUILayout.Height(size.y * 2f)
			});
			tempRect = GUILayoutUtility.GetLastRect();
			string a = GUI.TextField(tempRect, filterText, searchStyle);
			if (a != filterText)
			{
				filterText = a;
				calculateCurrentLog();
			}
		}
		if (GUILayout.Button(infoContent, barStyle, new GUILayoutOption[]
		{
			GUILayout.Width(size.x * 2f),
			GUILayout.Height(size.y * 2f)
		}))
		{
			currentView = Reporter.ReportView.Info;
		}
		GUILayout.FlexibleSpace();
		string text = " ";
		if (collapse)
		{
			text += numOfCollapsedLogs;
		}
		else
		{
			text += numOfLogs;
		}
		string text2 = " ";
		if (collapse)
		{
			text2 += numOfCollapsedLogsWarning;
		}
		else
		{
			text2 += numOfLogsWarning;
		}
		string text3 = " ";
		if (collapse)
		{
			text3 += numOfCollapsedLogsError;
		}
		else
		{
			text3 += numOfLogsError;
		}
		GUILayout.BeginHorizontal((!showLog) ? barStyle : buttonActiveStyle, new GUILayoutOption[0]);
		if (GUILayout.Button(logContent, nonStyle, new GUILayoutOption[]
		{
			GUILayout.Width(size.x * 2f),
			GUILayout.Height(size.y * 2f)
		}))
		{
			showLog = !showLog;
			calculateCurrentLog();
		}
		if (GUILayout.Button(text, nonStyle, new GUILayoutOption[]
		{
			GUILayout.Width(size.x * 2f),
			GUILayout.Height(size.y * 2f)
		}))
		{
			showLog = !showLog;
			calculateCurrentLog();
		}
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal((!showWarning) ? barStyle : buttonActiveStyle, new GUILayoutOption[0]);
		if (GUILayout.Button(warningContent, nonStyle, new GUILayoutOption[]
		{
			GUILayout.Width(size.x * 2f),
			GUILayout.Height(size.y * 2f)
		}))
		{
			showWarning = !showWarning;
			calculateCurrentLog();
		}
		if (GUILayout.Button(text2, nonStyle, new GUILayoutOption[]
		{
			GUILayout.Width(size.x * 2f),
			GUILayout.Height(size.y * 2f)
		}))
		{
			showWarning = !showWarning;
			calculateCurrentLog();
		}
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal((!showError) ? nonStyle : buttonActiveStyle, new GUILayoutOption[0]);
		if (GUILayout.Button(errorContent, nonStyle, new GUILayoutOption[]
		{
			GUILayout.Width(size.x * 2f),
			GUILayout.Height(size.y * 2f)
		}))
		{
			showError = !showError;
			calculateCurrentLog();
		}
		if (GUILayout.Button(text3, nonStyle, new GUILayoutOption[]
		{
			GUILayout.Width(size.x * 2f),
			GUILayout.Height(size.y * 2f)
		}))
		{
			showError = !showError;
			calculateCurrentLog();
		}
		GUILayout.EndHorizontal();
		if (GUILayout.Button(closeContent, barStyle, new GUILayoutOption[]
		{
			GUILayout.Width(size.x * 2f),
			GUILayout.Height(size.y * 2f)
		}))
		{
			show = false;
			ReporterGUI component = base.gameObject.GetComponent<ReporterGUI>();
			UnityEngine.Object.DestroyImmediate(component);
			try
			{
				base.gameObject.SendMessage("OnHideReporter");
			}
			catch (Exception exception)
			{
				UnityEngine.Debug.LogException(exception);
			}
		}
		GUILayout.EndHorizontal();
		GUILayout.EndScrollView();
		GUILayout.EndArea();
	}

	private void DrawLogs()
	{
		GUILayout.BeginArea(logsRect, backStyle);
		GUI.skin = logScrollerSkin;
		Vector2 drag = getDrag();
		if (drag.y != 0f && logsRect.Contains(new Vector2(downPos.x, (float)Screen.height - downPos.y)))
		{
			scrollPosition.y = scrollPosition.y + (drag.y - oldDrag);
		}
		scrollPosition = GUILayout.BeginScrollView(scrollPosition, new GUILayoutOption[0]);
		oldDrag = drag.y;
		int num = (int)((float)Screen.height * 0.75f / size.y);
		int count = currentLog.Count;
		num = Mathf.Min(num, count - startIndex);
		int num2 = 0;
		int num3 = (int)((float)startIndex * size.y);
		if (num3 > 0)
		{
			GUILayout.BeginHorizontal(new GUILayoutOption[]
			{
				GUILayout.Height((float)num3)
			});
			GUILayout.Label("---", new GUILayoutOption[0]);
			GUILayout.EndHorizontal();
		}
		int num4 = startIndex + num;
		num4 = Mathf.Clamp(num4, 0, count);
		bool flag = num < count;
		int num5 = startIndex;
		while (startIndex + num2 < num4)
		{
			if (num5 >= currentLog.Count)
			{
				break;
			}
			Reporter.Log log = currentLog[num5];
			if (log.logType != Reporter._LogType.Log || showLog)
			{
				if (log.logType != Reporter._LogType.Warning || showWarning)
				{
					if (log.logType != Reporter._LogType.Error || showError)
					{
						if (log.logType != Reporter._LogType.Assert || showError)
						{
							if (log.logType != Reporter._LogType.Exception || showError)
							{
								if (num2 >= num)
								{
									break;
								}
								GUIContent content;
								if (log.logType == Reporter._LogType.Log)
								{
									content = logContent;
								}
								else if (log.logType == Reporter._LogType.Warning)
								{
									content = warningContent;
								}
								else
								{
									content = errorContent;
								}
								GUIStyle guistyle = ((startIndex + num2) % 2 != 0) ? oddLogStyle : evenLogStyle;
								if (log == selectedLog)
								{
									guistyle = selectedLogStyle;
								}
								tempContent.text = log.count.ToString();
								float num6 = 0f;
								if (collapse)
								{
									num6 = barStyle.CalcSize(tempContent).x + 3f;
								}
								countRect.x = (float)Screen.width - num6;
								countRect.y = size.y * (float)num5;
								if (num3 > 0)
								{
									countRect.y = countRect.y + 8f;
								}
								countRect.width = num6;
								countRect.height = size.y;
								if (flag)
								{
									countRect.x = countRect.x - size.x * 2f;
								}
								Reporter.Sample sample = samples[log.sampleId];
								fpsRect = countRect;
								if (showFps)
								{
									tempContent.text = sample.fpsText;
									num6 = guistyle.CalcSize(tempContent).x + size.x;
									fpsRect.x = fpsRect.x - num6;
									fpsRect.width = size.x;
									fpsLabelRect = fpsRect;
									fpsLabelRect.x = fpsLabelRect.x + size.x;
									fpsLabelRect.width = num6 - size.x;
								}
								memoryRect = fpsRect;
								if (showMemory)
								{
									tempContent.text = sample.memory.ToString("0.000");
									num6 = guistyle.CalcSize(tempContent).x + size.x;
									memoryRect.x = memoryRect.x - num6;
									memoryRect.width = size.x;
									memoryLabelRect = memoryRect;
									memoryLabelRect.x = memoryLabelRect.x + size.x;
									memoryLabelRect.width = num6 - size.x;
								}
								sceneRect = memoryRect;
								if (showScene)
								{
									tempContent.text = sample.GetSceneName();
									num6 = guistyle.CalcSize(tempContent).x + size.x;
									sceneRect.x = sceneRect.x - num6;
									sceneRect.width = size.x;
									sceneLabelRect = sceneRect;
									sceneLabelRect.x = sceneLabelRect.x + size.x;
									sceneLabelRect.width = num6 - size.x;
								}
								timeRect = sceneRect;
								if (showTime)
								{
									tempContent.text = sample.time.ToString("0.000");
									num6 = guistyle.CalcSize(tempContent).x + size.x;
									timeRect.x = timeRect.x - num6;
									timeRect.width = size.x;
									timeLabelRect = timeRect;
									timeLabelRect.x = timeLabelRect.x + size.x;
									timeLabelRect.width = num6 - size.x;
								}
								GUILayout.BeginHorizontal(guistyle, new GUILayoutOption[0]);
								if (log == selectedLog)
								{
									GUILayout.Box(content, nonStyle, new GUILayoutOption[]
									{
										GUILayout.Width(size.x),
										GUILayout.Height(size.y)
									});
									GUILayout.Label(log.condition, selectedLogFontStyle, new GUILayoutOption[0]);
									if (showTime)
									{
										GUI.Box(timeRect, showTimeContent, guistyle);
										GUI.Label(timeLabelRect, sample.time.ToString("0.000"), guistyle);
									}
									if (showScene)
									{
										GUI.Box(sceneRect, showSceneContent, guistyle);
										GUI.Label(sceneLabelRect, sample.GetSceneName(), guistyle);
									}
									if (showMemory)
									{
										GUI.Box(memoryRect, showMemoryContent, guistyle);
										GUI.Label(memoryLabelRect, sample.memory.ToString("0.000") + " mb", guistyle);
									}
									if (showFps)
									{
										GUI.Box(fpsRect, showFpsContent, guistyle);
										GUI.Label(fpsLabelRect, sample.fpsText, guistyle);
									}
								}
								else
								{
									if (GUILayout.Button(content, nonStyle, new GUILayoutOption[]
									{
										GUILayout.Width(size.x),
										GUILayout.Height(size.y)
									}))
									{
										selectedLog = log;
									}
									if (GUILayout.Button(log.condition, logButtonStyle, new GUILayoutOption[0]))
									{
										selectedLog = log;
									}
									if (showTime)
									{
										GUI.Box(timeRect, showTimeContent, guistyle);
										GUI.Label(timeLabelRect, sample.time.ToString("0.000"), guistyle);
									}
									if (showScene)
									{
										GUI.Box(sceneRect, showSceneContent, guistyle);
										GUI.Label(sceneLabelRect, sample.GetSceneName(), guistyle);
									}
									if (showMemory)
									{
										GUI.Box(memoryRect, showMemoryContent, guistyle);
										GUI.Label(memoryLabelRect, sample.memory.ToString("0.000") + " mb", guistyle);
									}
									if (showFps)
									{
										GUI.Box(fpsRect, showFpsContent, guistyle);
										GUI.Label(fpsLabelRect, sample.fpsText, guistyle);
									}
								}
								if (collapse)
								{
									GUI.Label(countRect, log.count.ToString(), barStyle);
								}
								GUILayout.EndHorizontal();
								num2++;
							}
						}
					}
				}
			}
			num5++;
		}
		int num7 = (int)((float)(count - (startIndex + num)) * size.y);
		if (num7 > 0)
		{
			GUILayout.BeginHorizontal(new GUILayoutOption[]
			{
				GUILayout.Height((float)num7)
			});
			GUILayout.Label(" ", new GUILayoutOption[0]);
			GUILayout.EndHorizontal();
		}
		GUILayout.EndScrollView();
		GUILayout.EndArea();
		buttomRect.x = 0f;
		buttomRect.y = (float)Screen.height - size.y;
		buttomRect.width = (float)Screen.width;
		buttomRect.height = size.y;
		if (showGraph)
		{
			drawGraph();
		}
		else
		{
			drawStack();
		}
	}

	private void drawGraph()
	{
		graphRect = stackRect;
		graphRect.height = (float)Screen.height * 0.25f;
		GUI.skin = graphScrollerSkin;
		Vector2 drag = getDrag();
		if (graphRect.Contains(new Vector2(downPos.x, (float)Screen.height - downPos.y)))
		{
			if (drag.x != 0f)
			{
				graphScrollerPos.x = graphScrollerPos.x - (drag.x - oldDrag3);
				graphScrollerPos.x = Mathf.Max(0f, graphScrollerPos.x);
			}
			Vector2 lhs = downPos;
			if (lhs != Vector2.zero)
			{
				currentFrame = startFrame + (int)(lhs.x / graphSize);
			}
		}
		oldDrag3 = drag.x;
		GUILayout.BeginArea(graphRect, backStyle);
		graphScrollerPos = GUILayout.BeginScrollView(graphScrollerPos, new GUILayoutOption[0]);
		startFrame = (int)(graphScrollerPos.x / graphSize);
		if (graphScrollerPos.x >= (float)samples.Count * graphSize - (float)Screen.width)
		{
			graphScrollerPos.x = graphScrollerPos.x + graphSize;
		}
		GUILayout.Label(" ", new GUILayoutOption[]
		{
			GUILayout.Width((float)samples.Count * graphSize)
		});
		GUILayout.EndScrollView();
		GUILayout.EndArea();
		maxFpsValue = 0f;
		minFpsValue = 100000f;
		maxMemoryValue = 0f;
		minMemoryValue = 100000f;
		int num = 0;
		while ((float)num < (float)Screen.width / graphSize)
		{
			int num2 = startFrame + num;
			if (num2 >= samples.Count)
			{
				break;
			}
			Reporter.Sample sample = samples[num2];
			if (maxFpsValue < sample.fps)
			{
				maxFpsValue = sample.fps;
			}
			if (minFpsValue > sample.fps)
			{
				minFpsValue = sample.fps;
			}
			if (maxMemoryValue < sample.memory)
			{
				maxMemoryValue = sample.memory;
			}
			if (minMemoryValue > sample.memory)
			{
				minMemoryValue = sample.memory;
			}
			num++;
		}
		if (currentFrame != -1 && currentFrame < samples.Count)
		{
			Reporter.Sample sample2 = samples[currentFrame];
			GUILayout.BeginArea(buttomRect, backStyle);
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.Box(showTimeContent, nonStyle, new GUILayoutOption[]
			{
				GUILayout.Width(size.x),
				GUILayout.Height(size.y)
			});
			GUILayout.Label(sample2.time.ToString("0.0"), nonStyle, new GUILayoutOption[0]);
			GUILayout.Space(size.x);
			GUILayout.Box(showSceneContent, nonStyle, new GUILayoutOption[]
			{
				GUILayout.Width(size.x),
				GUILayout.Height(size.y)
			});
			GUILayout.Label(sample2.GetSceneName(), nonStyle, new GUILayoutOption[0]);
			GUILayout.Space(size.x);
			GUILayout.Box(showMemoryContent, nonStyle, new GUILayoutOption[]
			{
				GUILayout.Width(size.x),
				GUILayout.Height(size.y)
			});
			GUILayout.Label(sample2.memory.ToString("0.000"), nonStyle, new GUILayoutOption[0]);
			GUILayout.Space(size.x);
			GUILayout.Box(showFpsContent, nonStyle, new GUILayoutOption[]
			{
				GUILayout.Width(size.x),
				GUILayout.Height(size.y)
			});
			GUILayout.Label(sample2.fpsText, nonStyle, new GUILayoutOption[0]);
			GUILayout.Space(size.x);
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			GUILayout.EndArea();
		}
		graphMaxRect = stackRect;
		graphMaxRect.height = size.y;
		GUILayout.BeginArea(graphMaxRect);
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Box(showMemoryContent, nonStyle, new GUILayoutOption[]
		{
			GUILayout.Width(size.x),
			GUILayout.Height(size.y)
		});
		GUILayout.Label(maxMemoryValue.ToString("0.000"), nonStyle, new GUILayoutOption[0]);
		GUILayout.Box(showFpsContent, nonStyle, new GUILayoutOption[]
		{
			GUILayout.Width(size.x),
			GUILayout.Height(size.y)
		});
		GUILayout.Label(maxFpsValue.ToString("0.000"), nonStyle, new GUILayoutOption[0]);
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
		graphMinRect = stackRect;
		graphMinRect.y = stackRect.y + stackRect.height - size.y;
		graphMinRect.height = size.y;
		GUILayout.BeginArea(graphMinRect);
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Box(showMemoryContent, nonStyle, new GUILayoutOption[]
		{
			GUILayout.Width(size.x),
			GUILayout.Height(size.y)
		});
		GUILayout.Label(minMemoryValue.ToString("0.000"), nonStyle, new GUILayoutOption[0]);
		GUILayout.Box(showFpsContent, nonStyle, new GUILayoutOption[]
		{
			GUILayout.Width(size.x),
			GUILayout.Height(size.y)
		});
		GUILayout.Label(minFpsValue.ToString("0.000"), nonStyle, new GUILayoutOption[0]);
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
	}

	private void drawStack()
	{
		if (selectedLog != null)
		{
			Vector2 drag = getDrag();
			if (drag.y != 0f && stackRect.Contains(new Vector2(downPos.x, (float)Screen.height - downPos.y)))
			{
				scrollPosition2.y = scrollPosition2.y + (drag.y - oldDrag2);
			}
			oldDrag2 = drag.y;
			GUILayout.BeginArea(stackRect, backStyle);
			scrollPosition2 = GUILayout.BeginScrollView(scrollPosition2, new GUILayoutOption[0]);
			Reporter.Sample sample = null;
			try
			{
				sample = samples[selectedLog.sampleId];
			}
			catch (Exception exception)
			{
				UnityEngine.Debug.LogException(exception);
			}
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.Label(selectedLog.condition, stackLabelStyle, new GUILayoutOption[0]);
			GUILayout.EndHorizontal();
			GUILayout.Space(size.y * 0.25f);
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.Label(selectedLog.stacktrace, stackLabelStyle, new GUILayoutOption[0]);
			GUILayout.EndHorizontal();
			GUILayout.Space(size.y);
			GUILayout.EndScrollView();
			GUILayout.EndArea();
			GUILayout.BeginArea(buttomRect, backStyle);
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.Box(showTimeContent, nonStyle, new GUILayoutOption[]
			{
				GUILayout.Width(size.x),
				GUILayout.Height(size.y)
			});
			GUILayout.Label(sample.time.ToString("0.000"), nonStyle, new GUILayoutOption[0]);
			GUILayout.Space(size.x);
			GUILayout.Box(showSceneContent, nonStyle, new GUILayoutOption[]
			{
				GUILayout.Width(size.x),
				GUILayout.Height(size.y)
			});
			GUILayout.Label(sample.GetSceneName(), nonStyle, new GUILayoutOption[0]);
			GUILayout.Space(size.x);
			GUILayout.Box(showMemoryContent, nonStyle, new GUILayoutOption[]
			{
				GUILayout.Width(size.x),
				GUILayout.Height(size.y)
			});
			GUILayout.Label(sample.memory.ToString("0.000"), nonStyle, new GUILayoutOption[0]);
			GUILayout.Space(size.x);
			GUILayout.Box(showFpsContent, nonStyle, new GUILayoutOption[]
			{
				GUILayout.Width(size.x),
				GUILayout.Height(size.y)
			});
			GUILayout.Label(sample.fpsText, nonStyle, new GUILayoutOption[0]);
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			GUILayout.EndArea();
		}
		else
		{
			GUILayout.BeginArea(stackRect, backStyle);
			GUILayout.EndArea();
			GUILayout.BeginArea(buttomRect, backStyle);
			GUILayout.EndArea();
		}
	}

	public void OnGUIDraw()
	{
		if (!show)
		{
			return;
		}
		screenRect.x = 0f;
		screenRect.y = 0f;
		screenRect.width = (float)Screen.width;
		screenRect.height = (float)Screen.height;
		getDownPos();
		logsRect.x = 0f;
		logsRect.y = size.y * 2f;
		logsRect.width = (float)Screen.width;
		logsRect.height = (float)Screen.height * 0.75f - size.y * 2f;
		stackRectTopLeft.x = 0f;
		stackRect.x = 0f;
		stackRectTopLeft.y = (float)Screen.height * 0.75f;
		stackRect.y = (float)Screen.height * 0.75f;
		stackRect.width = (float)Screen.width;
		stackRect.height = (float)Screen.height * 0.25f - size.y;
		detailRect.x = 0f;
		detailRect.y = (float)Screen.height - size.y * 3f;
		detailRect.width = (float)Screen.width;
		detailRect.height = size.y * 3f;
		if (currentView == Reporter.ReportView.Info)
		{
			DrawInfo();
		}
		else if (currentView == Reporter.ReportView.Logs)
		{
			drawToolBar();
			DrawLogs();
		}
	}

	private static int GetActiveTouchCount(out TouchControl primary)
	{
		primary = null;
		Touchscreen touchscreen = Touchscreen.current;
		if (touchscreen == null)
		{
			return 0;
		}
		int count = 0;
		for (int i = 0; i < touchscreen.touches.Count; i++)
		{
			if (touchscreen.touches[i].phase.ReadValue() != TouchPhase.None)
			{
				if (count == 0)
				{
					primary = touchscreen.touches[i];
				}
				count++;
			}
		}
		return count;
	}

	private bool isGestureDone()
	{
		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
		{
			TouchControl primary;
			int activeTouches = Reporter.GetActiveTouchCount(out primary);
			if (activeTouches != 1)
			{
				gestureDetector.Clear();
				gestureCount = 0;
			}
			else
			{
				TouchPhase phase = primary.phase.ReadValue();
				if (phase == TouchPhase.Canceled || phase == TouchPhase.Ended)
				{
					gestureDetector.Clear();
				}
				else if (phase == TouchPhase.Moved)
				{
					Vector2 position = primary.position.ReadValue();
					if (gestureDetector.Count == 0 || (position - gestureDetector[gestureDetector.Count - 1]).magnitude > 10f)
					{
						gestureDetector.Add(position);
					}
				}
			}
		}
		else if (Mouse.current != null && Mouse.current.leftButton.wasReleasedThisFrame)
		{
			gestureDetector.Clear();
			gestureCount = 0;
		}
		else if (Mouse.current != null && Mouse.current.leftButton.isPressed)
		{
			Vector2 vector = Mouse.current.position.ReadValue();
			if (gestureDetector.Count == 0 || (vector - gestureDetector[gestureDetector.Count - 1]).magnitude > 10f)
			{
				gestureDetector.Add(vector);
			}
		}
		if (gestureDetector.Count < 10)
		{
			return false;
		}
		gestureSum = Vector2.zero;
		gestureLength = 0f;
		Vector2 rhs = Vector2.zero;
		for (int i = 0; i < gestureDetector.Count - 2; i++)
		{
			Vector2 vector2 = gestureDetector[i + 1] - gestureDetector[i];
			float magnitude = vector2.magnitude;
			gestureSum += vector2;
			gestureLength += magnitude;
			float num = Vector2.Dot(vector2, rhs);
			if (num < 0f)
			{
				gestureDetector.Clear();
				gestureCount = 0;
				return false;
			}
			rhs = vector2;
		}
		int num2 = (Screen.width + Screen.height) / 4;
		if (gestureLength > (float)num2 && gestureSum.magnitude < (float)(num2 / 2))
		{
			gestureDetector.Clear();
			gestureCount++;
			if (gestureCount >= numOfCircleToShow)
			{
				return true;
			}
		}
		return false;
	}

	private bool isDoubleClickDone()
	{
		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
		{
			TouchControl primary;
			int activeTouches = Reporter.GetActiveTouchCount(out primary);
			if (activeTouches != 1)
			{
				lastClickTime = -1f;
			}
			else if (primary.phase.ReadValue() == TouchPhase.Began)
			{
				if (lastClickTime == -1f)
				{
					lastClickTime = Time.realtimeSinceStartup;
				}
				else
				{
					if (Time.realtimeSinceStartup - lastClickTime < 0.2f)
					{
						lastClickTime = -1f;
						return true;
					}
					lastClickTime = Time.realtimeSinceStartup;
				}
			}
		}
		else if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
		{
			if (lastClickTime == -1f)
			{
				lastClickTime = Time.realtimeSinceStartup;
			}
			else
			{
				if (Time.realtimeSinceStartup - lastClickTime < 0.2f)
				{
					lastClickTime = -1f;
					return true;
				}
				lastClickTime = Time.realtimeSinceStartup;
			}
		}
		return false;
	}

	private Vector2 getDownPos()
	{
		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
		{
			TouchControl primary;
			int activeTouches = Reporter.GetActiveTouchCount(out primary);
			if (activeTouches == 1 && primary.phase.ReadValue() == TouchPhase.Began)
			{
				downPos = primary.position.ReadValue();
				return downPos;
			}
		}
		else if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
		{
			Vector2 mousePos = Mouse.current.position.ReadValue();
			downPos.x = mousePos.x;
			downPos.y = mousePos.y;
			return downPos;
		}
		return Vector2.zero;
	}

	private Vector2 getDrag()
	{
		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
		{
			TouchControl primary;
			int activeTouches = Reporter.GetActiveTouchCount(out primary);
			if (activeTouches != 1)
			{
				return Vector2.zero;
			}
			return primary.position.ReadValue() - downPos;
		}
		else
		{
			if (Mouse.current != null && Mouse.current.leftButton.isPressed)
			{
				mousePosition = Mouse.current.position.ReadValue();
				return mousePosition - downPos;
			}
			return Vector2.zero;
		}
	}

	private void calculateStartIndex()
	{
		startIndex = (int)(scrollPosition.y / size.y);
		startIndex = Mathf.Clamp(startIndex, 0, currentLog.Count);
	}

	private void doShow()
	{
		show = true;
		currentView = Reporter.ReportView.Logs;
		base.gameObject.AddComponent<ReporterGUI>();
		try
		{
			base.gameObject.SendMessage("OnShowReporter");
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogException(exception);
		}
	}

	private void Update()
	{
		fpsText = fps.ToString("0.000");
		gcTotalMemory = (float)GC.GetTotalMemory(false) / 1024f / 1024f;
		int buildIndex = SceneManager.GetActiveScene().buildIndex;
		if (buildIndex != -1 && string.IsNullOrEmpty(Reporter.scenes[buildIndex]))
		{
			Reporter.scenes[SceneManager.GetActiveScene().buildIndex] = SceneManager.GetActiveScene().name;
		}
		calculateStartIndex();
		if (!show && isGestureDone())
		{
			doShow();
		}
		if (threadedLogs.Count > 0)
		{
			object obj = threadedLogs;
			lock (obj)
			{
				for (int i = 0; i < threadedLogs.Count; i++)
				{
					Reporter.Log log = threadedLogs[i];
					AddLog(log.condition, log.stacktrace, (LogType)log.logType);
				}
				threadedLogs.Clear();
			}
		}
		if (firstTime)
		{
			firstTime = false;
			lastUpdate = Time.realtimeSinceStartup;
			frames = 0;
			return;
		}
		frames++;
		float num = Time.realtimeSinceStartup - lastUpdate;
		if (num > 0.25f && frames > 10)
		{
			fps = (float)frames / num;
			lastUpdate = Time.realtimeSinceStartup;
			frames = 0;
		}
	}

	private void CaptureLog(string condition, string stacktrace, LogType type)
	{
		AddLog(condition, stacktrace, type);
	}

	private void AddLog(string condition, string stacktrace, LogType type)
	{
		float num = 0f;
		string text = string.Empty;
		if (cachedString.ContainsKey(condition))
		{
			text = cachedString[condition];
		}
		else
		{
			text = condition;
			cachedString.Add(text, text);
			num += (float)((!string.IsNullOrEmpty(text)) ? (text.Length * 2) : 0);
			num += (float)IntPtr.Size;
		}
		string text2 = string.Empty;
		if (cachedString.ContainsKey(stacktrace))
		{
			text2 = cachedString[stacktrace];
		}
		else
		{
			text2 = stacktrace;
			cachedString.Add(text2, text2);
			num += (float)((!string.IsNullOrEmpty(text2)) ? (text2.Length * 2) : 0);
			num += (float)IntPtr.Size;
		}
		bool flag = false;
		addSample();
		Reporter.Log log = new Reporter.Log
		{
			logType = (Reporter._LogType)type,
			condition = text,
			stacktrace = text2,
			sampleId = samples.Count - 1
		};
		num += log.GetMemoryUsage();
		logsMemUsage += num / 1024f / 1024f;
		if (TotalMemUsage > maxSize)
		{
			clear();
			UnityEngine.Debug.Log("Memory Usage Reach" + maxSize + " mb So It is Cleared");
			return;
		}
		bool flag2;
		if (logsDic.ContainsKey(text, stacktrace))
		{
			flag2 = false;
			logsDic[text][stacktrace].count++;
		}
		else
		{
			flag2 = true;
			collapsedLogs.Add(log);
			logsDic[text][stacktrace] = log;
			if (type == LogType.Log)
			{
				numOfCollapsedLogs++;
			}
			else if (type == LogType.Warning)
			{
				numOfCollapsedLogsWarning++;
			}
			else
			{
				numOfCollapsedLogsError++;
			}
		}
		if (type == LogType.Log)
		{
			numOfLogs++;
		}
		else if (type == LogType.Warning)
		{
			numOfLogsWarning++;
		}
		else
		{
			numOfLogsError++;
		}
		logs.Add(log);
		if (!collapse || flag2)
		{
			bool flag3 = false;
			if (log.logType == Reporter._LogType.Log && !showLog)
			{
				flag3 = true;
			}
			if (log.logType == Reporter._LogType.Warning && !showWarning)
			{
				flag3 = true;
			}
			if (log.logType == Reporter._LogType.Error && !showError)
			{
				flag3 = true;
			}
			if (log.logType == Reporter._LogType.Assert && !showError)
			{
				flag3 = true;
			}
			if (log.logType == Reporter._LogType.Exception && !showError)
			{
				flag3 = true;
			}
			if (!flag3 && (string.IsNullOrEmpty(filterText) || log.condition.ToLower().Contains(filterText.ToLower())))
			{
				currentLog.Add(log);
				flag = true;
			}
		}
		if (flag)
		{
			calculateStartIndex();
			int count = currentLog.Count;
			int num2 = (int)((float)Screen.height * 0.75f / size.y);
			if (startIndex >= count - num2)
			{
				scrollPosition.y = scrollPosition.y + size.y;
			}
		}
		try
		{
			base.gameObject.SendMessage("OnLog", log);
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogException(exception);
		}
	}

	private void CaptureLogThread(string condition, string stacktrace, LogType type)
	{
		Reporter.Log item = new Reporter.Log
		{
			condition = condition,
			stacktrace = stacktrace,
			logType = (Reporter._LogType)type
		};
		object obj = threadedLogs;
		lock (obj)
		{
			threadedLogs.Add(item);
		}
	}


	private void OnApplicationQuit()
	{
		PlayerPrefs.SetInt("Reporter_currentView", (int)currentView);
		PlayerPrefs.SetInt("Reporter_show", (!show) ? 0 : 1);
		PlayerPrefs.SetInt("Reporter_collapse", (!collapse) ? 0 : 1);
		PlayerPrefs.SetInt("Reporter_clearOnNewSceneLoaded", (!clearOnNewSceneLoaded) ? 0 : 1);
		PlayerPrefs.SetInt("Reporter_showTime", (!showTime) ? 0 : 1);
		PlayerPrefs.SetInt("Reporter_showScene", (!showScene) ? 0 : 1);
		PlayerPrefs.SetInt("Reporter_showMemory", (!showMemory) ? 0 : 1);
		PlayerPrefs.SetInt("Reporter_showFps", (!showFps) ? 0 : 1);
		PlayerPrefs.SetInt("Reporter_showGraph", (!showGraph) ? 0 : 1);
		PlayerPrefs.SetInt("Reporter_showLog", (!showLog) ? 0 : 1);
		PlayerPrefs.SetInt("Reporter_showWarning", (!showWarning) ? 0 : 1);
		PlayerPrefs.SetInt("Reporter_showError", (!showError) ? 0 : 1);
		PlayerPrefs.SetString("Reporter_filterText", filterText);
		PlayerPrefs.SetFloat("Reporter_size", size.x);
		PlayerPrefs.SetInt("Reporter_showClearOnNewSceneLoadedButton", (!showClearOnNewSceneLoadedButton) ? 0 : 1);
		PlayerPrefs.SetInt("Reporter_showTimeButton", (!showTimeButton) ? 0 : 1);
		PlayerPrefs.SetInt("Reporter_showSceneButton", (!showSceneButton) ? 0 : 1);
		PlayerPrefs.SetInt("Reporter_showMemButton", (!showMemButton) ? 0 : 1);
		PlayerPrefs.SetInt("Reporter_showFpsButton", (!showFpsButton) ? 0 : 1);
		PlayerPrefs.SetInt("Reporter_showSearchText", (!showSearchText) ? 0 : 1);
		PlayerPrefs.Save();
	}

	private IEnumerator readInfo()
	{
		string prefFile = "build_info.txt";
		string url = prefFile;
		if (prefFile.IndexOf("://") == -1)
		{
			string text = Application.streamingAssetsPath;
			if (text == string.Empty)
			{
				text = Application.dataPath + "/StreamingAssets/";
			}
			url = Path.Combine(text, prefFile);
		}
		using (UnityWebRequest www = UnityWebRequest.Get(url))
		{
			yield return www.SendWebRequest();
			if (www.result != UnityWebRequest.Result.Success)
			{
				UnityEngine.Debug.LogError(www.error);
			}
			else
			{
				buildDate = www.downloadHandler.text;
			}
		}
		yield break;
	}

	private List<Reporter.Sample> samples = new List<Reporter.Sample>(216000);

	private List<Reporter.Log> logs = new List<Reporter.Log>();

	private List<Reporter.Log> collapsedLogs = new List<Reporter.Log>();

	private List<Reporter.Log> currentLog = new List<Reporter.Log>();

	private MultiKeyDictionary<string, string, Reporter.Log> logsDic = new MultiKeyDictionary<string, string, Reporter.Log>();

	private Dictionary<string, string> cachedString = new Dictionary<string, string>();

	[HideInInspector]
	public bool show;

	private bool collapse;

	private bool clearOnNewSceneLoaded;

	private bool showTime;

	private bool showScene;

	private bool showMemory;

	private bool showFps;

	private bool showGraph;

	private bool showLog = true;

	private bool showWarning = true;

	private bool showError = true;

	private int numOfLogs;

	private int numOfLogsWarning;

	private int numOfLogsError;

	private int numOfCollapsedLogs;

	private int numOfCollapsedLogsWarning;

	private int numOfCollapsedLogsError;

	private bool showClearOnNewSceneLoadedButton = true;

	private bool showTimeButton = true;

	private bool showSceneButton = true;

	private bool showMemButton = true;

	private bool showFpsButton = true;

	private bool showSearchText = true;

	private string buildDate;

	private string logDate;

	private float logsMemUsage;

	private float graphMemUsage;

	private float gcTotalMemory;

	public string UserData = string.Empty;

	public float fps;

	public string fpsText;

	private Reporter.ReportView currentView = Reporter.ReportView.Logs;

	private static bool created;

	public Images images;

	private GUIContent clearContent;

	private GUIContent collapseContent;

	private GUIContent clearOnNewSceneContent;

	private GUIContent showTimeContent;

	private GUIContent showSceneContent;

	private GUIContent userContent;

	private GUIContent showMemoryContent;

	private GUIContent softwareContent;

	private GUIContent dateContent;

	private GUIContent showFpsContent;

	private GUIContent infoContent;

	private GUIContent searchContent;

	private GUIContent closeContent;

	private GUIContent buildFromContent;

	private GUIContent systemInfoContent;

	private GUIContent graphicsInfoContent;

	private GUIContent backContent;

	private GUIContent logContent;

	private GUIContent warningContent;

	private GUIContent errorContent;

	private GUIStyle barStyle;

	private GUIStyle buttonActiveStyle;

	private GUIStyle nonStyle;

	private GUIStyle lowerLeftFontStyle;

	private GUIStyle backStyle;

	private GUIStyle evenLogStyle;

	private GUIStyle oddLogStyle;

	private GUIStyle logButtonStyle;

	private GUIStyle selectedLogStyle;

	private GUIStyle selectedLogFontStyle;

	private GUIStyle stackLabelStyle;

	private GUIStyle scrollerStyle;

	private GUIStyle searchStyle;

	private GUIStyle sliderBackStyle;

	private GUIStyle sliderThumbStyle;

	private GUISkin toolbarScrollerSkin;

	private GUISkin logScrollerSkin;

	private GUISkin graphScrollerSkin;

	public Vector2 size = new Vector2(32f, 32f);

	public float maxSize = 20f;

	public int numOfCircleToShow = 1;

	private static string[] scenes;

	private string currentScene;

	private string filterText = string.Empty;

	private string deviceModel;

	private string deviceType;

	private string deviceName;

	private string graphicsMemorySize;

	private string maxTextureSize;

	private string systemMemorySize;

	public bool Initialized;

	private Rect screenRect;

	private Rect toolBarRect;

	private Rect logsRect;

	private Rect stackRect;

	private Rect graphRect;

	private Rect graphMinRect;

	private Rect graphMaxRect;

	private Rect buttomRect;

	private Vector2 stackRectTopLeft;

	private Rect detailRect;

	private Vector2 scrollPosition;

	private Vector2 scrollPosition2;

	private Vector2 toolbarScrollPosition;

	private Reporter.Log selectedLog;

	private float toolbarOldDrag;

	private float oldDrag;

	private float oldDrag2;

	private float oldDrag3;

	private int startIndex;

	private Rect countRect;

	private Rect timeRect;

	private Rect timeLabelRect;

	private Rect sceneRect;

	private Rect sceneLabelRect;

	private Rect memoryRect;

	private Rect memoryLabelRect;

	private Rect fpsRect;

	private Rect fpsLabelRect;

	private GUIContent tempContent = new GUIContent();

	private Vector2 infoScrollPosition;

	private Vector2 oldInfoDrag;

	private Rect tempRect;

	private float graphSize = 4f;

	private int startFrame;

	private int currentFrame;

	private Vector3 tempVector1;

	private Vector3 tempVector2;

	private Vector2 graphScrollerPos;

	private float maxFpsValue;

	private float minFpsValue;

	private float maxMemoryValue;

	private float minMemoryValue;

	private List<Vector2> gestureDetector = new List<Vector2>();

	private Vector2 gestureSum = Vector2.zero;

	private float gestureLength;

	private int gestureCount;

	private float lastClickTime = -1f;

	private Vector2 startPos;

	private Vector2 downPos;

	private Vector2 mousePosition;

	private int frames;

	private bool firstTime = true;

	private float lastUpdate;

	private const int requiredFrames = 10;

	private const float updateInterval = 0.25f;

	private List<Reporter.Log> threadedLogs = new List<Reporter.Log>();

	public enum _LogType
	{
		Assert = 1,
		Error = 0,
		Exception = 4,
		Log = 3,
		Warning = 2
	}

	public class Sample
	{
		public static float MemSize()
		{
			return 13f;
		}

		public string GetSceneName()
		{
			if ((int)loadedScene == -1)
			{
				return "AssetBundleScene";
			}
			return Reporter.scenes[(int)loadedScene];
		}

		public float time;

		public byte loadedScene;

		public float memory;

		public float fps;

		public string fpsText;
	}

	public class Log
	{
		public Reporter.Log CreateCopy()
		{
			return (Reporter.Log)base.MemberwiseClone();
		}

		public float GetMemoryUsage()
		{
			return (float)(8 + condition.Length * 2 + stacktrace.Length * 2 + 4);
		}

		public int count = 1;

		public Reporter._LogType logType;

		public string condition;

		public string stacktrace;

		public int sampleId;
	}

	private enum ReportView
	{
		None,
		Logs,
		Info,
		Snapshot
	}

	private enum DetailView
	{
		None,
		StackTrace,
		Graph
	}
}
