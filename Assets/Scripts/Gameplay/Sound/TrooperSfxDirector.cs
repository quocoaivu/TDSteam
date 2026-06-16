using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay
{
	public class TrooperSfxDirector : MonoSingleton<TrooperSfxDirector>
	{
		public VolumeReader VolumeReader
		{
			get
			{
				return volumeReader;
			}
			set
			{
				volumeReader = value;
			}
		}

		[SerializeField]
		[FormerlySerializedAs("readDataVolumeAdjust")]
		private VolumeReader volumeReader;
	}
}
