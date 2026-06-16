using System;
using System.Collections.Generic;

public class FormationSetupRecord
{
    public List<float> times = new List<float>();

    public FormationSetupRecord()
	{
	}

	public FormationSetupRecord(FormationSetupRecord copyData)
	{
		for (int i = 0; i < copyData.times.Count; i++)
		{
			times.Add(copyData.times[i]);
		}
	}

	public FormationSetupRecord(List<float> times)
	{
		this.times = times;
	}

	public void AddTime(float timeInSecond)
	{
		times.Add(timeInSecond);
	}

	public float GetDuration()
	{
		return times[times.Count - 1];
	}
}
