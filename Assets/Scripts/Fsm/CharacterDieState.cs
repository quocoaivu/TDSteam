using System;
using Gameplay;

public class CharacterDieState : CharacterState
{
    private EntityPhaseEnum defaultReturnState;

    public CharacterDieState(CharacterEntity character, IFsmController fSMController, EntityPhaseEnum defaultReturnState) : base(character, fSMController)
	{
		this.defaultReturnState = defaultReturnState;
	}

	public override void OnStartState()
	{
		base.OnStartState();
		character.GetAnimationController().ToDieState();
	}

	// No Update tick here: the owner sets IsAlive=false on death, which gates off
	// CharacterEntity.Update, so the FSM is not driven while dead. Returning to life
	// is triggered externally via the Resurge input (scheduled by HeroModel.Dead).
	public override void OnInput(PhaseInputKind inputType, params object[] args)
	{
		base.OnInput(inputType, args);
		if (inputType == PhaseInputKind.Resurge)
		{
			base.SetTransition(defaultReturnState);
		}
	}
}
