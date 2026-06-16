using System;
using System.Collections.Generic;
using CodeStage.AntiCheat.ObscuredTypes;
using Data;
using DG.Tweening;
using Gameplay;
using MetaGame;
using Parameter;
using UnityEngine;
using UnityEngine.UI;

public class GameKit
{

    private static string EVENT_PREFIX = "evDt_";

    public static string EVENT_ENDTIME_PREFIX = "ev_end_";

    private static string LISTEVENT = "evList_";

    private static string UNCLAIM_EV_LIST = "unclaimEvs";

    public static string LAST_DATE_INIT_EVENT = "lastDateInitEv";

    public delegate float CustomScoreModifier(CharacterEntity characterModel, EnemyData enemy);

    public delegate bool CustomIsValidAlly(CharacterEntity characterModel);
    
	public static int GetGemQuantityTostartTour(bool increasePlayCount)
	{
		int numOfPlayTourCount = GameKit.GetNumOfPlayTourCount();
		if (numOfPlayTourCount == 0)
		{
			return 0;
		}
		float jFactor = ZoneRuleSpec.Instance.leagueIndexToPriceConstants[GameKit.tourUserSelfInfo.curtier].jFactor;
		int initGemQuantity = ZoneRuleSpec.Instance.leagueIndexToPriceConstants[GameKit.tourUserSelfInfo.curtier].initGemQuantity;
		if (increasePlayCount)
		{
			GameKit.SaveNumOfPlayTourCount(numOfPlayTourCount + 1);
		}
		return Mathf.CeilToInt((float)initGemQuantity * Mathf.Pow(jFactor, (float)(numOfPlayTourCount - 1)));
	}

	public static int GetNumOfPlayTourCount()
	{
		return PlayerPrefs.GetInt(GameKit.PLAY_TIME_COUNT, 0);
	}

	public static void SaveNumOfPlayTourCount(int count)
	{
		PlayerPrefs.SetInt(GameKit.PLAY_TIME_COUNT, count);
	}

	public static bool IsValidCharacter(CharacterEntity characterModel)
	{
		return !(characterModel == null) && characterModel.IsAlive;
	}

	public static bool IsCharacterVisible(CharacterEntity characterModel)
	{
		return !characterModel.IsInvisible;
	}

	public static bool IsValidEnemy(EnemyData enemyModel)
	{
		return !(enemyModel == null) && enemyModel.IsAlive && MonoSingleton<GameRecord>.Instance.IsListActiveEnemyContainThis(enemyModel);
	}

	public static bool IsEnemyAbleToGoTunnel(EnemyData enemyModel)
	{
		return !enemyModel.IsAir && !enemyModel.IsUnderground;
	}

	public static EnemyData GetNearstEnemy(GameObject sources)
	{
		EnemyData result = null;
		List<EnemyData> listActiveEnemy = MonoSingleton<GameRecord>.Instance.ListActiveEnemy;
		if (listActiveEnemy.Count > 0)
		{
			float num = float.PositiveInfinity;
			for (int i = listActiveEnemy.Count - 1; i >= 0; i--)
			{
				if (listActiveEnemy[i].curState != EntityPhaseEnum.EnemySpecialState && !listActiveEnemy[i].OriginalParameter.isBoss)
				{
					float sqrMagnitude = (sources.transform.position - listActiveEnemy[i].transform.position).sqrMagnitude;
					if (sqrMagnitude < num)
					{
						num = sqrMagnitude;
						result = listActiveEnemy[i];
					}
				}
			}
		}
		return result;
	}

	public static List<EnemyData> GetListEnemiesInRange(GameObject sources, SharedStrikeDamage commonAttackDamage)
	{
		return GameKit.GetListEnemiesInRange(sources.transform.position, commonAttackDamage);
	}

	public static List<EnemyData> GetListEnemiesInRange(Vector3 sourcePos, SharedStrikeDamage commonAttackDamage)
	{
		List<EnemyData> list = new List<EnemyData>();
		List<EnemyData> listActiveEnemy = MonoSingleton<GameRecord>.Instance.ListActiveEnemy;
		for (int i = 0; i < listActiveEnemy.Count; i++)
		{
			EnemyData enemyModel = listActiveEnemy[i];
			if (!enemyModel.IsAir || commonAttackDamage.targetType.isAir)
			{
				if (!enemyModel.IsUnderground || commonAttackDamage.targetType.isUnderGround)
				{
					if (!enemyModel.IsInTunnel || commonAttackDamage.targetType.isTunnel)
					{
						float num = MonoSingleton<GameRecord>.Instance.SqrDistance(sourcePos, enemyModel.transform.position);
						if (num <= commonAttackDamage.aoeRange * commonAttackDamage.aoeRange)
						{
							list.Add(enemyModel);
						}
					}
				}
			}
		}
		return list;
	}

