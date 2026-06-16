using System;
using Gameplay;

public class EnemySpecialState : EnemyState
{

    private float countdown;

    public EnemySpecialState(EnemyData enemyModel, IFsmController fsmController) : base(enemyModel, fsmController)
	{
	}

	public override void OnStartState()
	{
		base.OnStartState();
		countdown = enemyModel.GetSpecialStateDuration();
	}

	public override void Update(float dt)
	{
		base.Update(dt);
		countdown -= dt;
		if (countdown <= 0f)
		{
			base.SetTransition(EntityPhaseEnum.EnemyMove);
		}
	}
}
