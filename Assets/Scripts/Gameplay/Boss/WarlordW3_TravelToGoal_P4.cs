using System;

namespace Gameplay
{
	public class WarlordW3_TravelToGoal_P4 : WarlordW3BasePhase
	{
		public WarlordW3_TravelToGoal_P4(RuleEnemyWarlordW3 logicEnemy) : base(logicEnemy)
		{
			if (GameKit.IsValidEnemy(enemyModel))
			{
				enemyModel.GetFsmController().GetCurrentState().SetTransition(EntityPhaseEnum.EnemyMove);
			}
		}
	}
}
