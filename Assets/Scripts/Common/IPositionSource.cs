using System;
using UnityEngine;

namespace Common
{
	public interface IPositionSource
	{
		Vector3 Position { get; }
	}
}
