using System;
using System.Collections.Generic;
using Data;
using UnityEngine;

public class ConfigRegistry : MonoBehaviour
{
    public PetSetup petConfig;

    public TdLingo tdLocalizationConfig;

    public ArenaConstantSetup tournamentConstantConfig;

    public ArenaPrizeSetup tournamentPrizeConfig;

    public ItemSetup itemConfig;

    public DailyPrizeSetup dailyRewardConfig;

    public SalePackSetup saleBundleConfig;

    public SignalSetup eventConfig;

    public GameObject IgnoreDefPrefab;

    public MultiLanguageDataReader multiLanguageDataReader;

    [HideInInspector]
    public Dictionary<int, FormationSetupRecord> formationData = new Dictionary<int, FormationSetupRecord>();

    public Dictionary<SignalQuestKind, List<SignalSetupRecord>> eventTypeToEventData = new Dictionary<SignalQuestKind, List<SignalSetupRecord>>();

    public Dictionary<int, SignalSetupRecord> eventIdToEventData = new Dictionary<int, SignalSetupRecord>();
    
	public static ConfigRegistry Instance { get; private set; }


	private void Awake()
	{
		if (ConfigRegistry.Instance)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		ConfigRegistry.Instance = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		InitEventQuestData();
		ReadFormationData();
	}

	private void Start()
	{
		PlayerSaveStore.Instance.WriteDefaultSetting();
	}

	private void InitEventQuestData()
	{
		if (eventConfig == null || eventConfig.dataArray == null)
		{
			return;
		}
		int num = eventConfig.dataArray.Length;
		for (int i = 0; i < num; i++)
		{
			if (eventConfig.dataArray[i].Durationinday > 0)
			{
				SignalQuestKind eventquesttype = eventConfig.dataArray[i].EVENTQUESTTYPE;
				if (!eventTypeToEventData.ContainsKey(eventquesttype))
				{
					eventTypeToEventData.Add(eventquesttype, new List<SignalSetupRecord>());
				}
				eventTypeToEventData[eventquesttype].Add(eventConfig.dataArray[i]);
				eventIdToEventData[eventConfig.dataArray[i].Eventid] = eventConfig.dataArray[i];
			}
		}
	}

	private void ReadFormationData()
	{
		string text = "Parameters/MapCampaign/enemy_formation";
		try
		{
			List<Dictionary<string, object>> list = CSVLoader.Read(text);
			for (int i = 0; i < list.Count; i++)
			{
				int key = (int)list[i]["formation_id"];
				int num = (int)list[i]["time"];
				if (!formationData.ContainsKey(key))
				{
					formationData.Add(key, new FormationSetupRecord());
				}
				formationData[key].AddTime((float)num * 0.001f);
			}
		}
		catch (Exception)
		{
			UnityEngine.Debug.LogError("File " + text + ".csv khÃ´ng tá»“n táº¡i hoáº·c dá»¯ liá»‡u trong file khÃ´ng Ä‘Ãºng Ä‘á»‹nh dáº¡ng.");
			throw;
		}
	}
}
