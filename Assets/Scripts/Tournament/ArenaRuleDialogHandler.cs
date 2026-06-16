using System;
using System.Collections.Generic;
using Gameplay;
using UnityEngine;
using UnityEngine.UI;

namespace Tournament
{
	public class ArenaRuleDialogHandler : GameplayDialogHandler
	{
		private void Start()
		{
			List<ArenaPrizeSetupRecord> leagueAllPrize = GameKit.GetLeagueAllPrize(1000);
			int num = leagueAllPrize[0].Itemquantities[0];
			friendRuleText.text = string.Format(GameKit.GetLocalization("RULE_CONTENT_2"), num);
		}

		[Header("Rule content 2")]
		public Text friendRuleText;
	}
}
