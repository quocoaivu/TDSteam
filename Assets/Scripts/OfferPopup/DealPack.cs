using System;
using System.Collections.Generic;
using UnityEngine;

public class DealPack : ScriptableObject
{
	[Space]
	public List<DealPackSingleHero> bundlesSingleHero = new List<DealPackSingleHero>();

	[Space]
	public List<DealPackCrystalNItems> bundlesItems = new List<DealPackCrystalNItems>();

	[Space]
	public List<OfferBundleComboHeroes> bundlesComboHeroes = new List<OfferBundleComboHeroes>();
}