	public static EnemyData GetRandomEnemyInRange(GameObject sources, SharedStrikeDamage commonAttackDamage)
	{
		EnemyData result = new EnemyData();
		List<EnemyData> listEnemiesInRange = GameKit.GetListEnemiesInRange(sources, commonAttackDamage);
		if (listEnemiesInRange.Count > 0)
		{
			result = listEnemiesInRange[UnityEngine.Random.Range(0, listEnemiesInRange.Count)];
		}
		else
		{
			result = null;
		}
		return result;
	}

	public static List<EnemyData> GetListFlyingEnemiesInRange(GameObject sources, SharedStrikeDamage commonAttackDamage)
	{
		List<EnemyData> list = new List<EnemyData>();
		List<EnemyData> listActiveEnemy = MonoSingleton<GameRecord>.Instance.ListActiveEnemy;
		for (int i = 0; i < listActiveEnemy.Count; i++)
		{
			EnemyData enemyModel = listActiveEnemy[i];
			if (enemyModel.IsAir && commonAttackDamage.targetType.isAir)
			{
				float num = MonoSingleton<GameRecord>.Instance.SqrDistance(sources, enemyModel.gameObject);
				if (num <= commonAttackDamage.aoeRange * commonAttackDamage.aoeRange)
				{
					list.Add(enemyModel);
				}
			}
		}
		return list;
	}

	public static bool IsUnderTargetOfASpecificHero(EnemyData self, CharacterEntity characterModel)
	{
		return GameKit.IsValidCharacter(characterModel) && !(characterModel.GetCurrentTarget() == null) && characterModel.GetCurrentTarget().GetEntityId() == self.GetEntityId();
	}

	public static bool IsUnderTargetOfAnyHero(EnemyData self, bool hasExceptionHero = false, int exceptionId = -1)
	{
		if (!GameKit.IsValidEnemy(self))
		{
			return true;
		}
		List<CharacterEntity> listActiveAlly = MonoSingleton<GameRecord>.Instance.ListActiveAlly;
		for (int i = listActiveAlly.Count - 1; i >= 0; i--)
		{
			if (listActiveAlly[i].GetCurrentTarget() != null && (!hasExceptionHero || listActiveAlly[i].GetEntityId() != exceptionId) && listActiveAlly[i].GetCurrentTarget().GetEntityId() == self.GetEntityId() && listActiveAlly[i].GetFsmController().GetCurrentState() is CharacterMeleeAtkState)
			{
				return true;
			}
		}
		return false;
	}

	public static bool IsPetHavingAtkState(HeroEntity petModel)
	{
		return petModel.IsPet && (petModel.PetConfigData.Atk_magic_min > 0 || petModel.PetConfigData.Atk_physics_min > 0);
	}

	public static void CalculateAttackPosition(CharacterEntity heroModel, EnemyData enemy, float speed, out Vector3 attackPosition, out float timeMovingToAtkPos)
	{
		float x = 0f;
		float num = heroModel.GetAttackRangeMin() + (float)enemy.OriginalParameter.body_size / GameRecord.PIXEL_PER_UNIT;
		float num2 = 0f;
		if (enemy.transform.position.x > heroModel.transform.position.x)
		{
			x = enemy.transform.position.x - num;
		}
		if (enemy.transform.position.x < heroModel.transform.position.x)
		{
			x = enemy.transform.position.x + num;
		}
		float y = enemy.transform.position.y + num2;
		attackPosition = new Vector3(x, y, 0f);
		float num3 = Vector2.Distance(heroModel.transform.position, attackPosition);
		timeMovingToAtkPos = num3 / (speed / GameRecord.PIXEL_PER_UNIT);
	}

	public static void CalculatePosition(CharacterEntity heroModel, float speed, Vector3 targetPosition, out float timeMovingToAtkPos)
	{
		float num = Vector2.Distance(heroModel.transform.position, targetPosition);
		timeMovingToAtkPos = num / (speed / GameRecord.PIXEL_PER_UNIT);
	}

