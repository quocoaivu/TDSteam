using System;
using Gameplay;

public class CharacterIdleState : CharacterState
{
	public CharacterIdleState(CharacterEntity character, IFsmController fsmController) : base(character, fsmController)
	{
	}

	public override void OnStartState()
	{
		base.OnStartState();
		character.GetAnimationController().ToIdleState();
		EnemyData enemyModel = base.FindTargetInRange();
		if (enemyModel != null)
		{
			OnInput(PhaseInputKind.MonsterInAtkRange, new object[]
			{
				enemyModel
			});
		}
	}

	public override void OnInput(PhaseInputKind inputType, params object[] args)
	{
		base.OnInput(inputType, args);
		if (inputType == PhaseInputKind.MonsterInAtkRange)
		{
			EnemyData enemyWithHighestScore = GameKit.GetEnemyWithHighestScore(character);
			if (character.IsInMeleeRange(enemyWithHighestScore))
			{
				character.AddTarget(enemyWithHighestScore);
				base.SetTransition(EntityPhaseEnum.CharacterMoveToTarget);
			}
			else if (character.IsInRangerRange(enemyWithHighestScore))
			{
				character.AddTarget(enemyWithHighestScore);
				base.SetTransition(EntityPhaseEnum.CharacterRangeAtk);
			}
		}
	}
}
