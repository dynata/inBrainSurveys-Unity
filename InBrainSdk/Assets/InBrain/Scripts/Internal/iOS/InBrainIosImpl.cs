using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Linq;
using UnityEngine;

namespace InBrain
{
	public class InBrainIosImpl : IInBrainImpl
	{
		public void Init(string clientId, string clientSecret, bool isS2S)
		{
#if UNITY_IOS && !UNITY_EDITOR
			_ib_SetInBrain(clientId, clientSecret, isS2S);
#endif
		}

		public void Init(string clientId, string clientSecret, bool isS2S, string userId)
		{
#if UNITY_IOS && !UNITY_EDITOR
			_ib_SetInBrainWithUserId(clientId, clientSecret, isS2S, userId);
#endif
		}

		public void SetUserId(string userId)
		{
#if UNITY_IOS && !UNITY_EDITOR
			_ib_SetInBrainUserId(userId);
#endif
		}

		public void SetSessionId(string sessionId)
		{
#if UNITY_IOS && !UNITY_EDITOR
			_ib_SetSessionId(sessionId);
#endif
		}

		public void SetDemographicData(InBrainDemographicData demographicData)
		{
			string demographicDataJson = null;
			if (demographicData != null)
			{
				demographicDataJson = JsonUtility.ToJson(demographicData);
			}

#if UNITY_IOS && !UNITY_EDITOR
			_ib_SetDataOptions(demographicDataJson);
#endif
		}

		public void AddCallback(Action<List<InBrainReward>> onRewardsReceived, Action<InBrainRewardsViewDismissedResult> onRewardsViewDismissed,
			bool confirmRewardsAutomatically = false)
		{
			Action<string> onRewardsReceivedNative = rewardsJson =>
			{
				var rewardsResult = JsonUtility.FromJson<InBrainGetRewardsResult>(rewardsJson);
				onRewardsReceived?.Invoke(rewardsResult.rewards);

				if (confirmRewardsAutomatically && rewardsResult.rewards.Any())
				{
					ConfirmRewards(rewardsResult.rewards);
				}
			};
			
			Action<string> onRewardsViewDismissedNative = dismissedResultJson =>
			{
				var dismissedResult = JsonUtility.FromJson<InBrainRewardsViewDismissedResult>(dismissedResultJson);
				onRewardsViewDismissed?.Invoke(dismissedResult);
			};

#if UNITY_IOS && !UNITY_EDITOR
			_ib_SetCallback(Callbacks.ActionStringCallback, onRewardsReceivedNative.GetPointer(),
				Callbacks.ActionStringCallback, onRewardsViewDismissedNative.GetPointer());
#endif
		}

		public void RemoveCallback()
		{
#if UNITY_IOS && !UNITY_EDITOR
			_ib_RemoveCallback();
#endif
		}

		public void CheckSurveysAvailability(Action<bool> onAvailabilityChecked)
		{
#if UNITY_IOS && !UNITY_EDITOR
			_ib_CheckSurveysAvailability(Callbacks.ActionBoolCallback, onAvailabilityChecked.GetPointer());
#endif
		}

		public void ShowSurveys(InBrainWallOption wallOption)
		{
#if UNITY_IOS && !UNITY_EDITOR
			_ib_ShowSurveys();
#endif
		}

		public void ShowSurvey(string surveyId, string searchId, bool offersEnabled)
		{
#if UNITY_IOS && !UNITY_EDITOR
			_ib_ShowSurvey(surveyId, searchId);
#endif
		}

		public void GetRewards()
		{
#if UNITY_IOS && !UNITY_EDITOR
			_ib_GetRewards();
#endif
		}

		public void GetRewards(Action<List<InBrainReward>> onRewardsReceived, Action onFailedToReceiveRewards, bool confirmRewardsAutomatically = false)
		{
			Action<string> onRewardsReceivedNative = rewardsJson =>
			{
				var rewardsResult = JsonUtility.FromJson<InBrainGetRewardsResult>(rewardsJson);
				onRewardsReceived?.Invoke(rewardsResult.rewards);

				if (confirmRewardsAutomatically && rewardsResult.rewards.Any())
				{
					ConfirmRewards(rewardsResult.rewards);
				}
			};

#if UNITY_IOS && !UNITY_EDITOR
			_ib_GetRewardsWithCallback(Callbacks.ActionStringCallback, onRewardsReceivedNative.GetPointer(),
				Callbacks.ActionVoidCallback, onFailedToReceiveRewards.GetPointer());
#endif
		}

		public void ConfirmRewards(List<InBrainReward> rewards)
		{
			var rewardsIds = rewards.Select(reward => reward.transactionId).ToList();
			var rewardsJson = JsonUtility.ToJson(new InBrainRewardIds(rewardsIds));

#if UNITY_IOS && !UNITY_EDITOR
			_ib_ConfirmRewards(rewardsJson);
#endif
		}

		public void SetToolbarConfig(InBrainToolbarConfig config)
		{
#if UNITY_IOS && !UNITY_EDITOR
			var title = config.Title;
			var backgroundColor = config.ToolbarColor.ToARGBColor();
			var titleColor = config.TitleColor.ToARGBColor();
			var backButtonColor = config.BackButtonColor.ToARGBColor();

			_ib_SetNavigationBarConfig(title, backgroundColor, titleColor, backButtonColor);
#endif
		}

