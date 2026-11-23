using System;
using UnityEngine;

namespace InBrain
{
	[Serializable]
	public class InBrainOfferBannerAd
	{
		[SerializeField] public string header;
		[SerializeField] public string buttonText;
		[SerializeField] public string bannerDescription;

		public InBrainOfferBannerAd(string header, string buttonText, string bannerDescription)
		{
			this.header = header;
			this.buttonText = buttonText;
			this.bannerDescription = bannerDescription;
		}

		public static InBrainOfferBannerAd FromAJO(AndroidJavaObject ajo)
		{
			if (ajo.IsJavaNull())
			{
				return null;
			}

			return new InBrainOfferBannerAd(
				ajo.Call<string>("getHeader"),
				ajo.Call<string>("getButtonText"),
				ajo.Call<string>("getDescription"));
		}

		public override string ToString()
		{
			return string.Format("header: {0}, buttonText: {1}, bannerDescription: {2}",
				header, buttonText, bannerDescription);
		}
	}
}