	public static float MoveToAttackPosition(CharacterEntity heroModel, EnemyData enemy, float speed, Action CallbackWhencompleteMove)
	{
		Vector3 endValue;
		float num;
		GameKit.CalculateAttackPosition(heroModel, enemy, speed, out endValue, out num);
		heroModel.transform.DOMove(endValue, num, false).SetEase(Ease.Linear).OnComplete(delegate
		{
			if (CallbackWhencompleteMove != null)
			{
				CallbackWhencompleteMove();
			}
		});
		heroModel.GetAnimationController().ToRunState();
		if (endValue.x > heroModel.transform.position.x)
		{
			heroModel.transform.localScale = Vector3.one;
		}
		else
		{
			heroModel.transform.localScale = new Vector3(-1f, 1f, 1f);
		}
		return num;
	}

	public static float TimeMoveToPosition(CharacterEntity heroModel, float speed, Vector3 targetPosition, Action CallbackWhencompleteMove)
	{
		float num;
		GameKit.CalculatePosition(heroModel, speed, targetPosition, out num);
		heroModel.transform.DOMove(targetPosition, num, false).SetEase(Ease.Linear).OnComplete(delegate
		{
			if (CallbackWhencompleteMove != null)
			{
				CallbackWhencompleteMove();
			}
		});
		heroModel.GetAnimationController().ToRunState();
		return num;
	}

	public static int GetUniqueId()
	{
		GameKit._uniqueId++;
		return GameKit._uniqueId;
	}

	public static int GetTowerSourceId(int level, int id)
	{
		return level * 10 + id;
	}

	public static void SetRewardSprite(PrizeItem rewardItem, Image itemAvatar)
	{
		switch (rewardItem.rewardType)
		{
		case PrizeKind.Gem:
			itemAvatar.sprite = Common.AssetLoader.Load<Sprite>("LuckyChest/Items/lucky_item_gem");
			break;
		case PrizeKind.Life:
			itemAvatar.sprite = Common.AssetLoader.Load<Sprite>("LuckyChest/Items/lucky_item_life");
			break;
		case PrizeKind.Money:
			itemAvatar.sprite = Common.AssetLoader.Load<Sprite>("LuckyChest/Items/lucky_item_money");
			break;
		case PrizeKind.SingleHero:
			itemAvatar.sprite = Common.AssetLoader.Load<Sprite>(string.Format("LuckyChest/Items/lucky_item_hero_{0}", rewardItem.itemID));
			break;
		case PrizeKind.Item:
			itemAvatar.sprite = Common.AssetLoader.Load<Sprite>(string.Format("LuckyChest/Items/lucky_item_pw_{0}", rewardItem.itemID));
			break;
		case PrizeKind.League:
			itemAvatar.sprite = Common.AssetLoader.Load<Sprite>(string.Format("LuckyChest/Items/h{0}", rewardItem.itemID + 1));
			break;
		}
	}

	public static List<ArenaPrizeSetupRecord> GetLeagueAllPrize(int leagueIndex)
	{
		List<ArenaPrizeSetupRecord> list = new List<ArenaPrizeSetupRecord>();
		ArenaPrizeSetup tournamentPrizeConfig = ConfigRegistry.Instance.tournamentPrizeConfig;
		int num = tournamentPrizeConfig.dataArray.Length;
		for (int i = 0; i < num; i++)
		{
			if (tournamentPrizeConfig.dataArray[i].Leagueindex == leagueIndex)
			{
				list.Add(tournamentPrizeConfig.dataArray[i]);
			}
		}
		if (list.Count <= 0)
		{
			leagueIndex = 0;
			for (int j = 0; j < num; j++)
			{
				if (tournamentPrizeConfig.dataArray[j].Leagueindex == leagueIndex)
				{
					list.Add(tournamentPrizeConfig.dataArray[j]);
				}
			}
		}
		return list;
	}

