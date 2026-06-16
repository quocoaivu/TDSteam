using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	public class PointPatternsCluster : MonoBehaviour
	{
		public List<Transform> getPoints(int pointAmount)
		{
			List<Transform> list = new List<Transform>();
			if (pointAmount != 3)
			{
				if (pointAmount != 4)
				{
					if (pointAmount == 5)
					{
						foreach (Transform item in _5pointPattern.points)
						{
							list.Add(item);
						}
					}
				}
				else
				{
					foreach (Transform item2 in _4pointPattern.points)
					{
						list.Add(item2);
					}
				}
			}
			else
			{
				foreach (Transform item3 in _3pointPattern.points)
				{
					list.Add(item3);
				}
			}
			return list;
		}

		public Vector2 getParentPointsPosition(int pointAmount)
		{
			Vector2 result = Vector2.zero;
			if (pointAmount != 3)
			{
				if (pointAmount != 4)
				{
					if (pointAmount == 5)
					{
						result = _5pointPattern.transform.position;
					}
				}
				else
				{
					result = _4pointPattern.transform.position;
				}
			}
			else
			{
				result = _3pointPattern.transform.position;
			}
			return result;
		}

		public PointLayout _3pointPattern;

		public PointLayout _4pointPattern;

		public PointLayout _5pointPattern;
	}
}
