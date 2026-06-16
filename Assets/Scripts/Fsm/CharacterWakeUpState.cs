using System;
using Gameplay;

public class CharacterWakeUpState : CharacterIdleState
{
    private float countdown;

    public CharacterWakeUpState(CharacterEntity character, IFsmController fsmController) : base(character, fsmController)
	{
	}

	public override void OnStartState()
	{
		EnemyData enemyModel = base.FindTargetInRange();
		if (enemyModel != null)
		{
			OnInput(PhaseInputKind.MonsterInAtkRange, new object[]
			{
				enemyModel
			});
		}
		else
		{
			if (!character.GetAnimationController().ContainAppearAnim())
			{
				base.SetTransition(EntityPhaseEnum.CharacterIdle);
				return;
			}
			character.GetAnimationController().ToAppearState();
		}
		countdown = 0.8f;
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
