using System;
//using Firebase.Database;

namespace Services.PlatformSpecific
{
	public class RecordSnapshotAndroid : IRecordSnapshot
	{
		//public RecordSnapshotAndroid(DataSnapshot snapshot, bool isTaskFaulted, bool isTaskCompleted)
		//{
		//	this.snapshot = snapshot;
		//	this.isTaskFaulted = isTaskFaulted;
		//	this.isTaskCompleted = isTaskCompleted;
		//}

		public string GetRawJsonValue()
		{
            //if (snapshot == null)
            //{
            //	return string.Empty;
            //}
            //return snapshot.GetRawJsonValue();
            return string.Empty;
		}

		public bool IsCompleted()
		{
			return isTaskCompleted;
		}

		public bool IsFaulted()
		{
			return isTaskFaulted;
		}

		//private DataSnapshot snapshot;

		private bool isTaskFaulted;

		private bool isTaskCompleted;
	}
}
