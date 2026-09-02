using System;
using UnityEngine;

namespace InBrain
{
	[Serializable]
	public class InBrainOfferPromotion
	{
		[SerializeField] public float multiplier;
		[SerializeField] public float originalReward;
		[SerializeField] public string originalRewardString;

		public InBrainOfferPromotion(float multiplier, float originalReward, string originalRewardString)
		{
			this.multiplier = multiplier;
			this.originalReward = originalReward;
			this.originalRewardString = originalRewardString;
		}

		public static InBrainOfferPromotion FromAJO(AndroidJavaObject ajo)
		{
			if (ajo.IsJavaNull())
			{
				return null;
			}

			return new InBrainOfferPromotion(
				(float) ajo.Call<double>("getMultiplier"),
				(float) ajo.Call<double>("getOriginalReward"),
				ajo.Call<string>("getOriginalRewardString"));
		}

		public override string ToString()
		{
			return string.Format("multiplier: {0}, originalReward: {1}, originalRewardString: {2}",
				multiplier, originalReward, originalRewardString);
		}
	}
}
