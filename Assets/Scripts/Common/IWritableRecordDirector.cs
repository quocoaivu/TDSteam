using System;

namespace Common
{
	public interface IWritableRecordDirector
	{
		bool ContainsKey(string key);

		T LoadData<T>(string key);

		void SaveData<T>(string key, T data);

		void DeleteData(string key);

		void DeleteAllData();
	}
}
