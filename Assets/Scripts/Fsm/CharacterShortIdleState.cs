using System;
using Gameplay;

public class CharacterShortIdleState : CharacterIdleState
{
    private float idleCountdown;

    public CharacterShortIdleState(CharacterEntity character, IFsmController fSMController) : base(character, fSMController)
	{
	}

	public override void OnStartState()
	{
		base.OnStartState();
		idleCountdown = character.GetShortIdleDuration();
	}

	public override void Update(float dt)
	{
		base.Update(dt);
		idleCountdown -= dt;
		if (idleCountdown <= 0f)
		{
			base.SetTransition(EntityPhaseEnum.CharacterMove);
		}
	}
}
