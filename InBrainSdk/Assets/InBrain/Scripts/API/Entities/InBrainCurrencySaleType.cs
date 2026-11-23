using UnityEngine;

namespace InBrain
{
	public enum InBrainCurrencySaleType
	{
		Surveys = 0,
		Offers = 1
	}

	public static class InBrainCurrencySaleTypeExtensions
	{
		public static AndroidJavaObject ToAJO(this InBrainCurrencySaleType type)
		{
			if (Application.platform != RuntimePlatform.Android)
				return null;

			using (var saleTypeClass = new AndroidJavaClass(Constants.InBrainCurrencySaleTypeJavaClass))
			{
				switch (type)
				{
					case InBrainCurrencySaleType.Offers:
						return saleTypeClass.GetStatic<AndroidJavaObject>("OFFERS");
					default:
						return saleTypeClass.GetStatic<AndroidJavaObject>("SURVEYS");
				}
			}
		}
	}
}
