using System;

public static class IntToFixedText
{
    private const int MaxValue = 200;

    private static string[] fixedStrings = new string[201];

    private static string[] fixedStringsPercentage = new string[201];

    static IntToFixedText()
	{
		for (int i = 0; i < 200; i++)
		{
			IntToFixedText.fixedStrings[i] = i.ToString();
		}
		for (int j = 0; j < 200; j++)
		{
			IntToFixedText.fixedStringsPercentage[j] = j.ToString() + "%";
		}
	}

	public static string ToFixedString(this int numberUnder100)
	{
		return IntToFixedText.fixedStrings[numberUnder100];
	}

	public static string ToFixedStringPercentage(this int numberUnder100)
	{
		return IntToFixedText.fixedStringsPercentage[numberUnder100];
	}
}
