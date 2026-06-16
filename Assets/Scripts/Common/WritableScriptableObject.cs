using System;
using UnityEngine;

namespace Common
{
	[ExecuteInEditMode]
	public class WritableScriptableObject<T> : ScriptableObject, IWritableData<T>, IWritableScriptableObjectAide, IInspectorActionObject where T : new()
	{
        [SerializeField]
        [ReadOnlyTrait]
        private string key;

        [Space]
        [SerializeField]
        protected T currentData;

        [SerializeField]
        [InspectorCommand]
        private int saveCurrentData;

        [Space]
        [SerializeField]
        protected T defaultData = Activator.CreateInstance<T>();

        [NonSerialized]
        private bool loadedData;

        public T Data
		{
			get
			{
				if (!loadedData)
				{
					LoadData();
					loadedData = true;
				}
				return currentData;
			}
			set
			{
				currentData = value;
			}
		}

		public void SaveData()
		{
			WritableRecordDirectorSource.GetManager().SaveData<T>(key, currentData);
		}

		private void LoadData()
		{
			IWritableRecordDirector manager = WritableRecordDirectorSource.GetManager();
			if (manager.ContainsKey(key))
			{
				currentData = manager.LoadData<T>(key);
			}
			else
			{
				currentData = BinarySerializationAide.Clone<T>(defaultData);
			}
			OnDataLoaded();
		}

		private string GetAutoKey()
		{
			throw new NotImplementedException();
		}

		[ContextMenu("SetAutoKey")]
		protected void SetAutoKey()
		{
		}

		protected void CopyDefaultDataToCurrentData()
		{
			currentData = BinarySerializationAide.Clone<T>(defaultData);
		}

		protected virtual void OnDataLoaded()
		{
		}

		public virtual void OnValidate()
		{
			SetAutoKey();
			key = key.Trim();
		}

		public void Reset()
		{
			SetAutoKey();
		}

		[ContextMenu("LoadCurrentData")]
		public void Editor_LoadCurrentData()
		{
			LoadData();
			SSRTrace.Log("Loaded from " + key);
		}

		[ContextMenu("SaveCurrentData")]
		protected void Editor_SaveCurrentData()
		{
			SaveData();
			SSRTrace.Log("Saved to " + key);
		}

		[ContextMenu("CopyDefaultDataToCurrentData")]
		protected void Editor_CopyDefaultDataToCurrentData()
		{
			currentData = BinarySerializationAide.Clone<T>(defaultData);
		}

		void IInspectorActionObject.ExcuteCommand(int intPara, string stringPara)
		{
			Editor_SaveCurrentData();
		}
	}
}
