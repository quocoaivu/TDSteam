using System;

namespace Common
{
	public interface IWritableData<T>
	{
		T Data { get; set; }

		void SaveData();
	}
}
