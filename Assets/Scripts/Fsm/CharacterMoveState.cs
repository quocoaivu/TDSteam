using System;
using DG.Tweening;
using Gameplay;
using UnityEngine;

public class CharacterMoveState : CharacterState
{
    private Vector3 invertXVector = new Vector3(-1f, 1f, 1f);

    public CharacterMoveState(CharacterEntity character, IFsmController fSMController) : base(character, fSMController)
	{
	}

	public override void OnStartState()
	{
		base.OnStartState();
		character.AddTarget(null);
		character.GetAnimationController().ToRunState();
		MoveToAssignedPosition();
	}

	public override void OnExitState()
	{
		base.OnExitState();
		character.transform.DOKill(false);
	}

	private void MoveToAssignedPosition()
	{
		float duration = Vector2.Distance(character.transform.position, character.GetAssignedPosition()) / (character.GetSpeed() / GameRecord.PIXEL_PER_UNIT);
		character.transform.DOMove(character.GetAssignedPosition(), duration, false).SetEase(Ease.Linear).OnComplete(new TweenCallback(MoveToAssignedPositionComplete));
		character.GetAnimationController().ToRunState();
		ChangeAnimationRun(character.transform.position, character.GetAssignedPosition());
	}

	private void MoveToAssignedPositionComplete()
	{
		base.SetTransition(EntityPhaseEnum.CharacterIdle);
	}

	private void ChangeAnimationRun(Vector3 currentPosition, Vector3 assignedPosition)
	{
		if (assignedPosition.x - currentPosition.x > 0f)
		{
			character.transform.localScale = Vector3.one;
		}
		else
		{
			character.transform.localScale = invertXVector;
		}
	}
}
