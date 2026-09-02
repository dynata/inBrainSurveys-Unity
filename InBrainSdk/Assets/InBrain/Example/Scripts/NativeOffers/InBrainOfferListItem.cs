using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace InBrain
{
	public class InBrainOfferListItem : MonoBehaviour
	{
		static readonly Color PointsColor = new Color(0.278f, 0.639f, 0.863f);
		static readonly Color StruckColor = new Color(0.706f, 0.706f, 0.706f);
		static readonly Color CategoryColor = new Color(0.573f, 0.816f, 0.314f);
		static readonly Color ButtonColor = new Color(0.573f, 0.816f, 0.314f);

		Transform _body;
		RawImage _thumbnail;
		Text _titleText;
		Text _originalPointsText;
		Text _pointsText;
		Text _categoryText;
		Button _startButton;

		int _offerId;

		public void Init(InBrainNativeOffer data)
		{
			EnsureCard();

			_offerId = data.id;
			_titleText.text = data.title ?? string.Empty;

			var hasPromotion = data.promotion != null && data.promotion.multiplier > 1;
			var displayPoints = hasPromotion ? data.reward * data.promotion.multiplier : data.reward;
			_pointsText.text = string.Format("{0} points", Mathf.RoundToInt(displayPoints));

			if (hasPromotion)
			{
				_originalPointsText.gameObject.SetActive(true);
				_originalPointsText.text = string.Format("<s>{0}</s>", Mathf.RoundToInt(data.reward));
			}
			else
			{
				_originalPointsText.gameObject.SetActive(false);
			}

			if (data.categories != null && data.categories.Count > 0 && !string.IsNullOrEmpty(data.categories[0]))
			{
				_categoryText.gameObject.SetActive(true);
				_categoryText.text = data.categories[0];
			}
			else
			{
				_categoryText.gameObject.SetActive(false);
			}

			StopAllCoroutines();
			var thumbnailContainer = _thumbnail.transform.parent.gameObject;
			if (!string.IsNullOrEmpty(data.thumbnailUrl))
			{
				thumbnailContainer.SetActive(true);
				StartCoroutine(LoadThumbnail(data.thumbnailUrl));
			}
			else
			{
				thumbnailContainer.SetActive(false);
			}
		}

		public void OnStartOfferButtonClicked()
		{
			if (_startButton != null)
			{
				_startButton.interactable = false;
			}

			InBrain.Instance.OpenOffer(_offerId);
		}

		void OnDisable()
		{
			if (_startButton != null)
			{
				_startButton.interactable = true;
			}
		}

		void EnsureCard()
		{
			if (_titleText != null)
			{
				return;
			}

			CreateShadow();
			_body = CreateBody();

			_thumbnail = CreateThumbnail();
			_titleText = CreateLabel("Title", 26, FontStyle.Bold, InBrainOffersUi.TitleColor, 68);
			_titleText.horizontalOverflow = HorizontalWrapMode.Wrap;
			_titleText.verticalOverflow = VerticalWrapMode.Truncate;

			var pointsRow = CreateRow("PointsRow", 40);
			_originalPointsText = CreateLabel("OriginalPoints", 26, FontStyle.Bold, StruckColor, 40, pointsRow);
			_originalPointsText.supportRichText = true;
			_originalPointsText.gameObject.AddComponent<ContentSizeFitter>().horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
			_pointsText = CreateLabel("Points", 30, FontStyle.Bold, PointsColor, 40, pointsRow);

			_categoryText = CreateLabel("Category", 24, FontStyle.Normal, CategoryColor, 30);
			_startButton = CreateStartButton();
		}

		void CreateShadow()
		{
			var shadow = new GameObject("Shadow", typeof(RectTransform), typeof(Image));
			shadow.transform.SetParent(transform, false);
			var shadowRt = shadow.GetComponent<RectTransform>();
			shadowRt.anchorMin = Vector2.zero;
			shadowRt.anchorMax = Vector2.one;
			shadowRt.offsetMin = new Vector2(-4, -8);
			shadowRt.offsetMax = new Vector2(4, -2);
			var shadowImage = shadow.GetComponent<Image>();
			shadowImage.sprite = InBrainOffersUi.RoundedSprite;
			shadowImage.type = Image.Type.Sliced;
			shadowImage.color = new Color(0f, 0f, 0f, 0.18f);
			shadowImage.raycastTarget = false;
		}

		Transform CreateBody()
		{
			var body = new GameObject("Body", typeof(RectTransform), typeof(Image), typeof(Mask), typeof(VerticalLayoutGroup));
			body.transform.SetParent(transform, false);
			var bodyRt = body.GetComponent<RectTransform>();
			InBrainOffersUi.Stretch(bodyRt);

			var bodyImage = body.GetComponent<Image>();
			bodyImage.sprite = InBrainOffersUi.RoundedSprite;
			bodyImage.type = Image.Type.Sliced;
			bodyImage.color = Color.white;

			var mask = body.GetComponent<Mask>();
			mask.showMaskGraphic = true;

			var layout = body.GetComponent<VerticalLayoutGroup>();
			layout.padding = new RectOffset(0, 0, 0, 16);
			layout.spacing = 6;
			layout.childAlignment = TextAnchor.UpperCenter;
			layout.childControlWidth = true;
			layout.childControlHeight = false;
			layout.childForceExpandWidth = true;
			layout.childForceExpandHeight = false;
			return body.transform;
		}

		RawImage CreateThumbnail()
		{
			var container = new GameObject("Thumbnail", typeof(RectTransform), typeof(Image), typeof(LayoutElement));
			container.transform.SetParent(_body, false);
			var layoutElement = container.GetComponent<LayoutElement>();
			layoutElement.minHeight = 220;
			layoutElement.preferredHeight = 220;
			layoutElement.flexibleWidth = 1;
			var placeholder = container.GetComponent<Image>();
			placeholder.color = Color.white;
			placeholder.raycastTarget = false;

			var imageGo = new GameObject("Image", typeof(RectTransform), typeof(RawImage));
			imageGo.transform.SetParent(container.transform, false);
			var imageRt = imageGo.GetComponent<RectTransform>();
			InBrainOffersUi.Stretch(imageRt);
			var image = imageGo.GetComponent<RawImage>();
			image.color = Color.white;
			image.raycastTarget = false;
			return image;
		}

		static void ApplyCover(RawImage raw)
		{
			if (raw == null || raw.texture == null)
			{
				return;
			}

			var rect = raw.rectTransform.rect;
			if (rect.width <= 1f || rect.height <= 1f)
			{
				return;
			}

			var texAspect = (float) raw.texture.width / raw.texture.height;
			var rectAspect = rect.width / rect.height;
			if (texAspect > rectAspect)
			{
				var width = rectAspect / texAspect;
				raw.uvRect = new Rect((1f - width) * 0.5f, 0f, width, 1f);
			}
			else
			{
				var height = texAspect / rectAspect;
				raw.uvRect = new Rect(0f, (1f - height) * 0.5f, 1f, height);
			}
		}

		Text CreateLabel(string objectName, int fontSize, FontStyle style, Color color, float height, Transform parent = null)
		{
			var go = new GameObject(objectName, typeof(RectTransform), typeof(Text), typeof(LayoutElement));
			go.transform.SetParent(parent != null ? parent : _body, false);
			var layoutElement = go.GetComponent<LayoutElement>();
			layoutElement.minHeight = height;
			layoutElement.preferredHeight = height;
			var text = go.GetComponent<Text>();
			text.font = InBrainOffersUi.Font;
			text.fontSize = fontSize;
			text.fontStyle = style;
			text.color = color;
			text.alignment = TextAnchor.MiddleCenter;
			text.horizontalOverflow = HorizontalWrapMode.Overflow;
			text.raycastTarget = false;
			return text;
		}

		Transform CreateRow(string objectName, float height)
		{
			var go = new GameObject(objectName, typeof(RectTransform), typeof(HorizontalLayoutGroup), typeof(LayoutElement));
			go.transform.SetParent(_body, false);
			var layoutElement = go.GetComponent<LayoutElement>();
			layoutElement.minHeight = height;
			layoutElement.preferredHeight = height;
			var group = go.GetComponent<HorizontalLayoutGroup>();
			group.spacing = 10;
			group.childAlignment = TextAnchor.MiddleCenter;
			group.childControlHeight = true;
			group.childForceExpandWidth = false;
			group.childForceExpandHeight = true;
			return go.transform;
		}

		Button CreateStartButton()
		{
			var pad = new GameObject("ButtonPad", typeof(RectTransform), typeof(HorizontalLayoutGroup), typeof(LayoutElement));
			pad.transform.SetParent(_body, false);
			var padLayout = pad.GetComponent<LayoutElement>();
			padLayout.minHeight = 64;
			padLayout.preferredHeight = 64;
			var padGroup = pad.GetComponent<HorizontalLayoutGroup>();
			padGroup.padding = new RectOffset(20, 20, 0, 0);
			padGroup.childAlignment = TextAnchor.MiddleCenter;
			padGroup.childControlWidth = true;
			padGroup.childControlHeight = true;
			padGroup.childForceExpandWidth = true;
			padGroup.childForceExpandHeight = true;

			var go = new GameObject("StartOfferButton", typeof(RectTransform), typeof(Image), typeof(Button));
			go.transform.SetParent(pad.transform, false);
			var image = go.GetComponent<Image>();
			image.color = ButtonColor;
			image.sprite = InBrainOffersUi.RoundedSprite;
			image.type = Image.Type.Sliced;
			var button = go.GetComponent<Button>();
			button.targetGraphic = image;
			button.onClick.AddListener(OnStartOfferButtonClicked);

			var label = InBrainOffersUi.CreateText(go.transform, "Text", "Start Offer", 26, FontStyle.Bold, Color.white, TextAnchor.MiddleCenter);
			var labelRt = label.GetComponent<RectTransform>();
			InBrainOffersUi.Stretch(labelRt);

			return button;
		}

		IEnumerator LoadThumbnail(string url)
		{
			using (var request = UnityWebRequestTexture.GetTexture(url))
			{
				yield return request.SendWebRequest();
				if (request.result == UnityWebRequest.Result.Success)
				{
					_thumbnail.texture = DownloadHandlerTexture.GetContent(request);
					_thumbnail.color = Color.white;
					yield return null;
					ApplyCover(_thumbnail);
				}
			}
		}
	}
}
