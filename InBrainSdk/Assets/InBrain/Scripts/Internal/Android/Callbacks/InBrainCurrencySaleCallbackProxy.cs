using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace InBrain
{
	[SuppressMessage("ReSharper", "InconsistentNaming")]
	public class InBrainCurrencySaleCallbackProxy : AndroidJavaProxy
	{
		readonly Action<InBrainCurrencySale> _onCurrencySaleReceived;
		
		public InBrainCurrencySaleCallbackProxy(Action<InBrainCurrencySale> onCurrencySaleReceived) : base(Constants.GetCurrencySaleCallbackJavaClass)
		{
			_onCurrencySaleReceived = onCurrencySaleReceived;
		}

		public void currencySaleReceived(AndroidJavaObject currencySale)
		{
			InBrainSceneHelper.Queue(() => _onCurrencySaleReceived(!currencySale.IsJavaNull() ? InBrainCurrencySale.FromAJO(currencySale) : null));
		}
	}
}