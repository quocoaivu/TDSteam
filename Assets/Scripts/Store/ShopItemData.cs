using System;
using System.Collections.Generic;
using UnityEngine;

public class ShopItemData : ScriptableObject
{
	[Header("CrystalPack Attribute")]
	public List<CrystalPack> listGemPack = new List<CrystalPack>();

	[Header("Hero Attribute")]
	public List<HeroCard> listHeroItem = new List<HeroCard>();
}
