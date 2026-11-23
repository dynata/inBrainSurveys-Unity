using System;
using System.Globalization;
using UnityEngine;

namespace InBrain
{
	[Serializable]
	public class InBrainOfferGoal
	{
		[SerializeField] public int id;
		[SerializeField] public string title;
		[SerializeField] public string goalDescription;
		[SerializeField] public float reward;
		[SerializeField] public string rewardString;
		[SerializeField] public bool isCompleted;
		[SerializeField] public bool isPending;
		[SerializeField] public int sortOrder;
		[SerializeField] public int attributionWindowMinutes;
		[SerializeField] public string completeBy;
		[SerializeField] public InBrainOfferPromotion promotion;

		public DateTime? CompleteByDate => ParseIsoDate(completeBy);

		public InBrainOfferGoal(int id, string title, string goalDescription, float reward, string rewardString,
			bool isCompleted, int sortOrder, int attributionWindowMinutes, string completeBy, InBrainOfferPromotion promotion)
			: this(id, title, goalDescription, reward, rewardString, isCompleted, false, sortOrder,
				attributionWindowMinutes, completeBy, promotion)
		{
		}

		public InBrainOfferGoal(int id, string title, string goalDescription, float reward, string rewardString,
			bool isCompleted, bool isPending, int sortOrder, int attributionWindowMinutes, string completeBy,
			InBrainOfferPromotion promotion)
		{
			this.id = id;
			this.title = title;
			this.goalDescription = goalDescription;
			this.reward = reward;
			this.rewardString = rewardString;
			this.isCompleted = isCompleted;
			this.isPending = isPending;
			this.sortOrder = sortOrder;
			this.attributionWindowMinutes = attributionWindowMinutes;
			this.completeBy = completeBy;
			this.promotion = promotion;
		}

		public static InBrainOfferGoal FromAJO(AndroidJavaObject ajo)
		{
			return new InBrainOfferGoal(
				ajo.Call<int>("getId"),
				ajo.Call<string>("getTitle"),
				ajo.Call<string>("getDescription"),
				(float) ajo.Call<double>("getReward"),
				ajo.Call<string>("getRewardString"),
				ajo.Call<bool>("isCompleted"),
				ajo.Call<bool>("isPending"),
				ajo.Call<int>("getSortOrder"),
				ajo.Call<int>("getAttributionWindowMinutes"),
				JniUtils.FromJavaDate(ajo.CallAJO("getCompleteBy")),
				InBrainOfferPromotion.FromAJO(ajo.CallAJO("getPromotion")));
		}

		public override string ToString()
		{
			return string.Format(
				"id: {0}, title: {1}, goalDescription: {2}, reward: {3}, rewardString: {4}, isCompleted: {5}, isPending: {6}, sortOrder: {7}, attributionWindowMinutes: {8}, completeBy: {9}, promotion: {10}",
				id, title, goalDescription, reward, rewardString, isCompleted, isPending, sortOrder, attributionWindowMinutes, completeBy, promotion);
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
