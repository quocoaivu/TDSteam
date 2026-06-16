using System;
using UnityEngine;

public class ProjectileCalculator
{
	public static Vector3 GetEndPositionLineShot(Vector3 rootPosition, Vector3 targetPosition, float maxRange)
	{
		Vector3 zero = Vector3.zero;
		Vector3 vector = targetPosition - rootPosition;
		Vector3.Normalize(vector);
		return vector * maxRange;
	}
}
