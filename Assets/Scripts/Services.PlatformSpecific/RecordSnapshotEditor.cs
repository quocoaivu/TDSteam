using System;

namespace Services.PlatformSpecific
{
	public class RecordSnapshotEditor : IRecordSnapshot
	{
		public string GetRawJsonValue()
		{
			return string.Empty;
		}

		public bool IsCompleted()
		{
			return true;
		}

		public bool IsFaulted()
		{
			return false;
		}
	}
}
