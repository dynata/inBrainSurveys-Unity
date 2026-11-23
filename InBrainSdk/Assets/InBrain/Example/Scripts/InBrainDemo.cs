using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace InBrain
{
	public class InBrainDemo : MonoBehaviour
	{
		static readonly Color CurrencySaleBadgeColor = new Color(0.871f, 0.514f, 0.267f);

		[SerializeField] string appUserId = "testing-unity@inbrain.ai";

		[SerializeField] InBrainSurveysListPanel inBrainSurveysListPanel = null;
		[SerializeField] InBrainOffersListPanel inBrainOffersListPanel = null;

		[Space] [SerializeField] Text balanceText = null;

		List<InBrainReward> _receivedRewards;
		GameObject _currencySaleBadge;
		Text _currencySaleText;

		void Start()
		{
			_receivedRewards = new List<InBrainReward>();

			InBrain.Instance.Init();
			InBrain.Instance.SetAppUserId(appUserId);

			InBrain.Instance.AddCallback(ProcessRewards, ProcessWebViewDismissed);
			InBrain.Instance.GetRewards();

			// Uncomment following two lines of code in order to customize surveys wall UI
			// SetStatusBarConfiguration();
			// SetToolbarConfiguration();

			InBrain.Instance.CheckSurveysAvailability(flag =>
			{
				Debug.Log("Surveys availability: " + flag);
			});

			FetchCurrencySale();
		}

		public void OnOpenWallClicked()
		{
			Debug.Log("InBrain: OpenWall (All) button clicked");
			InBrain.Instance.OpenWall();
		}

		public void OnShowSurveysClicked()
		{
			Debug.Log("InBrain: OpenWall (Surveys) button clicked");
			InBrain.Instance.OpenWall(InBrainWallOption.SURVEYS);
		}

		public void OnShowOffersClicked()
		{
			Debug.Log("InBrain: OpenWall (Offers) button clicked");
			InBrain.Instance.OpenWall(InBrainWallOption.OFFERS);
		}

		public void OnShowSurveysListClicked()
		{
			inBrainSurveysListPanel?.Show();
		}

		public void OnShowOffersListClicked()
		{
			EnsureOffersListPanel();
			inBrainOffersListPanel?.Show();
		}

		public void OnGetRewardsClicked()
		{
			Debug.Log("InBrain: GetRewards button clicked");
			InBrain.Instance.GetRewards(ProcessRewards, () => { Debug.LogError("InBrain: Failed to receive rewards"); });
		}

		public void OnConfirmRewardsClicked()
		{
			Debug.Log("InBrain: ConfirmRewards button clicked");

			if (_receivedRewards.Any())
			{
				InBrain.Instance.ConfirmRewards(_receivedRewards);
				_receivedRewards.Clear();
			}
			else
			{
				Debug.Log("InBrain: There are no rewards to confirm");
			}
		}

		void ProcessRewards(List<InBrainReward> rewards)
		{
			Debug.Log("InBrain: Rewards callback received");

			_receivedRewards = rewards;

			var balance = rewards.Any() ? (int) rewards.Sum(reward => reward.amount) : 0;
			balanceText.text = $"Total Points: {balance}";

			Debug.Log($"InBrain: Pending rewards amount: {balance}");
		}

		void ProcessWebViewDismissed(InBrainRewardsViewDismissedResult result)
		{
			Debug.Log($"InBrain: Surveys web view was dismissed (by WebView: {result.byWebView})");

			FetchCurrencySale();

			if (inBrainOffersListPanel != null && inBrainOffersListPanel.gameObject.activeInHierarchy)
			{
				inBrainOffersListPanel.Refresh();
			}
		}

		void FetchCurrencySale()
		{
			InBrain.Instance.GetCurrencySaleFor(InBrainCurrencySaleType.Surveys, sale =>
			{
				if (sale != null)
				{
					Debug.Log(sale.ToString());
				}

				UpdateCurrencySaleBadge(sale);
			});
		}

		void UpdateCurrencySaleBadge(InBrainCurrencySale sale)
		{
			if (sale == null)
			{
				if (_currencySaleBadge != null)
				{
					_currencySaleBadge.SetActive(false);
				}

				return;
			}

			EnsureCurrencySaleBadge();
			if (_currencySaleBadge == null || _currencySaleText == null)
			{
				return;
			}

			var roundedMultiplier = Mathf.Round(sale.multiplier * 10f) / 10f;
			var multiplierText = roundedMultiplier.ToString("0.#", CultureInfo.InvariantCulture);
			_currencySaleText.text = string.Format("<b><size=30>{0}x</size></b> Earnings", multiplierText);
			_currencySaleBadge.SetActive(true);
		}

		void EnsureCurrencySaleBadge()
		{
			if (_currencySaleBadge != null)
			{
				return;
			}

			var startPanel = transform.Find("StartPanel") ?? transform;
			var badge = new GameObject("CurrencySaleBadge", typeof(RectTransform), typeof(Image), typeof(HorizontalLayoutGroup), typeof(ContentSizeFitter));
			badge.transform.SetParent(startPanel, false);

			var rt = badge.GetComponent<RectTransform>();
			rt.anchorMin = new Vector2(1f, 1f);
			rt.anchorMax = new Vector2(1f, 1f);
			rt.pivot = new Vector2(1f, 1f);
			rt.anchoredPosition = new Vector2(-36f, -36f);

			var bg = badge.GetComponent<Image>();
			bg.color = CurrencySaleBadgeColor;
			bg.sprite = InBrainOffersUi.RoundedSprite;
			if (bg.sprite != null)
			{
				bg.type = Image.Type.Sliced;
			}
			bg.raycastTarget = false;

			var layout = badge.GetComponent<HorizontalLayoutGroup>();
			layout.padding = new RectOffset(24, 24, 10, 10);
			layout.childAlignment = TextAnchor.MiddleCenter;
			layout.childControlWidth = true;
			layout.childControlHeight = true;
			layout.childForceExpandWidth = false;
			layout.childForceExpandHeight = false;

			var fitter = badge.GetComponent<ContentSizeFitter>();
			fitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
			fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

			var label = InBrainOffersUi.CreateText(badge.transform, "Text", string.Empty, 26, FontStyle.Normal, Color.white, TextAnchor.MiddleCenter);
			label.supportRichText = true;
			label.horizontalOverflow = HorizontalWrapMode.Overflow;

			badge.SetActive(false);
			_currencySaleBadge = badge;
			_currencySaleText = label;
		}

		void EnsureOffersListPanel()
		{
			if (inBrainOffersListPanel != null)
			{
				return;
			}

			var panel = new GameObject("OffersListPanel", typeof(RectTransform));
			panel.SetActive(false);
			panel.transform.SetParent(transform, false);
			InBrainOffersUi.Stretch(panel.GetComponent<RectTransform>());
			inBrainOffersListPanel = panel.AddComponent<InBrainOffersListPanel>();
		}

		void SetStatusBarConfiguration()
		{
			var statusBarConfig = new InBrainStatusBarConfig
			{
				StatusBarColor = Color.green,
				LightStatusBarIcons = false,
				HideStatusBarIos = false
			};
			InBrain.Instance.SetStatusBarConfig(statusBarConfig);
		}

		void SetToolbarConfiguration()
		{
			var toolbarConfig = new InBrainToolbarConfig
			{
				Title = "InBrain Demo (Unity)",
				ElevationEnabled = false,
				TitleColor = Color.red,
				ToolbarColor = Color.white,
				BackButtonColor = Color.magenta
			};
			InBrain.Instance.SetToolbarConfig(toolbarConfig);
		}
	}
}