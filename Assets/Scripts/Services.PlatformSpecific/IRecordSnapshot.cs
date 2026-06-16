using System;

namespace Services.PlatformSpecific
{
	public interface IRecordSnapshot
	{
		string GetRawJsonValue();

		bool IsCompleted();

		bool IsFaulted();
	}
}
