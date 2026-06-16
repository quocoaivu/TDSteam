using System;
using Gameplay;

public class PetWatchOwnerBgState : EntityState
{
    private HeroEntity petModel;

    private HeroEntity ownerModel;

    private IFsmController fsmController;

    public PetWatchOwnerBgState(CharacterEntity character, IFsmController fsmController) : base(fsmController)
	{
		petModel = (character as HeroEntity);
		ownerModel = petModel.PetOwner;
		this.fsmController = fsmController;
	}

	public override void Update(float dt)
	{
		base.Update(dt);
		if (ownerModel.IsAlive && ownerModel.GetFsmController().GetCurrentState() is CharacterMoveState && !(petModel.GetFsmController().GetCurrentState() is PetStayAroundPhase))
		{
			fsmController.GetCurrentState().OnInput(PhaseInputKind.ThePetOwnerIsMoving, new object[0]);
		}
	}
}
