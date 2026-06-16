using System;
using Gameplay;

public class CharacterSpecialState : CharacterState
{
    private float countdown;

    private string animationName;

    public CharacterSpecialState(CharacterEntity character, IFsmController fSMController) : base(character, fSMController)
	{
	}

	public override void OnStartState()
	{
		base.OnStartState();
		countdown = character.GetSpecialStateDuration();
		animationName = character.GetSpecialStateAnimationName();
		character.GetAnimationController().ToSpecialState(animationName, countdown);
		character.IsSpecialState = true;
	}

	public override void OnExitState()
	{
		base.OnExitState();
		character.IsSpecialState = false;
	}

	public override void Update(float dt)
	{
		base.Update(dt);
		countdown -= dt;
		if (countdown <= 0f)
		{
			base.SetTransition(EntityPhaseEnum.CharacterIdle);
		}
	}
}
