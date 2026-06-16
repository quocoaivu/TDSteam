using Bootstrap;
using Data;
using Parameter;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace LifetimePopup
{
	public class AskToRateDialogHandler : GameplayPriorityDialogHandler
	{
		public override void InitPriority(DialogPriorityEnum priority)
		{
			base.InitPriority(priority);
			starSlider.value = 0f;
			foreach (GameObject gameObject in starGroup)
			{
				gameObject.SetActive(false);
			}
		}

		public void UpdateStarSliderValue()
		{
			int num = (int)starSlider.value;
			if (num >= 1)
			{
				for (int i = 0; i < starGroup.Length; i++)
				{
					if (i <= num - 1)
					{
						starGroup[i].SetActive(true);
					}
					else
					{
						starGroup[i].SetActive(false);
					}
				}
			}
			else
			{
				foreach (GameObject gameObject in starGroup)
				{
					gameObject.SetActive(false);
				}
			}
		}

		public void Rate()
		{
			PlayerSaveStore.Instance.SetDataRated();
			CloseWithScaleAnimation();
			if (starSlider.value >= 4f)
			{
				Application.OpenURL(MarketingSetup.rateGameLink);
			}
			else
			{
				string ratingBehavior = Bootstrap.GameBootstrap.Instance.RemoteConfig.GetRatingBehavior();
				if (ratingBehavior.Equals(RatingConduct.thanknhide.ToString()))
				{
					string notiContent = Singleton<AlertSynopsis>.Instance.GetNotiContent(114);
					MonoSingleton<LifespanSurface>.Instance.NotifyPopupController.Init(notiContent, false, false);
				}
				else if (ratingBehavior.Equals(RatingConduct.tostore.ToString()))
				{
					Application.OpenURL(MarketingSetup.rateGameLink);
				}
			}
		}

		public void SendFeedback()
		{
			string companyEmail = MarketingSetup.companyEmail;
			string text = EscapeUrl(MarketingSetup.feedbackTitle);
			string text2 = EscapeUrl("Please Enter your message here");
			CloseWithScaleAnimation();
			Application.OpenURL(string.Concat(new string[]
			{
				"mailto:",
				companyEmail,
				"?subject=",
				text,
				"&body=",
				text2
			}));
			PlayerSaveStore.Instance.SetDataRated();
		}

		private string EscapeUrl(string url)
		{
			return UnityWebRequest.EscapeURL(url).Replace("+", "%20");
		}

		public override void Open()
		{
			base.Open();
		}

		public override void Close()
		{
			base.Close();
		}

		[SerializeField]
		private GameObject[] starGroup;

		[SerializeField]
		private Slider starSlider;
	}
}
