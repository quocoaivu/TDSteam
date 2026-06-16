using System;
using Common;
using UnityEngine;

namespace Common
{
	public abstract class PositionSource : MonoBehaviour, IPositionSource
	{
		public abstract Vector3 Position { get; }
	}
}
