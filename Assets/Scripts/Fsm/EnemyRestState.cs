using System;
using Gameplay;
using UnityEngine;

public class EnemyRestState : EnemyState
{
    private float minRestDur;

    private float maxRestDur;

    private float restCountdown;

    public EnemyRestState(EnemyData enemyModel, IFsmController fsmController) : base(enemyModel, fsmController)
	{
		minRestDur = enemyModel.EnemyMovementController.MinRestDuration;
		maxRestDur = enemyModel.EnemyMovementController.MaxRestDuration;
	}

	public override void OnStartState()
	{
		base.OnStartState();
		restCountdown = UnityEngine.Random.Range(minRestDur, maxRestDur);
	}

	public override void Update(float dt)
	{
		base.Update(dt);
		restCountdown -= dt;
		if (restCountdown <= 0f)
		{
			base.SetTransition(EntityPhaseEnum.EnemyMove);
		}
	}
}
