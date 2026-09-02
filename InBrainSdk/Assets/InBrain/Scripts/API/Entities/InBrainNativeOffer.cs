using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace InBrain
{
	[Serializable]
	public class InBrainNativeOffer
	{
		[SerializeField] public int id;
		[SerializeField] public string title;
		[SerializeField] public float reward;
		[SerializeField] public string rewardString;
		[SerializeField] public int featuredRank;
		[SerializeField] public string thumbnailUrl;
		[SerializeField] public string heroImageUrl;
		[SerializeField] public List<string> offerDescription = new List<string>();
		[SerializeField] public List<string> instructions = new List<string>();
		[SerializeField] public List<string> requirements = new List<string>();
		[SerializeField] public List<string> tags = new List<string>();
		[SerializeField] public List<string> categories = new List<string>();
		[SerializeField] public InBrainOfferPromotion promotion;
		[SerializeField] public List<InBrainOfferGoal> standardGoals = new List<InBrainOfferGoal>();
		[SerializeField] public List<InBrainOfferGoal> purchaseGoals = new List<InBrainOfferGoal>();
		[SerializeField] public int attributionWindowMinutes;
		[SerializeField] public string attemptedAt;
		[SerializeField] public string completeBy;
		[SerializeField] public InBrainCurrencySale campaignCurrencySale;

		public DateTime? AttemptedAtDate => ParseIsoDate(attemptedAt);
		public DateTime? CompleteByDate => ParseIsoDate(completeBy);

		public InBrainNativeOffer(int id, string title, float reward, string rewardString, int featuredRank,
			string thumbnailUrl, string heroImageUrl, List<string> offerDescription, List<string> instructions,
			List<string> requirements, List<string> tags, List<string> categories, InBrainOfferPromotion promotion,
			List<InBrainOfferGoal> standardGoals, List<InBrainOfferGoal> purchaseGoals, int attributionWindowMinutes,
			string attemptedAt, string completeBy, InBrainCurrencySale campaignCurrencySale)
		{
			this.id = id;
			this.title = title;
			this.reward = reward;
			this.rewardString = rewardString;
			this.featuredRank = featuredRank;
			this.thumbnailUrl = thumbnailUrl;
			this.heroImageUrl = heroImageUrl;
			this.offerDescription = offerDescription ?? new List<string>();
			this.instructions = instructions ?? new List<string>();
			this.requirements = requirements ?? new List<string>();
			this.tags = tags ?? new List<string>();
			this.categories = categories ?? new List<string>();
			this.promotion = promotion;
			this.standardGoals = standardGoals ?? new List<InBrainOfferGoal>();
			this.purchaseGoals = purchaseGoals ?? new List<InBrainOfferGoal>();
			this.attributionWindowMinutes = attributionWindowMinutes;
			this.attemptedAt = attemptedAt;
			this.completeBy = completeBy;
			this.campaignCurrencySale = campaignCurrencySale;
		}

		public static InBrainNativeOffer FromAJO(AndroidJavaObject ajo)
		{
			return new InBrainNativeOffer(
				ajo.Call<int>("getId"),
				ajo.Call<string>("getTitle"),
				(float) ajo.Call<double>("getReward"),
				ajo.Call<string>("getRewardString"),
				ajo.Call<int>("getFeaturedRank"),
				ajo.Call<string>("getThumbnailUrl"),
				ajo.Call<string>("getHeroImageUrl"),
				ajo.CallAJO("getDescription").FromJavaList<string>(),
				ajo.CallAJO("getInstructions").FromJavaList<string>(),
				ajo.CallAJO("getRequirements").FromJavaList<string>(),
				ajo.CallAJO("getTags").FromJavaList<string>(),
				ajo.CallAJO("getCategories").FromJavaList<string>(),
				InBrainOfferPromotion.FromAJO(ajo.CallAJO("getPromotion")),
				ajo.CallAJO("getStandardGoals").FromJavaList(InBrainOfferGoal.FromAJO),
				ajo.CallAJO("getPurchaseGoals").FromJavaList(InBrainOfferGoal.FromAJO),
				ajo.Call<int>("getAttributionWindowMinutes"),
				JniUtils.FromJavaDate(ajo.CallAJO("getAttemptedAt")),
				JniUtils.FromJavaDate(ajo.CallAJO("getCompleteBy")),
				FromCurrencySaleAJO(ajo.CallAJO("getCampaignCurrencySale")));
		}

		public override string ToString()
		{
			return string.Format(
				"id: {0}, title: {1}, reward: {2}, rewardString: {3}, featuredRank: {4}, thumbnailUrl: {5}, heroImageUrl: {6}, offerDescription: {7}, instructions: {8}, requirements: {9}, tags: {10}, categories: {11}, promotion: {12}, standardGoals: {13}, purchaseGoals: {14}, attributionWindowMinutes: {15}, attemptedAt: {16}, completeBy: {17}, campaignCurrencySale: {18}",
				id, title, reward, rewardString, featuredRank, thumbnailUrl, heroImageUrl,
				string.Join(";", offerDescription.ToArray()), string.Join(";", instructions.ToArray()),
				string.Join(";", requirements.ToArray()), string.Join(";", tags.ToArray()),
				string.Join(";", categories.ToArray()), promotion, standardGoals.Count, purchaseGoals.Count,
				attributionWindowMinutes, attemptedAt, completeBy, campaignCurrencySale);
		}

		static InBrainCurrencySale FromCurrencySaleAJO(AndroidJavaObject ajo)
		{
			return ajo.IsJavaNull() ? null : InBrainCurrencySale.FromAJO(ajo);
		}

		static DateTime? ParseIsoDate(string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				return null;
			}

			DateTime parsed;
			return DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out parsed)
				? parsed
				: (DateTime?) null;
		}
	}
}
