using System;
using System.Collections.Generic;
using UnityEngine;

namespace InBrain
{
	public class InBrainAndroidImpl : IInBrainImpl
	{
		readonly AndroidJavaObject _inBrainInst;

		InBrainCallbackProxy _callback;

		AndroidJavaObject InBrainInst
		{
			get
			{
				if (_inBrainInst == null)
				{
					Debug.LogWarning("InBrain Android implementation is not instantiated");
				}

				return _inBrainInst;
			}
		}

		public InBrainAndroidImpl()
		{
			using (var inBrainClass = new AndroidJavaClass(Constants.InBrainJavaClass))
			{
				_inBrainInst = inBrainClass.CallStatic<AndroidJavaObject>(Constants.GetInstanceJavaMethod);
			}
		}

		public void Init(string clientId, string clientSecret, bool isS2S)
		{
			InBrainInst?.Call(Constants.SetInBrainJavaMethod,
				JniUtils.Activity, clientId, clientSecret, isS2S);
		}

		public void Init(string clientId, string clientSecret, bool isS2S, string userId)
		{
			InBrainInst?.Call(Constants.SetInBrainJavaMethod,
				JniUtils.Activity, clientId, clientSecret, isS2S, userId);
		}

		public void SetUserId(string userId)
		{
			InBrainInst?.Call(Constants.SetInBrainUserIdJavaMethod, JniUtils.Activity, userId);
		}

		public void SetSessionId(string sessionId)
		{
			InBrainInst?.Call(Constants.SetSessionIdJavaMethod, sessionId);
		}

		public void SetDemographicData(InBrainDemographicData demographicData)
		{
			var demographicDataHashMap = new AndroidJavaObject("java.util.HashMap");
			demographicDataHashMap.Call<string>("put", "gender", demographicData?.gender);
			demographicDataHashMap.Call<string>("put", "age", demographicData?.age.ToString());

			InBrainInst?.Call(Constants.SetDataOptionsJavaMethod, demographicDataHashMap);
		}

		public void AddCallback(Action<List<InBrainReward>> onRewardsReceived, Action<InBrainRewardsViewDismissedResult> onRewardsViewDismissed,
			bool confirmRewardsAutomatically = false)
		{
			_callback = new InBrainCallbackProxy(onRewardsViewDismissed, onRewardsReceived, confirmRewardsAutomatically);

			InBrainInst?.Call(Constants.AddCallbackJavaMethod, _callback);
		}

		public void RemoveCallback()
		{
			if (_callback == null)
			{
				Debug.LogWarning("InBrain Android callback wasn't set");
				return;
			}

			InBrainInst?.Call(Constants.RemoveCallbackJavaMethod, _callback);

			_callback = null;
		}

		public void CheckSurveysAvailability(Action<bool> onAvailabilityChecked)
		{
			InBrainInst?.Call(Constants.CheckSurveysAvailabilityJavaMethod, JniUtils.Activity, new InBrainCheckSurveysAvailabilityCallbackProxy(onAvailabilityChecked));
		}

		public void OpenWall(InBrainWallOption wallOption)
		{
			InBrainInst?.Call(Constants.OpenSurveyWallJavaMethod, JniUtils.Activity, wallOption.ToAJO(), new InBrainStartSurveysCallbackProxy());
		}

		public void OpenSurvey(string surveyId, string searchId, bool offersEnabled)
		{
			InBrainInst?.Call(Constants.ShowSurveyJavaMethod, JniUtils.Activity, surveyId, searchId, offersEnabled, new InBrainStartSurveysCallbackProxy());
		}

		public void GetRewards()
		{
			JniUtils.RunOnUiThread(() => { InBrainInst?.Call(Constants.GetRewardsJavaMethod); });
		}

		public void GetRewards(Action<List<InBrainReward>> onRewardsReceived, Action onFailedToReceiveRewards, bool confirmRewardsAutomatically = false)
		{
			InBrainInst?.Call(Constants.GetRewardsJavaMethod,
				new InBrainGetRewardsCallbackProxy(onRewardsReceived, onFailedToReceiveRewards, confirmRewardsAutomatically));
		}

		public void ConfirmRewards(List<InBrainReward> rewards)
		{
			InBrainInst?.Call(Constants.ConfirmRewardsJavaMethod, rewards.ToJavaList(reward => reward.ToAJO()));
		}

		public void SetToolbarConfig(InBrainToolbarConfig config)
		{
			var javaConfig = new AndroidJavaObject(Constants.ToolbarConfigJavaClass)
				.CallAJO("setElevationEnabled", config.ElevationEnabled)
				.CallAJO("setToolbarColor", config.ToolbarColor.ToJavaColor())
				.CallAJO("setBackButtonColor", config.BackButtonColor.ToJavaColor())
				.CallAJO("setTitleColor", config.TitleColor.ToJavaColor())
				.CallAJO("setTitle", config.Title);

			InBrainInst?.Call(Constants.SetToolbarConfigJavaMethod, javaConfig);
		}

		public void SetStatusBarConfig(InBrainStatusBarConfig config)
		{
			var javaConfig = new AndroidJavaObject(Constants.StatusBarConfigJavaClass)
				.CallAJO("setStatusBarIconsLight", config.LightStatusBarIcons)
				.CallAJO("setStatusBarColor", config.StatusBarColor.ToJavaColor());

			InBrainInst?.Call(Constants.SetStatusBarConfigJavaMethod, javaConfig);
		}

		public void GetSurveysWithFilter(InBrainSurveyFilter filter, Action<List<InBrainSurvey>> onSurveysReceived)
		{
			InBrainInst?.Call(Constants.GetSurveysJavaMethod, filter?.ToAJO(), new InBrainGetSurveysCallbackProxy(onSurveysReceived));
		}

		public void GetCurrencySale(Action<InBrainCurrencySale> onCurrencySaleReceived)
		{
			InBrainInst?.Call(Constants.GetCurrencySaleJavaMethod, new InBrainCurrencySaleCallbackProxy(onCurrencySaleReceived));
		}
	}
}