using System;
using UnityEngine;

namespace Gameplay
{
	public class CreepPathAnchor
	{
		public CreepPathAnchor()
		{
		}

		public CreepPathAnchor(Vector3 posOnSegment, int pathSegmentId, LineRecord lineData, int curLineIndex)
		{
			this.curLineIndex = curLineIndex;
			this.lineData = lineData;
			this.pathSegmentId = pathSegmentId;
			lenProgress = (posOnSegment - lineData.Path[pathSegmentId]).magnitude;
		}

		public CreepPathAnchor(int curLineIndex, int pathSegmentId, float lenProgress)
		{
			this.curLineIndex = curLineIndex;
			this.pathSegmentId = pathSegmentId;
			this.lenProgress = lenProgress;
			lineData = LineDirector.Current.GetLine(curLineIndex);
		}

		public CreepPathAnchor(CreepPathAnchor pAnchor)
		{
			curLineIndex = pAnchor.curLineIndex;
			pathSegmentId = pAnchor.pathSegmentId;
			lenProgress = pAnchor.lenProgress;
			lineData = LineDirector.Current.GetLine(curLineIndex);
		}

		public Vector3 pos
		{
			get
			{
				return lineData.Path[pathSegmentId] + lineData.segmentForwards[pathSegmentId] * lenProgress;
			}
		}

		public int curLineIndex;

		public int pathSegmentId;

		public float lenProgress;

		private LineRecord lineData;
	}
}
