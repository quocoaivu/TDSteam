using System;
using Gameplay;

public class EnemyStateMachine : EntityFsmController
{
	public EnemyStateMachine(EnemyData enemyModel)
	{
		base.AddState(EntityPhaseEnum.EnemyMove, new EnemyMoveState(enemyModel, this));
		base.AddState(EntityPhaseEnum.EnemyWaitForAtk, new EnemyWaitForAtkState(enemyModel, this));
		base.AddState(EntityPhaseEnum.EnemyAttack, new EnemyAttackState(enemyModel, this));
		base.AddState(EntityPhaseEnum.EnemyRangeAtk, new EnemyRangeAtkState(enemyModel, this));
		base.AddState(EntityPhaseEnum.EnemyRest, new EnemyRestState(enemyModel, this));
		base.AddState(EntityPhaseEnum.EnemySpecialState, new EnemySpecialState(enemyModel, this));
		base.AddState(EntityPhaseEnum.EnemyDie, new EnemyDieState(enemyModel, this));
		base.CreateTransition(EntityPhaseEnum.EnemyMove, EntityPhaseEnum.EnemyWaitForAtk);
		base.CreateTransition(EntityPhaseEnum.EnemyMove, EntityPhaseEnum.EnemyRangeAtk);
		base.CreateTransition(EntityPhaseEnum.EnemyMove, EntityPhaseEnum.EnemyRest);
		base.CreateTransition(EntityPhaseEnum.EnemyWaitForAtk, EntityPhaseEnum.EnemyMove);
		base.CreateTransition(EntityPhaseEnum.EnemyWaitForAtk, EntityPhaseEnum.EnemyAttack);
		base.CreateTransition(EntityPhaseEnum.EnemyAttack, EntityPhaseEnum.EnemyMove);
		base.CreateTransition(EntityPhaseEnum.EnemyRangeAtk, EntityPhaseEnum.EnemyAttack);
		base.CreateTransition(EntityPhaseEnum.EnemyRangeAtk, EntityPhaseEnum.EnemyMove);
		base.CreateTransition(EntityPhaseEnum.EnemyRest, EntityPhaseEnum.EnemyMove);
		base.CreateTransition(EntityPhaseEnum.EnemySpecialState, EntityPhaseEnum.EnemyMove);
		base.CreateTransitionFromAllState(EntityPhaseEnum.EnemySpecialState, new EntityPhaseEnum[]
		{
			EntityPhaseEnum.EnemyDie
		});
		// Allow re-entering SpecialState so a periodic hold/stun (e.g. Pet1002Ability)
		// re-applied while the enemy is still held refreshes its duration instead of
		// failing silently. CreateTransitionFromAllState never adds the self-edge.
		// Mirrors PetFsmController's CharacterSpecialState self-transition.
		base.CreateTransition(EntityPhaseEnum.EnemySpecialState, EntityPhaseEnum.EnemySpecialState);
		base.CreateTransitionFromAllState(EntityPhaseEnum.EnemyDie, new EntityPhaseEnum[0]);
		base.SetInitialState(EntityPhaseEnum.EnemyMove);
	}
}
