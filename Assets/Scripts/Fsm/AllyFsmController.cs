using System;
using Gameplay;

public class AllyFsmController : EntityFsmController
{
	public AllyFsmController(CharacterEntity character)
	{
		base.AddState(EntityPhaseEnum.CharacterIdle, new CharacterIdleState(character, this));
		base.AddState(EntityPhaseEnum.CharacterWakeUp, new CharacterWakeUpState(character, this));
		base.AddState(EntityPhaseEnum.CharacterRangeAtk, new CharacterRangeAtkState(character, this));
		base.AddState(EntityPhaseEnum.CharacterMoveToTarget, new CharacterMoveToTargetState(character, this));
		base.AddState(EntityPhaseEnum.CharacterMeleeAtk, new CharacterMeleeAtkState(character, this));
		base.AddState(EntityPhaseEnum.CharacterShortIdle, new CharacterShortIdleState(character, this));
		base.AddState(EntityPhaseEnum.CharacterMove, new CharacterMoveState(character, this));
		base.AddState(EntityPhaseEnum.CharacterDie, new CharacterDieState(character, this, EntityPhaseEnum.CharacterDestroy));
		base.AddState(EntityPhaseEnum.CharacterDisappear, new CharacterDisappearState(character, this, false));
		base.AddState(EntityPhaseEnum.CharacterDestroy, new CharacterDisappearState(character, this, true));
		base.AddState(EntityPhaseEnum.CharacterSpecialState, new CharacterSpecialState(character, this));
		base.AddBackgroundState(new DetectEnemyBgState(character, this));
		base.CreateTransition(EntityPhaseEnum.CharacterIdle, EntityPhaseEnum.CharacterRangeAtk);
		base.CreateTransition(EntityPhaseEnum.CharacterIdle, EntityPhaseEnum.CharacterMoveToTarget);
		base.CreateTransition(EntityPhaseEnum.CharacterWakeUp, EntityPhaseEnum.CharacterIdle);
		base.CreateTransition(EntityPhaseEnum.CharacterWakeUp, EntityPhaseEnum.CharacterRangeAtk);
		base.CreateTransition(EntityPhaseEnum.CharacterWakeUp, EntityPhaseEnum.CharacterMoveToTarget);
		base.CreateTransition(EntityPhaseEnum.CharacterRangeAtk, EntityPhaseEnum.CharacterMoveToTarget);
		base.CreateTransition(EntityPhaseEnum.CharacterRangeAtk, EntityPhaseEnum.CharacterShortIdle);
		base.CreateTransition(EntityPhaseEnum.CharacterMoveToTarget, EntityPhaseEnum.CharacterMeleeAtk);
		base.CreateTransition(EntityPhaseEnum.CharacterMoveToTarget, EntityPhaseEnum.CharacterShortIdle);
		base.CreateTransition(EntityPhaseEnum.CharacterMeleeAtk, EntityPhaseEnum.CharacterMoveToTarget);
		base.CreateTransition(EntityPhaseEnum.CharacterMeleeAtk, EntityPhaseEnum.CharacterShortIdle);
		// Range-unlocked minions (TurretSummonMinionHandler.UnlockRangeAttackAbility)
		// can switch from melee to ranged when a new target appears out of melee range;
		// CharacterMeleeAtkState emits this transition. Hero/Pet graphs already declare it.
		base.CreateTransition(EntityPhaseEnum.CharacterMeleeAtk, EntityPhaseEnum.CharacterRangeAtk);
		base.CreateTransition(EntityPhaseEnum.CharacterShortIdle, EntityPhaseEnum.CharacterMoveToTarget);
		base.CreateTransition(EntityPhaseEnum.CharacterShortIdle, EntityPhaseEnum.CharacterRangeAtk);
		base.CreateTransition(EntityPhaseEnum.CharacterMove, EntityPhaseEnum.CharacterIdle);
		base.CreateTransition(EntityPhaseEnum.CharacterMove, EntityPhaseEnum.CharacterMove);
		base.CreateTransition(EntityPhaseEnum.CharacterDie, EntityPhaseEnum.CharacterDestroy);
		base.CreateTransition(EntityPhaseEnum.CharacterSpecialState, EntityPhaseEnum.CharacterIdle);
		base.CreateTransitionFromAllState(EntityPhaseEnum.CharacterMove, new EntityPhaseEnum[]
		{
			EntityPhaseEnum.CharacterDie,
			EntityPhaseEnum.CharacterDestroy,
			EntityPhaseEnum.CharacterDisappear
		});
		base.CreateTransitionFromAllState(EntityPhaseEnum.CharacterDie, new EntityPhaseEnum[]
		{
			EntityPhaseEnum.CharacterDisappear,
			EntityPhaseEnum.CharacterDestroy
		});
		base.CreateTransitionFromAllState(EntityPhaseEnum.CharacterDisappear, new EntityPhaseEnum[]
		{
			EntityPhaseEnum.CharacterDestroy,
			EntityPhaseEnum.CharacterDie
		});
		base.CreateTransitionFromAllState(EntityPhaseEnum.CharacterSpecialState, new EntityPhaseEnum[]
		{
			EntityPhaseEnum.CharacterDie,
			EntityPhaseEnum.CharacterDisappear,
			EntityPhaseEnum.CharacterDestroy
		});
		base.SetInitialState(EntityPhaseEnum.CharacterWakeUp);
	}
}
