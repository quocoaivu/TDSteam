using System;

public enum EntityPhaseEnum
{
	None,
	CharacterIdle,
	CharacterWakeUp,
	CharacterMoveToTarget,
	CharacterMeleeAtk,
	CharacterRangeAtk,
	CharacterShortIdle,
	CharacterMove,
	CharacterDie,
	CharacterSpecialState,
	CharacterDisappear,
	CharacterDestroy,
	EnemyMove,
	EnemyWaitForAtk,
	EnemyAttack,
	EnemyRangeAtk,
	EnemyRest,
	EnemySpecialState,
	EnemyDie
}
