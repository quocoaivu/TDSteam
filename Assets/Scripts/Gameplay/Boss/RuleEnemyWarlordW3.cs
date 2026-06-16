using System;
using MetaGame;
using UnityEngine;

namespace Gameplay
{
	public class RuleEnemyWarlordW3 : EnemyBrain
	{
		public override void Initialize()
		{
			base.Initialize();
			summonRoutineCountdown = 10;
			Common.GameObjectPool.InitPool("Enemies/enemy_18", 0);
			Common.GameObjectPool.InitPool("Enemies/enemy_20", 0);
			Common.GameObjectPool.InitPool("Enemies/enemy_21", 0);
		}

		public override void OnAppear()
		{
			base.OnAppear();
			curState = new WarlordW3_Summon_P0(this);
		}

		public override void Update()
		{
			base.Update();
			if (GameKit.IsValidEnemy(base.EnemyModel))
			{
				curState.OnUpdate(Time.deltaTime);
			}
		}

		public CreepPathAnchor GetRandomPosOnRoad(float minLineProp, float maxLineProp)
		{
			int gate = UnityEngine.Random.Range(0, LineDirector.Current.listGates.Count);
			int line = UnityEngine.Random.Range(0, Setup.Instance.LineCount);
			LineRecord line2 = LineDirector.Current.GetLine(gate, line);
			float num = UnityEngine.Random.Range(minLineProp, maxLineProp);
			float num2 = line2.Length * num;
			int pathSegmentId = 0;
			for (int i = 0; i < line2.segmentLengths.Length; i++)
			{
				if (line2.segmentLengths[i] >= num2)
				{
					pathSegmentId = i;
					break;
				}
				num2 -= line2.segmentLengths[i];
			}
			return new CreepPathAnchor(LineDirector.Current.GetLineIndex(gate, line), pathSegmentId, num2);
		}

		public static string animRunRight = "Run-Right";

		public static string animRunUp = "Run-Up";

		public static string animBirdRunRight = "Bird-Run-Right";

		public static string animAttackRange = "RangeAttack";

		public static string animSpecialAttack = "SpecialAttack";

		public static string animDie = "Die";

		public static string animIdle = "Idle";

		public static string animTurnToBird = "TurnToBird";

		public static string animTurnToBoss = "TurnToBoss";

		public Animator animator;

		public GameObject behitTurnAllyPrefab;

		public GameObject monsterTransformationPrefab;

		public GameObject summonEffectPrefab;

		public float teleSpeed;

		[HideInInspector]
		public WarlordW3BasePhase curState;

		[HideInInspector]
		public int summonRoutineCountdown;
	}
}
