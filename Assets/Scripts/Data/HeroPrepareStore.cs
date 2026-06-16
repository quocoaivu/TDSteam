using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Data
{
	public class HeroPrepareStore : MonoBehaviour
	{
		public static HeroPrepareStore Instance
		{
			get
			{
				return HeroPrepareStore.instance;
			}
		}

		private void Awake()
		{
			HeroPrepareStore.instance = this;
			SaveDefaultData();
			Load();
		}

		private void SaveDefaultData()
		{
			if (!File.Exists(Application.persistentDataPath + HeroPrepareStore.DB_NAME))
			{
				FileStream fileStream = File.Create(Application.persistentDataPath + HeroPrepareStore.DB_NAME);
				BinaryFormatter binaryFormatter = new BinaryFormatter { Binder = new Common.SaveTypeCompatBinder() };
				data.listHeroIDSaved = new int[]
				{
					-1,
					-1,
					-1
				};
				binaryFormatter.Serialize(fileStream, data);
				fileStream.Close();
			}
		}

		public void Save(int[] listHeroID)
		{
			FileStream fileStream = File.Create(Application.persistentDataPath + HeroPrepareStore.DB_NAME);
			BinaryFormatter binaryFormatter = new BinaryFormatter { Binder = new Common.SaveTypeCompatBinder() };
			for (int i = 0; i < data.listHeroIDSaved.Length; i++)
			{
				data.listHeroIDSaved[i] = listHeroID[i];
			}
			binaryFormatter.Serialize(fileStream, data);
			fileStream.Close();
		}

		private void Load()
		{
			string path = Application.persistentDataPath + HeroPrepareStore.DB_NAME;
			try
			{
				using (FileStream fileStream = File.Open(path, FileMode.Open))
				{
					BinaryFormatter binaryFormatter = new BinaryFormatter { Binder = new Common.SaveTypeCompatBinder() };
					data = (HeroPrepareSerializeRecord)binaryFormatter.Deserialize(fileStream);
				}
			}
			catch (Exception e)
			{
				UnityEngine.Debug.LogError("HeroPrepareStore: failed to load save, regenerating default. " + e.Message);
				if (File.Exists(path))
				{
					File.Delete(path);
				}
				SaveDefaultData();
			}
		}

		public int[] GetListHeroIDSaved()
		{
			Load();
			return data.listHeroIDSaved;
		}

		private static string DB_NAME = "/heroPrepareInfor.dat";

		private HeroPrepareSerializeRecord data = new HeroPrepareSerializeRecord();

		private static HeroPrepareStore instance;
		[UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			instance = null;
		}
	}
}
