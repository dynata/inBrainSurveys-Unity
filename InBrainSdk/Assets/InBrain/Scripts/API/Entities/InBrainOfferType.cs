using UnityEngine;

namespace InBrain
{
	public enum InBrainOfferType
	{
		Default = 0,
		Featured = 1,
		Started = 2
	}

	public static class InBrainOfferTypeExtensions
	{
		public static AndroidJavaObject ToAJO(this InBrainOfferType type)
		{
			if (Application.platform != RuntimePlatform.Android)
				return null;

			using (var offerTypeClass = new AndroidJavaClass(Constants.InBrainOfferTypeJavaClass))
			{
				switch (type)
				{
					case InBrainOfferType.Featured:
						return offerTypeClass.GetStatic<AndroidJavaObject>("FEATURED");
					case InBrainOfferType.Started:
						return offerTypeClass.GetStatic<AndroidJavaObject>("STARTED");
					default:
						return offerTypeClass.GetStatic<AndroidJavaObject>("DEFAULT");
				}
			}
		}
	}
}
