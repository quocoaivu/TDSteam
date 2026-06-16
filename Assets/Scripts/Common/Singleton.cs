using System;

public abstract class Singleton<T> where T : new()
{
    private static T instance;

    public static T Instance
	{
		get
		{
			if (Singleton<T>.instance == null)
			{
				Singleton<T>.instance = Activator.CreateInstance<T>();
			}
			return Singleton<T>.instance;
		}
	}


}
