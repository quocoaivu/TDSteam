using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Gameplay
{
	public class LineRecord
	{
		public LineRecord(DOTweenPath tweenPath)
		{
			endPoint = tweenPath.wps[tweenPath.wps.Count - 1];
			length = tweenPath.tween.PathLength();
			path = tweenPath.wps;
			path.Insert(0, tweenPath.transform.position);
			int count = path.Count;
			segmentForwards = new Vector3[count - 1];
			segmentLengths = new float[count - 1];
			for (int i = 0; i < count - 1; i++)
			{
				segmentForwards[i] = (path[i + 1] - path[i]).normalized;
				segmentLengths[i] = (path[i + 1] - path[i]).magnitude;
			}
			position = tweenPath.transform.position;
		}

		public List<Vector3> Path
		{
			get
			{
				return path;
			}
		}

		public Vector3 EndPoint
		{
			get
			{
				return endPoint;
			}
		}

		public float Length
		{
			get
			{
				return length;
			}
		}

		public Vector3 Position
		{
			get
			{
				return position;
			}
		}

		private Vector3 position;

		private float length;

		private Vector3 endPoint;

		private List<Vector3> path;

		public Vector3[] segmentForwards;

		public float[] segmentLengths;
	}
}
