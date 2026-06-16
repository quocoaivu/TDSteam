using System;
using UnityEngine;

namespace Common
{
	public interface ISfxFxDirector
	{
		int PlayEffect(AudioClip audioClip, float volumeScale);

		void TryStopEffect(int effectInstanceId);
	}
}
