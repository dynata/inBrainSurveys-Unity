using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace InBrain
{
	[PublicAPI]
	public class InBrain : MonoSingleton<InBrain>
	{
		IInBrainImpl _inBrainImpl;

		IInBrainImpl InBrainImpl
		{
			get
			{
				if (_inBrainImpl == null)
				{
					Debug.LogWarning("InBrain implementation is not instantiated or current platform is not supported");
				}

				return _inBrainImpl;
			}
		}

		protected override void Initialize()
		{
			// Depending on runtime platform create corresponding implementation

			if (Application.platform == RuntimePlatform.Android)
			{
				_inBrainImpl = new InBrainAndroidImpl();
			}

#if UNITY_IOS && !UNITY_EDITOR
			_inBrainImpl = new InBrainIosImpl();
#endif
		}
		
		/// <summary>
		/// Initialize InBrain SDK
		/// </summary>
		public void Init()
		{
			InBrainImpl?.Init(InBrainSettings.ClientId, InBrainSettings.ClientSecretKey, InBrainSettings.IsS2S);
		}

		/// <summary>
		/// Initialize InBrain SDK with optional user ID
		/// </summary>
		/// <param name="appUserId">Uniques user ID</param>
		public void Init([NotNull] string appUserId)
		{
			InBrainImpl?.Init(InBrainSettings.ClientId, InBrainSettings.ClientSecretKey, InBrainSettings.IsS2S, appUserId);
		}

		/// <summary>
		/// Set unique user ID required for interaction with InBrain surveys API
		/// </summary>
		/// <param name="appUserId">Uniques user ID</param>
		public void SetAppUserId([NotNull] string appUserId)
		{
			InBrainImpl?.SetUserId(appUserId);
		}

		/// <summary>
		/// Set unique session ID provided in the S2S callback which is required for user session tracking
		/// </summary>
		/// <param name="sessionId">Uniques session ID</param>
		public void SetSessionId([NotNull] string sessionId)
		{
			InBrainImpl?.SetSessionId(sessionId);
		}

		/// <summary>
		/// Set additional demographic data for enhancing InBrain experience
		/// </summary>
		/// <param name="demographicData">Additional data to provide seamless user on-boarding into the InBrain experience</param>
		public void SetDemographicData([NotNull] InBrainDemographicData demographicData)
		{
			InBrainImpl?.SetDemographicData(demographicData);
		}

		/// <summary>
		/// Set additional data for enhancing InBrain experience
		/// </summary>
		/// <param name="trackingData">Additional data that will be provided in the S2S callback</param>
		/// <param name="demographicData">Additional data to provide seamless user on-boarding into the InBrain experience</param>
		[Obsolete("This method is deprecated. Use `SetSessionId` and `SetDemographicData` methods instead", true)]
		public void SetCustomData([CanBeNull] InBrainTrackingData trackingData, [CanBeNull] InBrainDemographicData demographicData)
		{
			// Implementation removed due to its deprecation
		}

		/// <summary>
		/// Set global callback that will be triggered every time when new rewards received or surveys web view dismissed
		/// </summary>
		/// <param name="onRewardsReceived">Callback triggered when new rewards received</param>
		/// <param name="onRewardsViewDismissed">Callback triggered when surveys web view dismissed</param>
		/// <param name="confirmRewardsAutomatically">Flag indicating whether to confirm received rewards automatically</param>
		public void AddCallback([NotNull] Action<List<InBrainReward>> onRewardsReceived, [NotNull] Action<InBrainRewardsViewDismissedResult> onRewardsViewDismissed,
			bool confirmRewardsAutomatically = false)
		{
			InBrainImpl?.AddCallback(onRewardsReceived, onRewardsViewDismissed, confirmRewardsAutomatically);
		}

		/// <summary>
		/// Remove global callback to stop receiving notifications related to new rewards or surveys web view dismissal
		/// </summary>
		public void RemoveCallback()
		{
			InBrainImpl?.RemoveCallback();
		}

		/// <summary>
		/// Check if there are any surveys available
		/// </summary>
		/// <param name="onAvailabilityChecked">
		/// Callback triggered when surveys availability checked
		/// Received flag is true if there are surveys available, false otherwise
		/// </param>
		public void CheckSurveysAvailability([NotNull] Action<bool> onAvailabilityChecked)
		{
			InBrainImpl?.CheckSurveysAvailability(onAvailabilityChecked);
		}
		
		/// <summary>
		/// Open surveys wall (legacy API)
		/// </summary>
		[Obsolete("This method is deprecated. Use `void OpenWall(InBrainWallOption wallOption)` method instead.")]
		public void ShowSurveys()
		{
			InBrainImpl?.OpenWall(InBrainWallOption.ALL);
		}

		/// <summary>
		/// Open a specific wall
		/// </summary>
		/// <param name="wallOption">Wall type to open</param>
		public void OpenWall(InBrainWallOption wallOption = InBrainWallOption.ALL)
		{
			InBrainImpl?.OpenWall(wallOption);
		}

		/// <summary>
		/// Open web view for specified survey
		/// </summary>
		[Obsolete("This method is deprecated. Use `void ShowSurvey(string surveyId, string searchId)` method instead.", true)]
		public void ShowSurvey(string surveyId)
		{
			// Implementation removed due to its deprecation
		}

		/// <summary>
		/// Open web view for specified survey with given search identifier
		/// </summary>
		/// <param name="surveyId">Specific survey identifier</param>
		/// <param name="searchId">Search identifier</param>
		[Obsolete("This method is deprecated. Use `OpenSurvey(string surveyId, string searchId, bool offersEnabled)` method instead.")]
		public void ShowSurvey(string surveyId, string searchId)
		{
			OpenSurvey(surveyId, searchId, true);
		}

		/// <summary>
		/// Open web view for specified survey with given search identifier
		/// </summary>
		/// <param name="surveyId">Specific survey identifier</param>
		/// <param name="searchId">Search identifier</param>
		/// <param name="offersEnabled">Whether offers are enabled for the survey view.</param>
		public void OpenSurvey(string surveyId, string searchId, bool offersEnabled = true)
		{
			InBrainImpl?.OpenSurvey(surveyId, searchId, offersEnabled);
		}

		/// <summary>
		/// Request list of pending (unconfirmed) rewards. Rewards list can be obtained via global callback. See AddCallback for more details
		/// </summary>
		public void GetRewards()
		{
			InBrainImpl?.GetRewards();
		}

		/// <summary>
		/// Request list of pending (unconfirmed) rewards. Rewards list can be obtained via provided callbacks
		/// </summary>
		/// <param name="onRewardsReceived">Callback triggered when new rewards received</param>
		/// <param name="onFailedToReceiveRewards">Callback triggered in case error occured during requesting rewards</param>
		/// <param name="confirmRewardsAutomatically">Flag indicating whether to confirm received rewards automatically</param>
		public void GetRewards([NotNull] Action<List<InBrainReward>> onRewardsReceived, [CanBeNull] Action onFailedToReceiveRewards,
			bool confirmRewardsAutomatically = false)
		{
			InBrainImpl?.GetRewards(onRewardsReceived, onFailedToReceiveRewards, confirmRewardsAutomatically);
		}

		/// <summary>
		/// Confirm rewards
		/// </summary>
		/// <param name="rewards">List of rewards to be confirmed</param>
		public void ConfirmRewards([NotNull] List<InBrainReward> rewards)
		{
			InBrainImpl?.ConfirmRewards(rewards);
		}

		/// <summary>
		/// Set surveys web view language
		/// </summary>
		/// <param name="language">
		/// Language to use in surveys web view
		/// Accepted languages: "en-us", "en-gb", "en-ca", "en-au", "en-in", "de-de", "es-es", "es-mx", "es-us", "fr-fr", "fr-ca", "fr-br"
		/// </param>
		[Obsolete("This method is deprecated.", true)]
		public void SetLanguage(string language)
		{
			// Implementation removed due to its deprecation
		}

		/// <summary>
		/// Set custom style for the surveys web view toolbar
		/// </summary>
		/// <param name="config">Toolbar configuration parameters</param>
		public void SetToolbarConfig([NotNull] InBrainToolbarConfig config)
		{
			InBrainImpl?.SetToolbarConfig(config);
		}

		/// <summary>
		/// Set custom style for the surveys web view status bar
		/// </summary>
		/// <param name="config">Status bar configuration parameters</param>
		public void SetStatusBarConfig([NotNull] InBrainStatusBarConfig config)
		{
			InBrainImpl?.SetStatusBarConfig(config);
		}

		/// <summary>
		/// Request list of available surveys
		/// </summary>
		/// <param name="onSurveysReceived">Callback triggered when surveys received</param>
		[Obsolete("This method is deprecated. Use `GetSurveysWithFilter` instead.")]
		public void GetSurveys([NotNull] Action<List<InBrainSurvey>> onSurveysReceived)
		{
			InBrainImpl?.GetSurveysWithFilter(null, onSurveysReceived);
		}

		/// <summary>
		/// Request list of available surveys with given placement identifier
		/// </summary>
		/// <param name="placementId">Placement identifier</param>
		/// <param name="onSurveysReceived">Callback triggered when surveys received</param>
		[Obsolete("This method is deprecated. Use `GetSurveysWithFilter` instead.")]
		public void GetSurveys(string placementId, [NotNull] Action<List<InBrainSurvey>> onSurveysReceived)
		{
			var filter = new InBrainSurveyFilter(placementId);
			InBrainImpl?.GetSurveysWithFilter(filter, onSurveysReceived);
		}

		/// <summary>
		/// Request list of available surveys matching given filter
		/// </summary>
		/// <param name="filter">Surveys filter</param>
		/// <param name="onSurveysReceived">Callback triggered when surveys received</param>
		public void GetSurveysWithFilter(InBrainSurveyFilter filter, [NotNull] Action<List<InBrainSurvey>> onSurveysReceived)
		{
			InBrainImpl?.GetSurveysWithFilter(filter, onSurveysReceived);
		}

		/// <summary>
		/// Request information about active currency sale
		/// </summary>
		/// <param name="onCurrencySaleReceived">Callback triggered when currency sale received</param>
		public void GetCurrencySale([NotNull] Action<InBrainCurrencySale> onCurrencySaleReceived)
		{
			InBrainImpl?.GetCurrencySale(onCurrencySaleReceived);
		}
	}
}