	public static List<PrizeItem> GetTournamentRewardList(ArenaPrizeSetupRecord prize)
	{
		List<PrizeItem> list = new List<PrizeItem>();
		string[] array = prize.Itemtypes.Split(new char[]
		{
			','
		});
		for (int i = 0; i < array.Length; i++)
		{
			string text = array[i];
			if (text != null)
			{
				if (!(text == "Gem"))
				{
					if (!(text == "Item0"))
					{
						if (!(text == "Item1"))
						{
							if (!(text == "Item2"))
							{
								if (!(text == "Item3"))
								{
									if (text == "League")
									{
										list.Add(new PrizeItem(PrizeKind.League, prize.Leagueindex + 1, prize.Itemquantities[i], true));
									}
								}
								else
								{
									list.Add(new PrizeItem(PrizeKind.Item, 3, prize.Itemquantities[i], true));
								}
							}
							else
							{
								list.Add(new PrizeItem(PrizeKind.Item, 2, prize.Itemquantities[i], true));
							}
						}
						else
						{
							list.Add(new PrizeItem(PrizeKind.Item, 1, prize.Itemquantities[i], true));
						}
					}
					else
					{
						list.Add(new PrizeItem(PrizeKind.Item, 0, prize.Itemquantities[i], true));
					}
				}
				else
				{
					list.Add(new PrizeItem(PrizeKind.Gem, prize.Itemquantities[i], true));
				}
			}
		}
		return list;
	}

	public static int[] DecodeStringToIntArray(string s)
	{
		if (string.IsNullOrEmpty(s))
		{
			UnityEngine.Debug.LogError("DecodeStringToIntArray null or empty string");
			return new int[0];
		}
		string[] array = s.Split(new char[]
		{
			','
		});
		int num = array.Length;
		if (num > 0 && string.IsNullOrEmpty(array[num - 1]))
		{
			num--;
		}
		int[] array2 = new int[num];
		for (int i = 0; i < num; i++)
		{
			array2[i] = int.Parse(array[i]);
		}
		return array2;
	}

	public static int GetNumberOfLeagues()
	{
		return 7;
	}

	public static string ConvertIconToText(TdGlyphKey spriteId)
	{
		return string.Format("<sprite={0}>", (int)spriteId);
	}

	public static string GetPetName(int petId)
	{
		int num = petId % 1000;
		PetSetupRecord petConfigData = ConfigRegistry.Instance.petConfig.dataArray[num];
		return petConfigData.Petname;
	}

	public static string GetPetDescription(int petId)
	{
		int num = petId % 1000;
		PetSetupRecord petConfigData = ConfigRegistry.Instance.petConfig.dataArray[num];
		string[] array = new string[petConfigData.Desc_values.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = petConfigData.Desc_values[i].ToString();
		}
		return string.Format(GameKit.GetLocalization(petConfigData.Desc), array);
	}

	public static string GetLocalization(string key)
	{
		if (GameKit.keyToLocalization == null)
		{
			GameKit.keyToLocalization = new Dictionary<string, TdLingoRecord>();
			GameKit.SetCurrentLanguage(Setup.Instance.LanguageID);
			TdLingoRecord[] dataArray = ConfigRegistry.Instance.tdLocalizationConfig.dataArray;
			for (int i = dataArray.Length - 1; i >= 0; i--)
			{
				if (!string.IsNullOrEmpty(dataArray[i].Key) && !GameKit.keyToLocalization.ContainsKey(dataArray[i].Key))
				{
					GameKit.keyToLocalization.Add(dataArray[i].Key, dataArray[i]);
				}
			}
		}
		if (!GameKit.keyToLocalization.ContainsKey(key))
		{
			return string.Empty;
		}
		SystemLanguage systemLanguage = GameKit.curLanguage;
		switch (systemLanguage)
		{
		case SystemLanguage.Japanese:
			return GameKit.keyToLocalization[key].Japanese;
		case SystemLanguage.Korean:
			return GameKit.keyToLocalization[key].Korean;
		default:
			if (systemLanguage == SystemLanguage.French)
			{
				return GameKit.keyToLocalization[key].French;
			}
			if (systemLanguage == SystemLanguage.German)
			{
				return GameKit.keyToLocalization[key].German;
			}
			if (systemLanguage == SystemLanguage.Chinese)
			{
				return GameKit.keyToLocalization[key].Chinese;
			}
			if (systemLanguage == SystemLanguage.Spanish)
			{
				return GameKit.keyToLocalization[key].Spanish;
			}
			if (systemLanguage != SystemLanguage.Vietnamese)
			{
				return GameKit.keyToLocalization[key].En;
			}
			return GameKit.keyToLocalization[key].Vi;
		case SystemLanguage.Polish:
			return GameKit.keyToLocalization[key].Polish;
		case SystemLanguage.Portuguese:
			return GameKit.keyToLocalization[key].Brazil;
		case SystemLanguage.Russian:
			return GameKit.keyToLocalization[key].Russian;
		}
	}

