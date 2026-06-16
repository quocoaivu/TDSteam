using System;
using UnityEngine;

namespace GiftcodeSystem
{
	public static class VoucherCodeStaticDefine
	{
		public static string GetGiftCodeType(string json)
		{
			return JsonUtility.FromJson<VoucherCode>(json).type;
		}

		public static VoucherCodeGems GetGiftCodeGems(string json)
		{
			return JsonUtility.FromJson<VoucherCodeGems>(json);
		}

		public static VoucherCodeHero GetGiftCodeHero(string json)
		{
			return JsonUtility.FromJson<VoucherCodeHero>(json);
		}

		public static VoucherCodeHeroNCrystal GetGiftCodeHeroNGem(string json)
		{
			return JsonUtility.FromJson<VoucherCodeHeroNCrystal>(json);
		}

		public const string PREFIX_HERO = "HERO";

		public const string PREFIX_GEMS = "GEMS";

		public const string PREFIX_HERONGEM = "HERONGEM";
	}
}
