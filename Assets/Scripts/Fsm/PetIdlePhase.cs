using System;
using Gameplay;
using UnityEngine;

public class PetIdlePhase : EntityState
{
    public PetStayAroundPhase parentState;

    private HeroEntity allyModel;

    private float countdownToPlay;

    public PetIdlePhase(CharacterEntity character, IFsmController fsmController, PetStayAroundPhase parentState) : base(fsmController)
	{
		this.parentState = parentState;
		allyModel = (character as HeroEntity);
	}

	public override void OnStartState()
	{
		base.OnStartState();
		allyModel.GetAnimationController().ToIdleState();
		allyModel.SetAssignedPosition(allyModel.PetOwner.transform.position + parentState.ownerToIdleSpotOffset);
		if (UnityEngine.Random.Range(0f, 1f) < 0.5f)
		{
			countdownToPlay = UnityEngine.Random.Range(0.8f, 2.1f);
		}
		else
		{
			countdownToPlay = -1f;
		}
	}

	public override void Update(float dt)
	{
		base.Update(dt);
		if ((allyModel.transform.position - (allyModel.PetOwner.transform.position + parentState.ownerToIdleSpotOffset)).sqrMagnitude > GameKit.sqPetDisToOwnerThreshold)
		{
			parentState.OnInput(PhaseInputKind.PetFarFromOwner, new object[0]);
		}
		if (countdownToPlay >= 0f)
		{
			countdownToPlay -= dt;
			if (countdownToPlay < 0f)
			{
				allyModel.GetAnimationController().ToPlayState();
			}
		}
	}
}
