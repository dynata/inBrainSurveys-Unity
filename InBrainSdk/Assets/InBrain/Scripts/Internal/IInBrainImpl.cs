using System;
using System.Collections.Generic;

namespace InBrain
{
	public interface IInBrainImpl
	{
		void Init(string clientId, string clientSecret, bool isS2S);
		void Init(string clientId, string clientSecret, bool isS2S, string userId);
		void SetUserId(string userId);
		void SetSessionId(string sessionId);
		void SetDemographicData(InBrainDemographicData demographicData);
		void AddCallback(Action<List<InBrainReward>> onRewardsReceived, Action<InBrainRewardsViewDismissedResult> onRewardsViewDismissed, bool confirmRewardsAutomatically = false);
		void RemoveCallback();
		void CheckSurveysAvailability(Action<bool> onAvailabilityChecked);
		void OpenWall(InBrainWallOption wallOption);
		void OpenSurvey(string surveyId, string searchId, bool offersEnabled);
		void GetRewards();
		void GetRewards(Action<List<InBrainReward>> onRewardsReceived, Action onFailedToReceiveRewards, bool confirmRewardsAutomatically = false);
		void ConfirmRewards(List<InBrainReward> rewards);
		void SetToolbarConfig(InBrainToolbarConfig config);
		void SetStatusBarConfig(InBrainStatusBarConfig config);
		void GetSurveysWithFilter(InBrainSurveyFilter filter, Action<List<InBrainSurvey>> onSurveysReceived);
		void GetCurrencySale(Action<InBrainCurrencySale> onCurrencySaleReceived);
	}
}