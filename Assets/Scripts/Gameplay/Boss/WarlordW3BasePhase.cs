using System;

namespace Gameplay
{
	public class WarlordW3BasePhase
	{
        public RuleEnemyWarlordW3 logicEnemy;

        public EnemyData enemyModel;

        public WarlordW3BasePhase(RuleEnemyWarlordW3 logicEnemy)
		{
			this.logicEnemy = logicEnemy;
			enemyModel = logicEnemy.EnemyModel;
		}

		public virtual void OnUpdate(float dt)
		{
		}
	}
}