	public static void SetCurrentLanguage(string lang)
	{
		GameKit.curLanguage = SystemLanguage.English;
		if (lang == "lg_brazil")
		{
			GameKit.curLanguage = SystemLanguage.Portuguese;
		}
		else if (lang == "lg_en")
		{
			GameKit.curLanguage = SystemLanguage.English;
		}
		else if (lang == "lg_french")
		{
			GameKit.curLanguage = SystemLanguage.French;
		}
		else if (lang == "lg_german")
		{
			GameKit.curLanguage = SystemLanguage.German;
		}
		else if (lang == "lg_korean")
		{
			GameKit.curLanguage = SystemLanguage.Korean;
		}
		else if (lang == "lg_polish")
		{
			GameKit.curLanguage = SystemLanguage.Polish;
		}
		else if (lang == "lg_russian")
		{
			GameKit.curLanguage = SystemLanguage.Russian;
		}
		else if (lang == "lg_spanish")
		{
			GameKit.curLanguage = SystemLanguage.Spanish;
		}
		else if (lang == "lg_chinese")
		{
			GameKit.curLanguage = SystemLanguage.Chinese;
		}
		else if (lang == "lg_vi")
		{
			GameKit.curLanguage = SystemLanguage.Vietnamese;
		}
		else if (lang == "lg_japanese")
		{
			GameKit.curLanguage = SystemLanguage.Japanese;
		}
	}

	public static int GetEncodedHeroList(List<int> heroList)
	{
		int num = 0;
		int num2 = Mathf.Min(heroList.Count, 3);
		for (int i = num2 - 1; i >= 0; i--)
		{
			num = num * 1000 + (heroList[i] + 1);
		}
		return num;
	}

	public static List<int> DecodeHeroList(int encodeHeroes)
	{
		List<int> list = new List<int>();
		for (int i = 0; i < 3; i++)
		{
			int item = encodeHeroes % 1000 - 1;
			list.Add(item);
			encodeHeroes /= 1000;
			if (encodeHeroes <= 0)
			{
				break;
			}
		}
		return list;
	}

	public static bool IsUltimateHero(int heroId)
	{
		return GameKit.IsPetAvailable(heroId + 1000) && HeroStore.Instance.IsPetUnlocked(heroId);
	}

	public static bool IsPetAvailable(int petId)
	{
		int num = petId % 1000;
		return ConfigRegistry.Instance.petConfig.dataArray[num].Is_available > 0;
	}

	public static DateTime FromUnixTimeToDateTime(long unixTime)
	{
		DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
		return dateTime.ToLocalTime().AddMilliseconds((double)unixTime);
	}

	public static void WriteTimeStamp(string key, DateTime localdate)
	{
		DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
		DateTime d = dateTime.ToLocalTime();
		PlayerPrefs.SetString(key, ((long)(localdate - d).TotalMilliseconds).ToString());
	}

	public static DateTime ReadTimeStamp(string key)
	{
		string @string = PlayerPrefs.GetString(key, "0");
		long unixTime;
		long.TryParse(@string, out unixTime);
		return GameKit.FromUnixTimeToDateTime(unixTime);
	}

	public static DateTime GetNow()
	{
		return UnbiasedTime.Instance.Now();
	}

	public static DateTime GetMoment0(DateTime day)
	{
		day = day.AddHours((double)(-(double)day.Hour));
		day = day.AddMinutes((double)(-(double)day.Minute));
		day = day.AddSeconds((double)(-(double)day.Second));
		return day;
	}

	public static EnemyData SummonEnemy(int enemyId, int gate)
	{
		if (enemyId < 0)
		{
			UnityEngine.Debug.LogError("Input ID < 0");
			return null;
		}
		string gameObjectName = string.Format("enemy_{0}(Clone)", enemyId);
		GameObject gameObject = Common.GameObjectPool.Spawn(gameObjectName, default(Vector3), default(Quaternion));
		EnemyData component = gameObject.GetComponent<EnemyData>();
		component.transform.parent = MonoSingleton<EnemyPool>.Instance.transform;
		MonoSingleton<GameRecord>.Instance.AddEnemyToListActiveEnemy(component);
		component.gameObject.SetActive(true);
		component.gameObject.transform.localScale = Vector3.one;
		component.gameObject.transform.rotation = Quaternion.Euler(Vector3.zero);
		component.SetDataStartRun(enemyId, (int)CrossSceneData.Instance.BattleDifficulty, gate, UnityEngine.Random.Range(0, Setup.Instance.LineCount), 0f, -1);
		component.GetFsmController();
		return component;
	}

