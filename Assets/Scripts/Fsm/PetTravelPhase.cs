using System;
using Gameplay;

public class PetTravelPhase : CharacterState
{
	public PetTravelPhase(CharacterEntity character, IFsmController fSMController) : base(character, fSMController)
	{
	}

	public override void OnStartState()
	{
		base.OnStartState();
		base.SetTransition(EntityPhaseEnum.CharacterIdle);
	}
}
