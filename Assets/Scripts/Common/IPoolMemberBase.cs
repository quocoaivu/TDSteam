using System;

namespace Common
{
	public interface IPoolMemberBase<T> : ICloneable<T>
	{
		bool InPool { get; set; }
	}
}