	public static List<CharacterEntity> GetAllyInRange(Vector3 centerPos, GameKit.CustomIsValidAlly customIsValidAlly, float detectRange)
	{
		List<CharacterEntity> list = new List<CharacterEntity>();
		float num = detectRange * detectRange;
		List<CharacterEntity> listActiveAlly = MonoSingleton<GameRecord>.Instance.ListActiveAlly;
		for (int i = listActiveAlly.Count - 1; i >= 0; i--)
		{
			if (GameKit.IsValidCharacter(listActiveAlly[i]) && (centerPos - listActiveAlly[i].transform.position).sqrMagnitude <= num && customIsValidAlly(listActiveAlly[i]))
			{
				list.Add(listActiveAlly[i]);
			}
		}
		return list;
	}

	public static CharacterEntity GetAllyWithHighestScore(EnemyData enemy, GameKit.CustomIsValidAlly customIsValidAlly, float detectRange, GameKit.CustomScoreModifier customScoreModifier = null)
	{
		float num = -1000000f;
		CharacterEntity result = null;
		float num2 = detectRange * detectRange;
		List<CharacterEntity> listActiveAlly = MonoSingleton<GameRecord>.Instance.ListActiveAlly;
		for (int i = listActiveAlly.Count - 1; i >= 0; i--)
		{
			if (GameKit.IsValidCharacter(listActiveAlly[i]) && (enemy.transform.position - listActiveAlly[i].transform.position).sqrMagnitude <= num2 && customIsValidAlly(listActiveAlly[i]) && GameKit.IsCharacterVisible(listActiveAlly[i]))
			{
				float characterScore = GameKit.GetCharacterScore(listActiveAlly[i], enemy, customScoreModifier);
				if (characterScore > num)
				{
					num = characterScore;
					result = listActiveAlly[i];
				}
			}
		}
		return result;
	}

	public static float GetCharacterScore(CharacterEntity hero, EnemyData enemy, GameKit.CustomScoreModifier customScoreModifier = null)
	{
		float num = GameKit.GetEnemyHeroDistanceScore(enemy, hero);
		if (customScoreModifier != null)
		{
			num += customScoreModifier(hero, enemy);
		}
		return num;
	}

	public static EnemyData GetEnemyWithHighestScore(CharacterEntity hero)
	{
		float num = -1000000f;
		EnemyData result = null;
		List<EnemyData> listActiveEnemy = MonoSingleton<GameRecord>.Instance.ListActiveEnemy;
		for (int i = listActiveEnemy.Count - 1; i >= 0; i--)
		{
			if (!listActiveEnemy[i].IsInTunnel && (hero.IsInRangerRange(listActiveEnemy[i]) || hero.IsInMeleeRange(listActiveEnemy[i])))
			{
				float enemyScore = GameKit.GetEnemyScore(hero, listActiveEnemy[i]);
				if (enemyScore > num)
				{
					num = enemyScore;
					result = listActiveEnemy[i];
				}
			}
		}
		return result;
	}

	public static float GetEnemyScore(CharacterEntity hero, EnemyData enemy)
	{
		float num = 0f;
		if (GameKit.IsEnemyUnderAttackOfSpecificAlly(enemy, hero))
		{
			num += 20f;
		}
		num += GameKit.GetEnemyHeroDistanceScore(enemy, hero);
		if (GameKit.IsEnemyUnderAttackAndBlockingOfAnotherAlly(enemy, hero))
		{
			num -= 30f;
		}
		return num + GameKit.GetEnemyTypeBonusScore(enemy, hero);
	}

	public static bool IsEnemyUnderAttackOfSpecificAlly(EnemyData enemy, CharacterEntity hero)
	{
		return !GameKit.IsValidEnemy(enemy) || (GameKit.IsValidEnemy(hero.GetCurrentTarget()) && hero.GetCurrentTarget().GetEntityId() == enemy.GetEntityId());
	}

	public static float GetEnemyHeroDistanceScore(EnemyData enemy, CharacterEntity hero)
	{
		float num = 10f;
		float sqrMagnitude = (enemy.transform.position - hero.transform.position).sqrMagnitude;
		float num2 = hero.GetRangerRange() * hero.GetRangerRange();
		return (1f - sqrMagnitude / num2) * num;
	}

