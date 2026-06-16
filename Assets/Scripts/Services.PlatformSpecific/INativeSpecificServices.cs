using System;

namespace Services.PlatformSpecific
{
	public interface INativeSpecificServices
	{
		string StoreLink { get; }

		IMetrics Analytics { get; }

		IInappBilling InApPurchase { get; }

		ISocialServices FacebookServices { get; }

		IAdvert Ad { get; }

		IPlayerDossier UserProfile { get; }

		IRecordCloudSaver DataCloudSaver { get; }

		IAlert GameNotification { get; }
	}
}
