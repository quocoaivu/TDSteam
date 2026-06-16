using System;
using System.Collections.Generic;
using DG.Tweening;
using MetaGame;
using UnityEngine;

namespace Gameplay
{
	public class LineDirector : MonoBehaviour
	{
		public static LineDirector Current { get; set; }

		private void Awake()
		{
			LineDirector.Current = this;
			AddMemberGates();
			AddMemberShadowRoads();
		}

		private void AddMemberGates()
		{
			ZoneGateHandler[] componentsInChildren = base.GetComponentsInChildren<ZoneGateHandler>();
			listGates = new List<ZoneGateHandler>(componentsInChildren);
			for (int i = 0; i < listGates.Count; i++)
			{
				listGates[i].Init();
				for (int j = 0; j < listGates[i].gates.Count; j++)
				{
					DOTweenPath component = listGates[i].gates[j].GetComponent<DOTweenPath>();
					LineRecord value = new LineRecord(component);
					int lineIndex = GetLineIndex(i, j);
					linesData.Add(lineIndex, value);
				}
			}
		}

		private void AddMemberShadowRoads()
		{
			ShadeTrail[] componentsInChildren = base.GetComponentsInChildren<ShadeTrail>();
			shadowRoads = new List<ShadeTrail>(componentsInChildren);
		}

		public LineRecord GetLine(int gate, int line)
		{
			gateCount = listGates.Count;
			if (gate >= gateCount || line >= Setup.Instance.LineCount)
			{
				throw new IndexOutOfRangeException();
			}
			int lineIndex = GetLineIndex(gate, line);
			LineRecord lineData;
			if (!linesData.TryGetValue(lineIndex, out lineData))
			{
				DOTweenPath component = listGates[gate].gates[line].GetComponent<DOTweenPath>();
				lineData = new LineRecord(component);
				linesData.Add(lineIndex, lineData);
			}
			return lineData;
		}

		public LineRecord GetLine(int lineIndex)
		{
			int gate = lineIndex / 100;
			int line = lineIndex % 100;
			return GetLine(gate, line);
		}

		public int GetLineIndex(int gate, int line)
		{
			return gate * 100 + line;
		}

		public void FindNearestLine(Vector3 targetPos, out int lineIndex, out Vector3 offset, out CreepPathAnchor anchorOnSegment)
		{
			int num = -1;
			int num2 = -1;
			float num3 = float.PositiveInfinity;
			foreach (KeyValuePair<int, LineRecord> keyValuePair in linesData)
			{
				LineRecord value = keyValuePair.Value;
				for (int i = value.Path.Count - 2; i >= 0; i--)
				{
					if (GameKit.HaveProjectOnSegment(value.Path[i], value.Path[i + 1], targetPos))
					{
						float num4 = GameKit.SquareDistancePointSegment(targetPos, value.Path[i], value.Path[i + 1]);
						if (num4 < num3)
						{
							num3 = num4;
							num = keyValuePair.Key;
							num2 = i;
						}
					}
				}
			}
			lineIndex = num;
			LineRecord line = GetLine(lineIndex);
			Vector3 projectOnLine = GameKit.GetProjectOnLine(line.Path[num2], line.Path[num2 + 1], targetPos);
			offset = targetPos - projectOnLine;
			anchorOnSegment = new CreepPathAnchor(projectOnLine, num2, line, lineIndex);
		}

		public void RequestMove(EnemyData enemy, CreepPathRecord monsterPathData, float moveDistance, bool shorteningOffset = false, float dt = 0f)
		{
			MoveMonsterAnchor(monsterPathData.firstAnchor, moveDistance);
			MoveMonsterAnchor(monsterPathData.secondAnchor, moveDistance);
			if (shorteningOffset)
			{
				monsterPathData.offsetProportion = Mathf.MoveTowards(monsterPathData.offsetProportion, 0f, dt);
			}
			enemy.transform.position = monsterPathData.GetCurPos();
			LineRecord lineData = linesData[monsterPathData.firstAnchor.curLineIndex];
			int num = lineData.segmentForwards.Length;
			if (monsterPathData.firstAnchor.pathSegmentId == num - 1 && monsterPathData.OnCompletePath != null && monsterPathData.firstAnchor.lenProgress >= lineData.segmentLengths[num - 1])
			{
				monsterPathData.OnCompletePath();
				monsterPathData.OnCompletePath = null;
			}
		}

		public void MoveMonsterAnchor(CreepPathAnchor anchor, float moveDistance)
		{
			LineRecord lineData = linesData[anchor.curLineIndex];
			while (anchor.lenProgress + moveDistance < 0f && anchor.pathSegmentId > 0)
			{
				moveDistance += anchor.lenProgress;
				anchor.pathSegmentId--;
				anchor.lenProgress = lineData.segmentLengths[anchor.pathSegmentId];
			}
			while (anchor.lenProgress + moveDistance > lineData.segmentLengths[anchor.pathSegmentId] && anchor.pathSegmentId < lineData.segmentForwards.Length - 1)
			{
				moveDistance -= lineData.segmentLengths[anchor.pathSegmentId] - anchor.lenProgress;
				anchor.pathSegmentId++;
				anchor.lenProgress = 0f;
			}
			anchor.lenProgress += moveDistance;
		}

		private int gateCount;

		[NonSerialized]
		public List<ZoneGateHandler> listGates = new List<ZoneGateHandler>();

		[NonSerialized]
		public List<ShadeTrail> shadowRoads = new List<ShadeTrail>();

		public Dictionary<int, LineRecord> linesData = new Dictionary<int, LineRecord>();
	}
}