	public static float GetEnemyTypeBonusScore(EnemyData enemy, CharacterEntity hero)
	{
		float num = 0f;
		if (hero.CanAttackAirEnemy() && enemy.IsAir)
		{
			num += 5f;
		}
		return num;
	}

	public static bool IsEnemyUnderAttackAndBlockingOfAnotherAlly(EnemyData enemy, CharacterEntity hero)
	{
		if (!GameKit.IsValidEnemy(enemy))
		{
			return true;
		}
		if (enemy.EnemyFindTargetController == null)
		{
			return true;
		}
		if (enemy.EnemyFindTargetController.Target != null && enemy.EnemyFindTargetController.Target.GetEntityId() != hero.GetEntityId())
		{
			return true;
		}
		List<CharacterEntity> listActiveAlly = MonoSingleton<GameRecord>.Instance.ListActiveAlly;
		for (int i = listActiveAlly.Count - 1; i >= 0; i--)
		{
			if (listActiveAlly[i].GetEntityId() != hero.GetEntityId() && GameKit.IsValidEnemy(listActiveAlly[i].GetCurrentTarget()) && listActiveAlly[i].GetCurrentTarget().GetEntityId() == enemy.GetEntityId() && listActiveAlly[i].IsMeleeAttacking())
			{
				return true;
			}
		}
		return false;
	}

