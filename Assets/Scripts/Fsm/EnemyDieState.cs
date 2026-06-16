using System;
using Gameplay;

public class EnemyDieState : EnemyState
{
    private float delayDestroy;

    public EnemyDieState(EnemyData enemyModel, IFsmController fsmController) : base(enemyModel, fsmController)
	{
		delayDestroy = enemyModel.GetDieDuration();
	}

	public override void OnStartState()
	{
		base.OnStartState();
		enemyModel.EnemyAnimationController.ToDieState();
		enemyModel.ReturnPool(delayDestroy);
	}
}
