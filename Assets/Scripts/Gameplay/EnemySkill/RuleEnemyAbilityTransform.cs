using System;

namespace Gameplay
{
	public class RuleEnemyAbilityTransform : EnemyBrain
	{
		public void TransformUndergrounToBattleground()
		{
			base.EnemyModel.IsUnderground = false;
		}
	}
}
