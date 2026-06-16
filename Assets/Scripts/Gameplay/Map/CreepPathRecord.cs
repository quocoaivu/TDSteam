using System;
using UnityEngine;

namespace Gameplay
{
	public class CreepPathRecord
	{
		public CreepPathRecord(int lineIndex, Action OnCompletePath = null)
		{
			this.OnCompletePath = OnCompletePath;
			curLineIndex = lineIndex;
			firstAnchor = new CreepPathAnchor(curLineIndex, 0, 0f);
			secondAnchor = new CreepPathAnchor(curLineIndex, 0, 0f);
			LineDirector.Current.MoveMonsterAnchor(firstAnchor, -0.4f);
			LineDirector.Current.MoveMonsterAnchor(secondAnchor, 0.4f);
		}

		public CreepPathRecord(Vector3 curPos, Action OnCompletePath = null)
		{
			this.OnCompletePath = OnCompletePath;
			CreepPathAnchor monsterPathAnchor;
			LineDirector.Current.FindNearestLine(curPos, out curLineIndex, out offset, out monsterPathAnchor);
			firstAnchor = new CreepPathAnchor(curLineIndex, monsterPathAnchor.pathSegmentId, monsterPathAnchor.lenProgress);
			secondAnchor = new CreepPathAnchor(curLineIndex, monsterPathAnchor.pathSegmentId, monsterPathAnchor.lenProgress);
			LineDirector.Current.MoveMonsterAnchor(firstAnchor, -0.4f);
			LineDirector.Current.MoveMonsterAnchor(secondAnchor, 0.4f);
		}

		public CreepPathRecord(CreepPathAnchor pos, Action OnCompletePath = null)
		{
			this.OnCompletePath = OnCompletePath;
			curLineIndex = pos.curLineIndex;
			offset = Vector3.zero;
			firstAnchor = new CreepPathAnchor(curLineIndex, pos.pathSegmentId, pos.lenProgress);
			secondAnchor = new CreepPathAnchor(curLineIndex, pos.pathSegmentId, pos.lenProgress);
			LineDirector.Current.MoveMonsterAnchor(firstAnchor, -0.4f);
			LineDirector.Current.MoveMonsterAnchor(secondAnchor, 0.4f);
		}

		public CreepPathRecord(int lineIndex, Vector3 curPos, Action OnCompletePath = null)
		{
			this.OnCompletePath = OnCompletePath;
			curLineIndex = lineIndex;
			LineRecord line = LineDirector.Current.GetLine(curLineIndex);
			int num = -1;
			float num2 = float.PositiveInfinity;
			for (int i = line.Path.Count - 2; i >= 0; i--)
			{
				if (GameKit.HaveProjectOnSegment(line.Path[i], line.Path[i + 1], curPos))
				{
					float num3 = GameKit.SquareDistancePointSegment(curPos, line.Path[i], line.Path[i + 1]);
					if (num3 < num2)
					{
						num2 = num3;
						num = i;
					}
				}
			}
			Vector3 projectOnLine = GameKit.GetProjectOnLine(line.Path[num], line.Path[num + 1], curPos);
			CreepPathAnchor monsterPathAnchor = new CreepPathAnchor(projectOnLine, num, line, curLineIndex);
			firstAnchor = new CreepPathAnchor(curLineIndex, monsterPathAnchor.pathSegmentId, monsterPathAnchor.lenProgress);
			secondAnchor = new CreepPathAnchor(curLineIndex, monsterPathAnchor.pathSegmentId, monsterPathAnchor.lenProgress);
			LineDirector.Current.MoveMonsterAnchor(firstAnchor, -0.4f);
			LineDirector.Current.MoveMonsterAnchor(secondAnchor, 0.4f);
		}

		public Vector3 GetCurPos()
		{
			return (firstAnchor.pos + secondAnchor.pos) * 0.5f + offset * offsetProportion;
		}

		public int curLineIndex;

		public CreepPathAnchor firstAnchor;

		public CreepPathAnchor secondAnchor;

		public Vector3 offset = Vector3.zero;

		public float offsetProportion;

		public Action OnCompletePath;
	}
}