	public static void SaveListUnclaimedReward(List<SignalSetupRecord> unclaimedList)
	{
		byte[] array = new byte[unclaimedList.Count];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = (byte)unclaimedList[i].Eventid;
		}
		ObscuredPrefs.SetByteArray(GameKit.UNCLAIM_EV_LIST, array);
	}

	public static List<int> GetListUnclaimedReward()
	{
		List<int> list = new List<int>();
		byte[] byteArray = ObscuredPrefs.GetByteArray(GameKit.UNCLAIM_EV_LIST);
		for (int i = 0; i < byteArray.Length; i++)
		{
			list.Add((int)byteArray[i]);
		}
		return list;
	}

	public static void SaveListRunningEvent(List<RunningSignalRecord> listEvent)
	{
		byte[] array = new byte[listEvent.Count];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = (byte)listEvent[i].configData.Eventid;
			GameKit.SaveRunningEventProgress(listEvent[i]);
			GameKit.WriteTimeStamp(GameKit.EVENT_ENDTIME_PREFIX + listEvent[i].configData.Eventid, listEvent[i].endTime);
		}
		ObscuredPrefs.SetByteArray(GameKit.LISTEVENT, array);
	}

	public static byte[] GetListRunningEvent()
	{
		return ObscuredPrefs.GetByteArray(GameKit.LISTEVENT);
	}

	public static void SaveRunningEventProgress(RunningSignalRecord eventData)
	{
		ObscuredPrefs.SetInt(GameKit.EVENT_PREFIX + eventData.configData.Eventid, eventData.curProgress.Value);
	}

	public static void GetRunningEventProgress(RunningSignalRecord eventData)
	{
		int @int = ObscuredPrefs.GetInt(GameKit.EVENT_PREFIX + eventData.configData.Eventid, 0);
		DateTime endTime = GameKit.ReadTimeStamp(GameKit.EVENT_ENDTIME_PREFIX + eventData.configData.Eventid);
		eventData.curProgress.Value = @int;
		eventData.endTime = endTime;
	}

	public static int GetDayOfYearUpdateEvent()
	{
		return PlayerPrefs.GetInt("dayOYupdateEv", -1);
	}

	public static void SetDayOfYearUpdateEvent(int day)
	{
		PlayerPrefs.SetInt("dayOYupdateEv", day);
	}

	public static bool IsSubscriptionActive(SubscriptionType subId)
	{
		DateTime endSubscriptionTime = GameKit.GetEndSubscriptionTime(subId);
		return (endSubscriptionTime - GameKit.GetNow()).Seconds > 0;
	}

	public static void SetEndSubscriptionTime(SubscriptionType subId, DateTime localTime)
	{
		GameKit.WriteTimeStamp("subEndTime" + (int)subId, localTime);
	}

	public static DateTime GetEndSubscriptionTime(SubscriptionType subId)
	{
		return GameKit.ReadTimeStamp("subEndTime" + (int)subId);
	}

	public static void SetLastTimeCheckInSubscription(SubscriptionType subId, DateTime localTime)
	{
		GameKit.WriteTimeStamp("checksubTime" + (int)subId, localTime);
	}

	public static DateTime GetLastTimeCheckInSubscription(SubscriptionType subId)
	{
		return GameKit.ReadTimeStamp("checksubTime" + (int)subId);
	}

	public static float SquareDistance(float x1, float y1, float x2, float y2)
	{
		return (x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2);
	}

	public static float SquareDistance(Vector3 a, Vector3 b)
	{
		return GameKit.SquareDistance(a.x, a.y, b.x, b.y);
	}

	public static float SquareDistancePointSegment(float x, float z, float px, float pz, float qx, float qz)
	{
		float num = qx - px;
		float num2 = qz - pz;
		float num3 = x - px;
		float num4 = z - pz;
		float num5 = num * num + num2 * num2;
		float num6 = num * num3 + num2 * num4;
		if (num5 > 0f)
		{
			num6 /= num5;
		}
		if (num6 < 0f)
		{
			num6 = 0f;
		}
		else if (num6 > 1f)
		{
			num6 = 1f;
		}
		num3 = px + num6 * num - x;
		num4 = pz + num6 * num2 - z;
		return num3 * num3 + num4 * num4;
	}

	public static float SquareDistancePointSegment(Vector3 point, Vector3 segmentPoint1, Vector3 segmentPoint2)
	{
		return GameKit.SquareDistancePointSegment(point.x, point.y, segmentPoint1.x, segmentPoint1.y, segmentPoint2.x, segmentPoint2.y);
	}

	public static bool Left(Vector3 a, Vector3 b, Vector3 p)
	{
		return (b.x - a.x) * (p.y - a.y) - (p.x - a.x) * (b.y - a.y) <= 0f;
	}

	public static bool Left(Vector3 a, Vector3 p)
	{
		return -a.x * (p.y - a.y) - (p.x - a.x) * -a.y <= 0f;
	}

	public static bool HaveProjectOnSegment(Vector3 a, Vector3 b, Vector3 p)
	{
		Vector3 b2 = new Vector3(a.y - b.y, b.x - a.x, 0f);
		return GameKit.Left(a, a + b2, p) != GameKit.Left(b, b + b2, p);
	}

	public static Vector3 GetProjectOnLine(Vector3 a, Vector3 b, Vector3 p)
	{
		if (a.x == b.x && a.y == b.y)
		{
			a.x -= 1E-05f;
		}
		float num = (p.x - a.x) * (b.x - a.x) + (p.y - a.y) * (b.y - a.y);
		float num2 = (b.x - a.x) * (b.x - a.x) + (b.y - a.y) * (b.y - a.y);
		num /= num2;
		Vector3 result = new Vector3(a.x + num * (b.x - a.x), a.y + num * (b.y - a.y), 0f);
		return result;
	}

	public static float petDisToOwnerThreshold = 0.4f;

	public static float sqPetDisToOwnerThreshold = GameKit.petDisToOwnerThreshold * GameKit.petDisToOwnerThreshold;

	public static string pet1003BulletPath = "Bullets/hero_1003_bullet_0";

	public static string pet1003BulletName = "hero_1003_bullet_0(Clone)";

	private static string PLAY_TIME_COUNT = "playCountKe";

	public static int deltaValue = 1046527;

	private static Dictionary<string, TdLingoRecord> keyToLocalization;

	public static SystemLanguage curLanguage;

	private static int _uniqueId = 0;

	public static List<ArenaPlayerDetail> tourplayers;

	public static Dictionary<string, TourGroupInfo> allGroupInfos = new Dictionary<string, TourGroupInfo>();

	public static ArenaPlayerSelfDetail tourUserSelfInfo;

	public static ArenaSeasonDetail tourSeasonInfo;

	public static int blessedHeroId = -1;

	public static int maxUserPerTourGroup = 50;

	public static int requiredNumOfTourFriend = 5;

	public static bool isTestingSeasonReward = false;

	public static bool isTestingFriendReward = false;

	public static bool isTestingFriendRewardNoFakeUser = false;

	public static bool cachedHavingBooster = false;

	public static float cachedBoosterMultiplier = 1f;

	public static Dictionary<string, SubscriptionType> productIdToSubscriptionEnum = new Dictionary<string, SubscriptionType>
	{
		{
			"kd.sale.bundle.dailybooster",
			SubscriptionType.dailyBooster
		},
		{
			"kd.sale.bundle.boosterone",
			SubscriptionType.fiftyPercentAtkBoost
		},
		{
			"kd.sale.bundle.boostertwo",
			SubscriptionType.doubleAttack
		}
	};
}
