using System;
using GameCore;
using UnityEngine;

public abstract class MonoSingleton<T> : BaseMonoBehaviour where T : BaseMonoBehaviour
{
    private static T instance;

    private static bool isQuitting;


    public static T Instance
	{
		get
		{
			if (MonoSingleton<T>.instance == null)
			{
				MonoSingleton<T>.instance = (UnityEngine.Object.FindAnyObjectByType(typeof(T)) as T);
				if (MonoSingleton<T>.instance == null && !MonoSingleton<T>.isQuitting)
				{
					MonoSingleton<T>.instance = new GameObject().AddComponent<T>();
					MonoSingleton<T>.instance.gameObject.name = MonoSingleton<T>.instance.GetType().Name;
				}
			}
			return MonoSingleton<T>.instance;
		}
	}

	// Read-only access không auto-create. Dùng trong OnDestroy/OnDisable để tránh tạo ghost
	// GameObject khi scene đang unload và singleton đã bị destroy.
	public static T InstanceIfExists
	{
		get { return MonoSingleton<T>.instance; }
	}

	protected virtual void OnApplicationQuit()
	{
		MonoSingleton<T>.isQuitting = true;
	}


}
