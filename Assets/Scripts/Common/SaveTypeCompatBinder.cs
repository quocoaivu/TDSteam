using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace Common
{
	// Save files written before the type-rename sweep embed the OLD [Serializable]
	// type names (e.g. "Data.MapSerializeData"). BinaryFormatter records each type's
	// full name in the stream, so renaming a save-graph type breaks loading old saves.
	// This binder rewrites pre-rename names to the current ones on load; new saves are
	// written with the current names and need no rewriting.
	public sealed class SaveTypeCompatBinder : SerializationBinder
	{
		// Old short type name -> current short type name. Only [Serializable] types that
		// can appear in a BinaryFormatter save graph need to be listed here. Add an entry
		// whenever the rename sweep renames another [Serializable] save type.
		private static readonly Dictionary<string, string> OldToNew = new Dictionary<string, string>
		{
			{ "BulletParameter", "ProjectileSpec" },
			{ "DailyRewardConfig", "DailyPrizeSetup" },
			{ "DailyRewardConfigData", "DailyPrizeSetupRecord" },
			{ "DailyRewardData", "DailyPrizeRecord" },
			{ "DailyRewardSerializeData", "DailyPrizeSerializeRecord" },
			{ "DailyTrialData", "DailyOrdealRecord" },
			{ "DailyTrialSerializeData", "DailyOrdealSerializeRecord" },
			{ "EndGameVideoParam", "EndGameClipSpecs" },
			{ "EventConfig", "SignalSetup" },
			{ "EventConfigData", "SignalSetupRecord" },
			{ "FreeChestOfferParam", "FreeCrateDealSpecs" },
			{ "FreeResourcesData", "FreeResourcesRecord" },
			{ "GemPack", "CrystalPack" },
			{ "GiftCode", "VoucherCode" },
			{ "GiftCodeGems", "VoucherCodeGems" },
			{ "GiftCodeHero", "VoucherCodeHero" },
			{ "GiftCodeHeroNGem", "VoucherCodeHeroNCrystal" },
			{ "GlobalUpgradeProgressData", "GlobalEnhanceProgressRecord" },
			{ "HeroData", "HeroRecord" },
			{ "HeroItem", "HeroCard" },
			{ "HeroPrepareSerializeData", "HeroPrepareSerializeRecord" },
			{ "HeroSerializeData", "HeroSerializeRecord" },
			{ "ItemConfig", "ItemSetup" },
			{ "ItemConfigData", "ItemSetupRecord" },
			{ "MapData", "ZoneRecord" },
			{ "MapSerializeData", "ZoneSerializeRecord" },
			{ "OfferData", "DealRecord" },
			{ "PlayerCurrencyData", "PlayerCoinageRecord" },
			{ "PowerUpItemData", "PowerUpItemRecord" },
			{ "SaleBundleData", "SalePackRecord" },
			{ "SerializeBundleItem", "SerializePackItem" },
			{ "TierUpgradeStatus", "TierEnhanceStanding" },
			{ "ThemeSerializeData", "SkinSerializeRecord" },
			{ "TutorialData", "TutorialRecord" },
			{ "UserProfileData", "PlayerDossierRecord" },
		};

		// Matches any old token as a whole word. Longer keys first so overlapping names
		// (e.g. GiftCodeHeroNGem vs GiftCode) resolve to the most specific match.
		private static readonly Regex OldTokenRegex = BuildRegex();

		private static Regex BuildRegex()
		{
			List<string> keys = new List<string>(OldToNew.Keys);
			keys.Sort((a, b) => b.Length - a.Length);
			return new Regex(@"\b(" + string.Join("|", keys.ToArray()) + @")\b");
		}

		public override Type BindToType(string assemblyName, string typeName)
		{
			// No pre-rename name present -> defer to the formatter's default binding
			// (returning null keeps the exact original behavior, no regression).
			if (!OldTokenRegex.IsMatch(typeName))
			{
				return null;
			}
			string rewritten = OldTokenRegex.Replace(typeName, m => OldToNew[m.Value]);
			if (rewritten == typeName)
			{
				return null;
			}
			return Resolve(assemblyName, rewritten);
		}

		private static Type Resolve(string assemblyName, string typeName)
		{
			Type type = Type.GetType(typeName + ", " + assemblyName);
			if (type != null)
			{
				return type;
			}
			type = Type.GetType(typeName);
			if (type != null)
			{
				return type;
			}
			foreach (System.Reflection.Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				type = assembly.GetType(typeName);
				if (type != null)
				{
					return type;
				}
			}
			return null;
		}
	}
}
