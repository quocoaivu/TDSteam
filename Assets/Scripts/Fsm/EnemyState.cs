using System;
using Gameplay;

public class EnemyState : EntityState
{
    public EnemyData enemyModel;

    public EnemyState(EnemyData enemyModel, IFsmController fSMController) : base(fSMController)
	{
		this.enemyModel = enemyModel;
	}

	public override void OnInput(PhaseInputKind inputType, params object[] args)
	{
		base.OnInput(inputType, args);
		if (inputType == PhaseInputKind.SpecialState)
		{
			base.SetTransition(EntityPhaseEnum.EnemySpecialState);
		}
	}
}
