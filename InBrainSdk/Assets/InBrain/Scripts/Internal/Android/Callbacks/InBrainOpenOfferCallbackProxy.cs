using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace InBrain
{
	[SuppressMessage("ReSharper", "InconsistentNaming")]
	public class InBrainOpenOfferCallbackProxy : AndroidJavaProxy
	{
		public InBrainOpenOfferCallbackProxy() : base(Constants.OpenOfferCallbackJavaClass)
		{
		}

		public void onSuccess()
		{
			Debug.Log("InBrain offer opened successfully");
		}

		public void onFailure(AndroidJavaObject error)
		{
			var message = error.IsJavaNull() ? "Unknown error" : error.Call<string>("getMessage");
			Debug.LogError(string.Format("Failed to open inBrain offer: {0}", message));
		}
	}
}
