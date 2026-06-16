using System;
using Gameplay;

public class HeroFsmHandler : EntityFsmController
{
	public HeroFsmHandler(CharacterEntity character)
	{
		base.AddState(EntityPhaseEnum.CharacterIdle, new CharacterIdleState(character, this));
		base.AddState(EntityPhaseEnum.CharacterRangeAtk, new CharacterRangeAtkState(character, this));
		base.AddState(EntityPhaseEnum.CharacterMoveToTarget, new CharacterMoveToTargetState(character, this));
		base.AddState(EntityPhaseEnum.CharacterMeleeAtk, new CharacterMeleeAtkState(character, this));
		base.AddState(EntityPhaseEnum.CharacterShortIdle, new CharacterShortIdleState(character, this));
		base.AddState(EntityPhaseEnum.CharacterMove, new CharacterMoveState(character, this));
		base.AddState(EntityPhaseEnum.CharacterDie, new CharacterDieState(character, this, EntityPhaseEnum.CharacterIdle));
		base.AddState(EntityPhaseEnum.CharacterSpecialState, new CharacterSpecialState(character, this));
		base.AddBackgroundState(new DetectEnemyBgState(character, this));
		base.CreateTransition(EntityPhaseEnum.CharacterIdle, EntityPhaseEnum.CharacterRangeAtk);
		base.CreateTransition(EntityPhaseEnum.CharacterIdle, EntityPhaseEnum.CharacterMoveToTarget);
		base.CreateTransition(EntityPhaseEnum.CharacterRangeAtk, EntityPhaseEnum.CharacterMoveToTarget);
		base.CreateTransition(EntityPhaseEnum.CharacterRangeAtk, EntityPhaseEnum.CharacterShortIdle);
		base.CreateTransition(EntityPhaseEnum.CharacterMoveToTarget, EntityPhaseEnum.CharacterMeleeAtk);
		base.CreateTransition(EntityPhaseEnum.CharacterMoveToTarget, EntityPhaseEnum.CharacterShortIdle);
		base.CreateTransition(EntityPhaseEnum.CharacterMeleeAtk, EntityPhaseEnum.CharacterMoveToTarget);
		base.CreateTransition(EntityPhaseEnum.CharacterMeleeAtk, EntityPhaseEnum.CharacterShortIdle);
		base.CreateTransition(EntityPhaseEnum.CharacterMeleeAtk, EntityPhaseEnum.CharacterRangeAtk);
		base.CreateTransition(EntityPhaseEnum.CharacterShortIdle, EntityPhaseEnum.CharacterMoveToTarget);
		base.CreateTransition(EntityPhaseEnum.CharacterShortIdle, EntityPhaseEnum.CharacterRangeAtk);
		base.CreateTransition(EntityPhaseEnum.CharacterMove, EntityPhaseEnum.CharacterIdle);
		base.CreateTransition(EntityPhaseEnum.CharacterMove, EntityPhaseEnum.CharacterMove);
		base.CreateTransition(EntityPhaseEnum.CharacterDie, EntityPhaseEnum.CharacterIdle);
		base.CreateTransition(EntityPhaseEnum.CharacterSpecialState, EntityPhaseEnum.CharacterIdle);
		base.CreateTransitionFromAllState(EntityPhaseEnum.CharacterMove, new EntityPhaseEnum[]
		{
			EntityPhaseEnum.CharacterDie,
			EntityPhaseEnum.CharacterSpecialState
		});
		base.CreateTransitionFromAllState(EntityPhaseEnum.CharacterDie, new EntityPhaseEnum[0]);
		base.CreateTransitionFromAllState(EntityPhaseEnum.CharacterSpecialState, new EntityPhaseEnum[]
		{
			EntityPhaseEnum.CharacterDie
		});
		base.SetInitialState(EntityPhaseEnum.CharacterIdle);
	}
}
