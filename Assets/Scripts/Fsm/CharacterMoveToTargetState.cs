using System;
using DG.Tweening;
using Gameplay;

public class CharacterMoveToTargetState : CharacterState
{
    private EnemyData enemy;

    public CharacterMoveToTargetState(CharacterEntity character, IFsmController fSMController) : base(character, fSMController)
	{
	}

	public override void OnStartState()
	{
		base.OnStartState();
		enemy = character.GetCurrentTarget();
		if (enemy.EnemyFindTargetController)
		{
			enemy.EnemyFindTargetController.AddTarget(character);
		}
		enemy.enemyFsmController.GetCurrentState().OnInput(PhaseInputKind.SetEnemyIdleWaitForMeleeAtk, new object[]
		{
			character
		});
		character.GetAnimationController().ToRunState();
		GameKit.MoveToAttackPosition(character, enemy, character.GetSpeed(), new Action(MoveToAttackPositionComplete));
	}

	public override void OnExitState()
	{
		base.OnExitState();
		CancelMoveToAtkPos();
	}

	public override void OnInput(PhaseInputKind inputType, params object[] args)
	{
		base.OnInput(inputType, args);
		if (inputType == PhaseInputKind.MonsterInAtkRange)
		{
			EnemyData enemyWithHighestScore = GameKit.GetEnemyWithHighestScore(character);
			// No valid target in range (e.g. only candidate just entered a tunnel/died):
			// scorer returns null. Bail out like Idle/MeleeAtk do, instead of NRE.
			if (enemyWithHighestScore == null)
			{
				return;
			}
			if (enemyWithHighestScore.GetEntityId() != character.GetCurrentTarget().GetEntityId())
			{
				CancelMoveToAtkPos();
				character.AddTarget(enemyWithHighestScore);
				enemy = enemyWithHighestScore;
				GameKit.MoveToAttackPosition(character, enemy, character.GetSpeed(), new Action(MoveToAttackPositionComplete));
			}
		}
	}

	private void CancelMoveToAtkPos()
	{
		character.transform.DOKill(false);
	}

	private void MoveToAttackPositionComplete()
	{
		base.SetTransition(EntityPhaseEnum.CharacterMeleeAtk);
	}

	public override void Update(float dt)
	{
		base.Update(dt);
		if (!character.IsInMeleeRange(enemy))
		{
			character.AddTarget(null);
			base.SetTransition(EntityPhaseEnum.CharacterShortIdle);
		}
	}
}