		public void SetStatusBarConfig(InBrainStatusBarConfig config)
		{
#if UNITY_IOS && !UNITY_EDITOR
			var white = config.LightStatusBarIcons;
			var hide = config.HideStatusBarIos;

			_ib_SetStatusBarConfig(white, hide);
#endif
		}

		public void GetSurveysWithFilter(InBrainSurveyFilter filter, Action<List<InBrainSurvey>> onSurveysReceived)
		{
			var filterJson = JsonUtility.ToJson(filter);

			Action<string> onSurveysReceivedNative = surveysJson =>
			{
				var surveysResult = JsonUtility.FromJson<InBrainGetSurveysResult>(surveysJson);
				onSurveysReceived?.Invoke(surveysResult.surveys);
			};

			Action onFailedToReceiveSurveys = () =>
			{
				Debug.Log("Failed to receive surveys list");
			};

#if UNITY_IOS && !UNITY_EDITOR
			_ib_GetNativeSurveysWithFilterAndCallback(filterJson, Callbacks.ActionStringCallback, onSurveysReceivedNative.GetPointer(),
				Callbacks.ActionVoidCallback, onFailedToReceiveSurveys.GetPointer());
#endif
		}

		public void GetCurrencySale(Action<InBrainCurrencySale> onCurrencySaleReceived)
		{
			Action<string> onCurrencySaleReceivedNative = currencySaleJson =>
			{
				var currencySale = JsonUtility.FromJson<InBrainCurrencySale>(currencySaleJson);
				onCurrencySaleReceived?.Invoke(currencySale);
			};

			Action onFailedToReceiveCurrencySale = () =>
			{
				Debug.Log("Failed to receive currency sale");
			};
			
#if UNITY_IOS && !UNITY_EDITOR
			_ib_GetCurrencySale(Callbacks.ActionStringCallback, onCurrencySaleReceivedNative.GetPointer(),
				Callbacks.ActionVoidCallback, onFailedToReceiveCurrencySale.GetPointer());
#endif
		}

#if UNITY_IOS && !UNITY_EDITOR
		[DllImport("__Internal")]
		static extern void _ib_SetInBrain(string clientId, string secret, bool isS2S);

		[DllImport("__Internal")]
		static extern void _ib_SetInBrainWithUserId(string clientId, string secret, bool isS2S, string userId);

		[DllImport("__Internal")]
		static extern void _ib_SetInBrainUserId(string userId);

		[DllImport("__Internal")]
		static extern void _ib_SetSessionId(string sessionId);

		[DllImport("__Internal")]
		static extern void _ib_SetDataOptions(string demographicData);

		[DllImport("__Internal")]
		static extern void _ib_CheckSurveysAvailability(Callbacks.ActionBoolCallbackDelegate surveysAvailabilityCheckedCallback, IntPtr surveysAvailabilityCheckedActionPtr);

		[DllImport("__Internal")]
		static extern void _ib_ShowSurveys();

		[DllImport("__Internal")]
		static extern void _ib_ShowSurvey(string id, string searchId);

		[DllImport("__Internal")]
		static extern void _ib_SetCallback(Callbacks.ActionStringCallbackDelegate rewardReceivedCallback, IntPtr rewardReceivedActionPtr,
			Callbacks.ActionStringCallbackDelegate rewardViewDismissedCallback, IntPtr rewardViewDismissedActionPtr);

		[DllImport("__Internal")]
		static extern void _ib_RemoveCallback();

		[DllImport("__Internal")]
		static extern void _ib_GetRewards();

		[DllImport("__Internal")]
		static extern void _ib_GetRewardsWithCallback(Callbacks.ActionStringCallbackDelegate rewardReceivedCallback, IntPtr rewardReceivedActionPtr,
			Callbacks.ActionVoidCallbackDelegate failedToReceiveRewardsCallback, IntPtr failedToReceiveRewardsActionPtr);

		[DllImport("__Internal")]
		static extern void _ib_ConfirmRewards(string rewardsJson);

		[DllImport("__Internal")]
		static extern void _ib_SetNavigationBarConfig(string title, int backgroundColor, int titleColor, int backButtonColor);

		[DllImport("__Internal")]
		static extern void _ib_SetStatusBarConfig(bool light, bool hide);

		[DllImport("__Internal")]
		static extern void _ib_GetNativeSurveysWithFilterAndCallback(string filterJson, Callbacks.ActionStringCallbackDelegate surveysReceivedCallback, IntPtr surveysReceivedActionPtr,
			Callbacks.ActionVoidCallbackDelegate failedToReceiveSurveysCallback, IntPtr failedToReceiveSurveysActionPtr);

		[DllImport("__Internal")]
		static extern void _ib_GetCurrencySale(Callbacks.ActionStringCallbackDelegate currencySaleReceivedCallback, IntPtr currencySaleReceivedActionPtr,
			Callbacks.ActionVoidCallbackDelegate failedToReceiveCurrencySaleCallback, IntPtr failedToReceiveCurrencySaleActionPtr);
#endif
	}
}