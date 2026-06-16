using System;
using UnityEngine;

public class ArenaSeasonDetail
{
	public ArenaSeasonDetail(int seasonNumber, DateTime startData, DateTime endData)
	{
		this.seasonNumber = seasonNumber;
		seasonStartDate = startData;
		seasonEndDate = endData;
	}

	public ArenaSeasonDetail(int seasonNumber, DateTime startData, DateTime endData, string minVersion, bool chooseGroupBaseonTier) : this(seasonNumber, startData, endData)
	{
		this.minVersion = minVersion;
		isChoosingGroupBaseOnTier = chooseGroupBaseonTier;
	}

	public bool IsCurVersionUptodate()
	{
		string version = Application.version;
		string[] array = minVersion.Split(new char[]
		{
			'.'
		});
		string[] array2 = version.Split(new char[]
		{
			'.'
		});
		int num = Mathf.Min(array.Length, array2.Length);
		for (int i = 0; i < num; i++)
		{
			int num2;
			int.TryParse(array[i], out num2);
			int num3;
			int.TryParse(array2[i], out num3);
			if (num2 > num3)
			{
				return false;
			}
			if (num3 > num2)
			{
				break;
			}
		}
		return true;
	}

	public int seasonNumber;

	public DateTime seasonStartDate;

	public DateTime seasonEndDate;

	public string minVersion = "0.0.0";

	public bool isChoosingGroupBaseOnTier;
}
