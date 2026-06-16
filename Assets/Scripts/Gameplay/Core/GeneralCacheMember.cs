using System;
using Common;

namespace Gameplay
{
	public class GeneralCacheMember : MultiPrototypesPoolMemeberMonoBehavior<GeneralCacheMember>
	{
		public void TryReturnToPool()
		{
			if (base.Pool != null)
			{
				base.Pool.PushInstance<GeneralCacheMember>(this);
			}
		}
	}
}
