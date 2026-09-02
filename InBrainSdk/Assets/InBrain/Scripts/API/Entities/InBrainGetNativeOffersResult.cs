using System;
using System.Collections.Generic;
using UnityEngine;

namespace InBrain
{
	[Serializable]
	public class InBrainGetNativeOffersResult
	{
		[SerializeField] public List<InBrainNativeOffer> offers;

		public InBrainGetNativeOffersResult()
		{
			offers = new List<InBrainNativeOffer>();
		}

		public InBrainGetNativeOffersResult(AndroidJavaObject listAJO)
		{
			offers = listAJO.FromJavaList(InBrainNativeOffer.FromAJO);
		}
	}
}
