using System;
using Gameplay;
using UnityEngine;

public class PetFollowPhase : EntityState
{
    public PetStayAroundPhase parentState;

    private HeroEntity allyModel;

    private float farThreshold = 3f;

    private float sqFarThreshold;

    private float sqIdleDisThreshold;

    private float moveSpeed;

    private Vector3 invertXVector = new Vector3(-1f, 1f, 1f);

    public PetFollowPhase(CharacterEntity character, IFsmController fsmController, PetStayAroundPhase parentState) : base(fsmController)
	{
		this.parentState = parentState;
		allyModel = (character as HeroEntity);
		sqIdleDisThreshold = GameKit.sqPetDisToOwnerThreshold * 0.5f;
		moveSpeed = allyModel.GetSpeed() / GameRecord.PIXEL_PER_UNIT;
		sqFarThreshold = farThreshold * farThreshold;
	}

	public override void OnStartState()
	{
		base.OnStartState();
		allyModel.GetAnimationController().ToRunState();
	}

	public override void Update(float dt)
	{
		base.Update(dt);
		Vector3 vector = allyModel.PetOwner.transform.position + parentState.ownerToIdleSpotOffset - allyModel.transform.position;
		float sqrMagnitude = vector.sqrMagnitude;
		if (sqrMagnitude <= sqIdleDisThreshold)
		{
			parentState.OnInput(PhaseInputKind.PetReachFollowSpot, new object[0]);
			return;
		}
		Vector3 normalized = vector.normalized;
		allyModel.transform.localScale = ((normalized.x <= 0f) ? invertXVector : Vector3.one);
		allyModel.transform.position += normalized * dt * moveSpeed * (float)((sqrMagnitude <= sqFarThreshold) ? 1 : 2);
	}
}
