using System;
using System.Collections.Generic;
using Gameplay;

public class DetectEnemyBgState : EntityState
{
    private float delayCheckNewEnemy = 0.12f;

    private float countdown;

    private HashSet<int> enemiesInRangeLastCycle = new HashSet<int>();

    private HashSet<int> enemiesInRangeThisCycle = new HashSet<int>();

    public CharacterEntity character;

    public DetectEnemyBgState(CharacterEntity character, IFsmController fsmController) : base(fsmController)
	{
		this.character = character;
	}

	public override void Update(float dt)
	{
		base.Update(dt);
		countdown -= dt;
		if (countdown <= 0f)
		{
			countdown += delayCheckNewEnemy;
			DetectNewEnemyInRange();
			if (!character.IsAlive && !(character.GetFsmController().GetCurrentState() is CharacterDieState))
			{
				character.Dead();
			}
		}
	}

	public void DetectNewEnemyInRange()
	{
		enemiesInRangeThisCycle.Clear();
		List<EnemyData> listActiveEnemy = MonoSingleton<GameRecord>.Instance.ListActiveEnemy;
		for (int i = listActiveEnemy.Count - 1; i >= 0; i--)
		{
			// Skip tunnel enemies here too: GameKit.GetEnemyWithHighestScore filters them
			// out, so notifying about one (when it's the only in-range enemy) would make the
			// scorer return null and NRE downstream. Keep this filter aligned with the scorer.
			if (listActiveEnemy[i].IsInTunnel)
			{
				continue;
			}
			if (character.IsInRangerRange(listActiveEnemy[i]) || character.IsInMeleeRange(listActiveEnemy[i]))
			{
				int instanceID = listActiveEnemy[i].GetEntityId();
				enemiesInRangeThisCycle.Add(instanceID);
				// Only notify when the enemy just entered range (was not in range last cycle).
				if (!enemiesInRangeLastCycle.Contains(instanceID))
				{
					character.GetFsmController().GetCurrentState().OnInput(PhaseInputKind.MonsterInAtkRange, new object[]
					{
						listActiveEnemy[i]
					});
				}
			}
		}
		// Swap sets so dead/out-of-range enemies drop out; reuse the old set to avoid allocations.
		HashSet<int> temp = enemiesInRangeLastCycle;
		enemiesInRangeLastCycle = enemiesInRangeThisCycle;
		enemiesInRangeThisCycle = temp;
	}
}
