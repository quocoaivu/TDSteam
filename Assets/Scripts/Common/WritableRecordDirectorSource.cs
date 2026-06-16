using System;
using System.Collections.Generic;

namespace Common
{
	public static class WritableRecordDirectorSource
	{
        private static IWritableRecordDirector manager;

        public static IWritableRecordDirector GetManager()
		{
			if (WritableRecordDirectorSource.manager == null)
			{
				WritableRecordDirectorSource.manager = new WritableRecordDirectorSource.WritableDataManager();
			}
			return WritableRecordDirectorSource.manager;
		}

		private class WritableDataManager : IWritableRecordDirector
		{
            private const string KeysListKey = "DatakeyList";

            private List<string> keys;

            public WritableDataManager()
			{
				LoadKeysList();
			}

			public bool ContainsKey(string key)
			{
				return keys.Contains(key);
			}

			public T LoadData<T>(string key)
			{
				byte[] bytes = PersistentFileAide.LoadFile(GetFileName(key));
				return BinarySerializationAide.Deserialize<T>(bytes);
			}

			public void SaveData<T>(string key, T data)
			{
				byte[] bytes = BinarySerializationAide.Serialize<T>(data);
				PersistentFileAide.SaveFile(bytes, GetFileName(key));
				if (!keys.Contains(key))
				{
					keys.Add(key);
					SavekeysList();
				}
			}

			public void DeleteData(string key)
			{
				keys.Remove(key);
				SavekeysList();
			}

			public void DeleteAllData()
			{
				keys = new List<string>();
				SavekeysList();
			}

			private void LoadKeysList()
			{
				string fileName = GetFileName("DatakeyList");
				if (PersistentFileAide.FileExist(fileName))
				{
					byte[] bytes = PersistentFileAide.LoadFile(fileName);
					keys = BinarySerializationAide.Deserialize<List<string>>(bytes);
				}
				else
				{
					keys = new List<string>();
				}
			}

			private void SavekeysList()
			{
				byte[] bytes = BinarySerializationAide.Serialize<List<string>>(keys);
				PersistentFileAide.SaveFile(bytes, GetFileName("DatakeyList"));
			}

			private string GetFileName(string key)
			{
				return string.Format("WritableData_{0}.SSR", key);
			}
		}
	}
}
