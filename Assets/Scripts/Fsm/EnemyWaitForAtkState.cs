using System;
using Gameplay;

public class EnemyWaitForAtkState : EnemyState
{
    private CharacterEntity attackingHero;

    public EnemyWaitForAtkState(EnemyData enemyModel, IFsmController fSMController) : base(enemyModel, fSMController)
	{
	}

	public override void OnStartState()
	{
		base.OnStartState();
		if (enemyModel.EnemyFindTargetController)
		{
			attackingHero = enemyModel.EnemyFindTargetController.Target;
		}
		if (enemyModel.IsUnderground)
		{
			enemyModel.IsUnderground = false;
			enemyModel.EnemyAnimationController.ToSpawnFromGroundState();
		}
		else
		{
			enemyModel.EnemyAnimationController.ToIdleState();
		}
	}

	public override void OnInput(PhaseInputKind inputType, params object[] args)
	{
		base.OnInput(inputType, args);
		if (inputType == PhaseInputKind.HeroMeleeAttackEnemy)
		{
			CharacterEntity target = (CharacterEntity)args[0];
			enemyModel.EnemyFindTargetController.Target = target;
			base.SetTransition(EntityPhaseEnum.EnemyAttack);
		}
	}

	public override void Update(float dt)
	{
		base.Update(dt);
		if (!IsValidHeroAndReachingToAtk())
		{
			base.SetTransition(EntityPhaseEnum.EnemyMove);
		}
	}

	private bool IsValidHeroAndReachingToAtk()
	{
		return GameKit.IsValidCharacter(attackingHero) && GameKit.IsValidEnemy(attackingHero.GetCurrentTarget()) && attackingHero.GetCurrentTarget().GetEntityId() == enemyModel.GetEntityId();
	}
}
