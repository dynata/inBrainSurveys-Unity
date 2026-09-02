using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace InBrain
{
	public class InBrainOffersListPanel : MonoBehaviour
	{
		readonly List<InBrainOfferListItem> _offers = new List<InBrainOfferListItem>();

		Transform _offersContent;
		GameObject _loadingIcon;
		GameObject _emptyState;
		bool _built;
		int _fetchId;

		public void Show()
		{
			EnsureUi();
			gameObject.SetActive(true);
			Refresh();
		}

		public void OnBackButtonClicked()
		{
			gameObject.SetActive(false);
		}

		public void Refresh()
		{
			EnsureUi();
			ClearList();
			SetLoading(true);
			SetEmpty(false);

			var fetchId = ++_fetchId;
			var filter = new InBrainOfferFilter(InBrainOfferType.Default, 20);
			InBrain.Instance.GetNativeOffers(filter, offers => OnOffersFetched(fetchId, offers));
		}

		void OnDisable()
		{
			ClearList();
		}

		void ClearList()
		{
			_offers.ForEach(offer =>
			{
				if (offer != null)
				{
					Destroy(offer.gameObject);
				}
			});
			_offers.Clear();
		}

		void OnOffersFetched(int fetchId, List<InBrainNativeOffer> offers)
		{
			if (fetchId != _fetchId || !isActiveAndEnabled)
			{
				return;
			}

			ClearList();
			SetLoading(false);

			if (offers == null || offers.Count == 0)
			{
				SetEmpty(true);
				Debug.Log("InBrain: No native offers available");
				return;
			}

			SetEmpty(false);
			foreach (var offer in offers)
			{
				var card = new GameObject("OfferCard", typeof(RectTransform), typeof(InBrainOfferListItem));
				card.transform.SetParent(_offersContent, false);
				var item = card.GetComponent<InBrainOfferListItem>();
				item.Init(offer);
				_offers.Add(item);
				Debug.Log(offer);
			}
		}

		void SetLoading(bool visible)
		{
			if (_loadingIcon != null)
			{
				_loadingIcon.SetActive(visible);
			}
		}

		void SetEmpty(bool visible)
		{
			if (_emptyState != null)
			{
				_emptyState.SetActive(visible);
			}
		}

		void EnsureUi()
		{
			if (_built)
			{
				return;
			}

			_built = true;
			BuildScreen();
		}

		void BuildScreen()
		{
			var root = GetComponent<RectTransform>() ?? gameObject.AddComponent<RectTransform>();
			InBrainOffersUi.Stretch(root);

			var background = GetComponent<Image>() ?? gameObject.AddComponent<Image>();
			background.color = InBrainOffersUi.ScreenBackground;
			background.raycastTarget = true;

			BuildHeader(root);
			BuildList(root);
			BuildEmptyState(root);
			BuildLoading(root);
		}

		void BuildHeader(RectTransform parent)
		{
			var surveysHeader = FindSurveysHeader();
			if (surveysHeader != null)
			{
				var header = Instantiate(surveysHeader.gameObject, parent);
				header.name = "Header";
				header.SetActive(true);

				var title = header.transform.Find("TitleText");
				if (title != null)
				{
					var titleText = title.GetComponent<Text>();
					if (titleText != null)
					{
						titleText.text = "Native Offers";
					}
				}

				var back = header.transform.Find("BackButton");
				if (back != null)
				{
					var button = back.GetComponent<Button>();
					if (button != null)
					{
						button.onClick = new Button.ButtonClickedEvent();
						button.onClick.AddListener(OnBackButtonClicked);
					}
				}

				return;
			}

			BuildFallbackHeader(parent);
		}

		static Transform FindSurveysHeader()
		{
			var surveysPanel = FindFirstObjectByType<InBrainSurveysListPanel>(FindObjectsInactive.Include);
			return surveysPanel != null ? surveysPanel.transform.Find("Header") : null;
		}

		void BuildFallbackHeader(RectTransform parent)
		{
			var header = new GameObject("Header", typeof(RectTransform), typeof(Image));
			header.transform.SetParent(parent, false);
			var headerRt = header.GetComponent<RectTransform>();
			headerRt.anchorMin = new Vector2(0, 1);
			headerRt.anchorMax = new Vector2(1, 1);
			headerRt.pivot = new Vector2(0.5f, 0.5f);
			headerRt.sizeDelta = new Vector2(0, 300);
			headerRt.anchoredPosition = new Vector2(0, -150);
			header.GetComponent<Image>().color = InBrainOffersUi.HeaderBackground;

			var clouds = new GameObject("CloudsImage", typeof(RectTransform), typeof(Image));
			clouds.transform.SetParent(header.transform, false);
			var cloudsRt = clouds.GetComponent<RectTransform>();
			cloudsRt.anchorMin = Vector2.zero;
			cloudsRt.anchorMax = Vector2.one;
			cloudsRt.anchoredPosition = new Vector2(0, -50);
			cloudsRt.sizeDelta = new Vector2(0, -100);
			var cloudsImage = clouds.GetComponent<Image>();
			cloudsImage.sprite = InBrainOffersUi.CloudsSprite;
			cloudsImage.color = Color.white;
			cloudsImage.raycastTarget = false;
			cloudsImage.preserveAspect = false;

			var title = InBrainOffersUi.CreateText(header.transform, "TitleText", "Native Offers", 48, FontStyle.Bold, Color.white, TextAnchor.MiddleCenter);
			title.font = InBrainOffersUi.TitleFont;
			var titleRt = title.GetComponent<RectTransform>();
			titleRt.anchorMin = new Vector2(0, 1);
			titleRt.anchorMax = new Vector2(1, 1);
			titleRt.pivot = new Vector2(0.5f, 0.5f);
			titleRt.anchoredPosition = new Vector2(0, -100);
			titleRt.sizeDelta = new Vector2(-400, 70);

			var back = new GameObject("BackButton", typeof(RectTransform), typeof(Button));
			back.transform.SetParent(header.transform, false);
			var backRt = back.GetComponent<RectTransform>();
			backRt.anchorMin = new Vector2(0, 1);
			backRt.anchorMax = new Vector2(0, 1);
			backRt.pivot = new Vector2(0.5f, 0.5f);
			backRt.anchoredPosition = new Vector2(100, -100);
			backRt.sizeDelta = new Vector2(150, 50);
			var backButton = back.GetComponent<Button>();
			backButton.transition = Selectable.Transition.None;
			backButton.onClick.AddListener(OnBackButtonClicked);

			var arrow = new GameObject("Image", typeof(RectTransform), typeof(Image));
			arrow.transform.SetParent(back.transform, false);
			var arrowRt = arrow.GetComponent<RectTransform>();
			arrowRt.anchorMin = arrowRt.anchorMax = new Vector2(0, 0.5f);
			arrowRt.pivot = new Vector2(0.5f, 0.5f);
			arrowRt.anchoredPosition = new Vector2(30, 0);
			arrowRt.sizeDelta = new Vector2(45, 45);
			var arrowImage = arrow.GetComponent<Image>();
			arrowImage.sprite = InBrainOffersUi.BackArrowSprite;
			arrowImage.color = Color.white;
			arrowImage.raycastTarget = false;

			var backLabel = InBrainOffersUi.CreateText(back.transform, "Text", "Back", 42, FontStyle.Normal, Color.white, TextAnchor.MiddleLeft);
			var backLabelRt = backLabel.GetComponent<RectTransform>();
			InBrainOffersUi.Stretch(backLabelRt);
			backLabelRt.anchoredPosition = new Vector2(27.5f, 0);
			backLabelRt.sizeDelta = new Vector2(-55, 0);
		}

		void BuildList(RectTransform parent)
		{
			var list = new GameObject("OffersList", typeof(RectTransform), typeof(ScrollRect), typeof(Image));
			list.transform.SetParent(parent, false);
			var listRt = list.GetComponent<RectTransform>();
			listRt.anchorMin = Vector2.zero;
			listRt.anchorMax = Vector2.one;
			listRt.offsetMin = new Vector2(0, 0);
			listRt.offsetMax = new Vector2(0, -300);
			list.GetComponent<Image>().color = InBrainOffersUi.ScreenBackground;

			var viewport = new GameObject("Viewport", typeof(RectTransform), typeof(Image), typeof(Mask));
			viewport.transform.SetParent(list.transform, false);
			var viewportRt = viewport.GetComponent<RectTransform>();
			InBrainOffersUi.Stretch(viewportRt);
			viewport.GetComponent<Image>().color = InBrainOffersUi.ScreenBackground;
			viewport.GetComponent<Mask>().showMaskGraphic = false;

			var content = new GameObject("Content", typeof(RectTransform), typeof(GridLayoutGroup), typeof(ContentSizeFitter));
			content.transform.SetParent(viewport.transform, false);
			var contentRt = content.GetComponent<RectTransform>();
			contentRt.anchorMin = new Vector2(0, 1);
			contentRt.anchorMax = new Vector2(1, 1);
			contentRt.pivot = new Vector2(0.5f, 1);
			contentRt.anchoredPosition = Vector2.zero;
			contentRt.sizeDelta = Vector2.zero;

			var grid = content.GetComponent<GridLayoutGroup>();
			grid.startCorner = GridLayoutGroup.Corner.UpperLeft;
			grid.startAxis = GridLayoutGroup.Axis.Horizontal;
			grid.childAlignment = TextAnchor.UpperCenter;
			grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
			grid.constraintCount = 2;
			content.AddComponent<InBrainOffersGridFitter>();

			var fitter = content.GetComponent<ContentSizeFitter>();
			fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

			var scroll = list.GetComponent<ScrollRect>();
			scroll.content = contentRt;
			scroll.viewport = viewportRt;
			scroll.horizontal = false;
			scroll.vertical = true;
			scroll.movementType = ScrollRect.MovementType.Elastic;

			_offersContent = content.transform;
		}

		void BuildEmptyState(RectTransform parent)
		{
			var empty = new GameObject("EmptyState", typeof(RectTransform));
			empty.transform.SetParent(parent, false);
			var emptyRt = empty.GetComponent<RectTransform>();
			InBrainOffersUi.Stretch(emptyRt);
			emptyRt.offsetMax = new Vector2(0, -300);
			InBrainOffersUi.CreateText(empty.transform, "Text", "Ooops... No offers available right now!", 32, FontStyle.Normal, new Color(0.3f, 0.3f, 0.3f), TextAnchor.MiddleCenter);
			var textRt = empty.transform.Find("Text").GetComponent<RectTransform>();
			InBrainOffersUi.Stretch(textRt);
			empty.SetActive(false);
			_emptyState = empty;
		}

		void BuildLoading(RectTransform parent)
		{
			var loading = new GameObject("LoadingIcon", typeof(RectTransform), typeof(Image));
			loading.transform.SetParent(parent, false);
			var loadingRt = loading.GetComponent<RectTransform>();
			loadingRt.anchorMin = loadingRt.anchorMax = new Vector2(0.5f, 0.5f);
			loadingRt.sizeDelta = new Vector2(80, 80);
			var image = loading.GetComponent<Image>();
			image.color = new Color(0.573f, 0.816f, 0.314f, 0.85f);
			image.sprite = InBrainOffersUi.RoundedSprite;
			image.type = Image.Type.Sliced;
			loading.AddComponent<InBrainLoadingCircle>();
			_loadingIcon = loading;
		}
	}

	class InBrainOffersGridFitter : MonoBehaviour
	{
		const float SidePadding = 40f;
		const float VerticalPadding = 24f;
		const float Spacing = 16f;
		const float CellAspect = 620f / 460f;

		GridLayoutGroup _grid;
		RectTransform _rect;

		void OnEnable()
		{
			Apply();
		}

		void Start()
		{
			Apply();
		}

		void OnRectTransformDimensionsChange()
		{
			Apply();
		}

		void Apply()
		{
			if (_grid == null)
			{
				_grid = GetComponent<GridLayoutGroup>();
			}

			if (_rect == null)
			{
				_rect = GetComponent<RectTransform>();
			}

			if (_grid == null || _rect == null)
			{
				return;
			}

			var width = _rect.rect.width;
			if (width < 2f)
			{
				return;
			}

			var cellWidth = Mathf.Floor((width - SidePadding * 2f - Spacing) / 2f);
			if (cellWidth < 1f)
			{
				return;
			}

			_grid.padding = new RectOffset((int) SidePadding, (int) SidePadding, (int) VerticalPadding, (int) VerticalPadding);
			_grid.spacing = new Vector2(Spacing, Spacing);
			_grid.cellSize = new Vector2(cellWidth, Mathf.Round(cellWidth * CellAspect));
		}
	}
}
