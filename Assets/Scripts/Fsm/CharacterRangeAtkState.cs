using System;
using Gameplay;

public class CharacterRangeAtkState : CharacterState
{
    private float cooldownCountdown;

    public CharacterRangeAtkState(CharacterEntity character, IFsmController fSMController) : base(character, fSMController)
	{
	}

	public override void OnStartState()
	{
		base.OnStartState();
		cooldownCountdown = 0f;
	}

	public override void Update(float dt)
	{
		base.Update(dt);
		cooldownCountdown -= dt;
		if (cooldownCountdown <= 0f)
		{
			cooldownCountdown = character.GetAtkCooldownDuration();
			character.LookAtEnemy();
			DoRangeAttack();
		}
	}

	public void DoRangeAttack()
	{
		if (!GameKit.IsValidEnemy(character.GetCurrentTarget()) || !character.IsInRangerRange(character.GetCurrentTarget()))
		{
			base.SetTransition(EntityPhaseEnum.CharacterShortIdle);
			return;
		}
		if (character.IsInMeleeRange(character.GetCurrentTarget()))
		{
			base.SetTransition(EntityPhaseEnum.CharacterMoveToTarget);
			return;
		}
		character.GetAnimationController().ToRangeAttackState();
		character.DoRangeAttack();
	}
}
