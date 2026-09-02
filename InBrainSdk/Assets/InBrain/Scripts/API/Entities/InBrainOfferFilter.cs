using System;
using UnityEngine;

namespace InBrain
{
	[Serializable]
	public class InBrainOfferFilter
	{
		[SerializeField] public InBrainOfferType type;
		[SerializeField] public int limit;
		[SerializeField] public int offset;

		public InBrainOfferFilter(InBrainOfferType type, int limit = 10, int offset = 0)
		{
			this.type = type;
			this.limit = limit;
			this.offset = offset;
		}

		public AndroidJavaObject ToAJO()
		{
			return Application.platform == RuntimePlatform.Android
				? new AndroidJavaObject(Constants.InBrainOfferFilterJavaClass, type.ToAJO(), limit, offset)
				: null;
		}

		public override string ToString()
		{
			return string.Format("type: {0}, limit: {1}, offset: {2}", type, limit, offset);
		}
	}
}
