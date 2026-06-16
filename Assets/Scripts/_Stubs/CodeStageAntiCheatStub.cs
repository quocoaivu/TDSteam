// Stub for CodeStage Anti-Cheat Toolkit. No encryption — placeholder only.
// Delete this file when re-installing real ACT 2 / switching to another anti-cheat solution.
// See Docs/CODESTAGE_REMOVAL_REINSTALL.md.
using System;
using UnityEngine;

namespace CodeStage.AntiCheat.ObscuredTypes
{
	[Serializable]
	public struct ObscuredInt
	{
		[SerializeField] private int value;

		public ObscuredInt(int v) { value = v; }

		public int Value
		{
			get { return value; }
			set { this.value = value; }
		}

		public static implicit operator int(ObscuredInt o) { return o.value; }
		public static implicit operator ObscuredInt(int v) { return new ObscuredInt(v); }

		public override string ToString() { return value.ToString(); }
	}

	public static class ObscuredPrefs
	{
		public static void SetInt(string key, int value)
		{
			PlayerPrefs.SetInt(key, value);
		}

		public static int GetInt(string key, int defaultValue = 0)
		{
			return PlayerPrefs.GetInt(key, defaultValue);
		}

		public static void SetByteArray(string key, byte[] value)
		{
			PlayerPrefs.SetString(key, value == null ? "" : Convert.ToBase64String(value));
		}

		public static byte[] GetByteArray(string key)
		{
			string s = PlayerPrefs.GetString(key, "");
			return string.IsNullOrEmpty(s) ? new byte[0] : Convert.FromBase64String(s);
		}
	}
}
