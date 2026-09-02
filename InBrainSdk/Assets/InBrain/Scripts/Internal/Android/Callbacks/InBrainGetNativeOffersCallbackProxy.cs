using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace InBrain
{
	[SuppressMessage("ReSharper", "InconsistentNaming")]
	public class InBrainGetNativeOffersCallbackProxy : AndroidJavaProxy
	{
		readonly Action<List<InBrainNativeOffer>> _onOffersReceived;

		public InBrainGetNativeOffersCallbackProxy(Action<List<InBrainNativeOffer>> onOffersReceived) : base(Constants.GetNativeOffersCallbackJavaClass)
		{
			_onOffersReceived = onOffersReceived;
		}

		public void onSuccess(AndroidJavaObject offerList)
		{
			InBrainSceneHelper.Queue(() => _onOffersReceived(new InBrainGetNativeOffersResult(offerList).offers));
		}

		public void onFailure(AndroidJavaObject error)
		{
			var message = error.IsJavaNull() ? "Unknown error" : error.Call<string>("getMessage");
			Debug.LogError(string.Format("Failed to receive native offers: {0}", message));
			InBrainSceneHelper.Queue(() => _onOffersReceived(new List<InBrainNativeOffer>()));
		}
	}
}
