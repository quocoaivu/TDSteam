using System;

namespace Services.PlatformSpecific
{
	public interface IAdvert
	{
		void RequestAds();

		bool IsOfferVideoAvailable { get; }

		void ShowInterstitial();

		void ShowOfferVideo(OfferVideoCallback offerVideoCallback);
	}
}
