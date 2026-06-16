using System;
using Gameplay;

public class CharacterMeleeAtkState : CharacterState
{
    private float cooldownCountdown;

    public CharacterMeleeAtkState(CharacterEntity character, IFsmController fSMController) : base(character, fSMController)
	{
	}

	public override void OnStartState()
	{
		base.OnStartState();
		cooldownCountdown = 0f;
	}

	public override void Update(float dt)
	{
		base.Update(dt);
		cooldownCountdown -= dt;
		if (cooldownCountdown <= 0f)
		{
			cooldownCountdown = character.GetAtkCooldownDuration();
			DoMeleeAttack();
		}
	}

	public override void OnInput(PhaseInputKind inputType, params object[] args)
	{
		base.OnInput(inputType, args);
		if (inputType == PhaseInputKind.MonsterInAtkRange)
		{
			EnemyData enemyWithHighestScore = GameKit.GetEnemyWithHighestScore(character);
			if (GameKit.IsValidEnemy(enemyWithHighestScore) && enemyWithHighestScore.GetEntityId() != character.GetCurrentTarget().GetEntityId())
			{
				character.AddTarget(enemyWithHighestScore);
				if (character.IsInMeleeRange(enemyWithHighestScore))
				{
					base.SetTransition(EntityPhaseEnum.CharacterMoveToTarget);
				}
				else
				{
					base.SetTransition(EntityPhaseEnum.CharacterRangeAtk);
				}
			}
		}
	}

	public void DoMeleeAttack()
	{
		EnemyData currentTarget = character.GetCurrentTarget();
		if (!GameKit.IsValidEnemy(currentTarget) || !character.IsInMeleeActionRange(currentTarget))
		{
			base.SetTransition(EntityPhaseEnum.CharacterShortIdle);
			return;
		}
		currentTarget.enemyFsmController.GetCurrentState().OnInput(PhaseInputKind.HeroMeleeAttackEnemy, new object[]
		{
			character
		});
		character.GetAnimationController().ToMeleeAttackState();
		character.DoMeleeAttack();
		character.LookAtEnemy();
	}
}
