using System;

namespace Services.PlatformSpecific
{
	public interface IAlert
	{
		void PushNotify(string content, int delayTimeBySecond);

		void CancelAllNotify();
	}
}
