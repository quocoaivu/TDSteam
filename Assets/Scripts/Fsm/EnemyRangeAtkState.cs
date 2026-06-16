using System;
using Gameplay;

public class EnemyRangeAtkState : EnemyState
{
    private float cooldownDur;

    private float countdownCooldown;

    private CharacterEntity attackingHero;

    private float atkRange;

    private float sqAtkRange;

    public EnemyRangeAtkState(EnemyData enemyModel, IFsmController fsmController) : base(enemyModel, fsmController)
	{
		if (enemyModel.enemyAttackController)
		{
			cooldownDur = enemyModel.enemyAttackController.GetCooldownTime();
			atkRange = enemyModel.enemyAttackController.GetRangerAtkRange();
			sqAtkRange = atkRange * atkRange;
		}
	}

	public override void OnStartState()
	{
		base.OnStartState();
		attackingHero = enemyModel.EnemyFindTargetController.Target;
		countdownCooldown = 0f;
	}

	public override void Update(float dt)
	{
		base.Update(dt);
		countdownCooldown -= dt;
		if (countdownCooldown <= 0f)
		{
			countdownCooldown = cooldownDur;
			if (!GameKit.IsValidCharacter(attackingHero) || (attackingHero.transform.position - enemyModel.transform.position).sqrMagnitude > sqAtkRange || !GameKit.IsCharacterVisible(attackingHero) || enemyModel.IsInTunnel)
			{
				base.SetTransition(EntityPhaseEnum.EnemyMove);
				return;
			}
			enemyModel.enemyAttackController.PrepareToRangeAttack();
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
}
