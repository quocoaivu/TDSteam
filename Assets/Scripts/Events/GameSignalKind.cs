using System;

public enum GameSignalKind
{
	OnTournamentMapRuleReceived = 4,
	OnTournamentTierUp,
	OnLanguageChanged,
	OnTournamentPriceConstantsReceived,
	OnCompletePurchase,
	OnEnemyMoveToEndPoint = 1001,
	OnSelectHero = 2001,
	OnSelectEnemy,
	OnSelectPet,
	OnSelectAlly = 3001,
	OnClickButton = 4001,
	OnAfterCalculatePhysicsDamage = 5001,
	OnAfterCalculateMagicDamage,
	OnBeforeCalculatePhysicsDamage,
	EventKillMonster = 6001,
	EventCampaign,
	EventUseItem,
	EventUseHero,
	EventPlayTournament,
	EventInviteFriend,
	EventEarnGold,
	EventUseGem
}
