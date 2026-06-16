using System;
using CodeStage.AntiCheat.ObscuredTypes;

public class TailoredObscuredInt
{
	public int Value
	{
		get
		{
			return _value - GameKit.deltaValue;
		}
		set
		{
			_value = value + GameKit.deltaValue;
		}
	}

	private ObscuredInt _value = GameKit.deltaValue;
}
