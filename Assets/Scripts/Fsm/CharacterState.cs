using System;
using Gameplay;
using UnityEngine;

public class CharacterState : EntityState
{
    public CharacterEntity character;

    public CharacterState(CharacterEntity character, IFsmController fsmController) : base(fsmController)
	{
		this.character = character;
	}

	public override void OnInput(PhaseInputKind inputType, params object[] args)
	{
		base.OnInput(inputType, args);
		switch (inputType)
		{
		case PhaseInputKind.UserAssignPosition:
		{
			Vector3 assignedPosition = (Vector3)args[0];
			character.SetAssignedPosition(assignedPosition);
			base.SetTransition(EntityPhaseEnum.CharacterMove);
			break;
		}
		case PhaseInputKind.ThePetOwnerIsMoving:
			base.SetTransition(EntityPhaseEnum.CharacterMove);
			break;
		case PhaseInputKind.Die:
			base.SetTransition(EntityPhaseEnum.CharacterDie);
			break;
		case PhaseInputKind.SpecialState:
			base.SetTransition(EntityPhaseEnum.CharacterSpecialState);
			break;
		case PhaseInputKind.Disappear:
			base.SetTransition(EntityPhaseEnum.CharacterDisappear);
			break;
		}
	}

	public EnemyData FindTargetInRange()
	{
		return GameKit.GetEnemyWithHighestScore(character);
	}
}
