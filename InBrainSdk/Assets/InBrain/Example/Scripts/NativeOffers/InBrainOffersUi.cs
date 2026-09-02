using UnityEngine;
using UnityEngine.UI;

namespace InBrain
{
	public static class InBrainOffersUi
	{
		public static readonly Color ScreenBackground = new Color(0.933f, 0.933f, 0.933f);
		public static readonly Color HeaderBackground = new Color(0.62352943f, 0.8117647f, 0.38431373f);
		public static readonly Color TitleColor = new Color(0.2f, 0.2f, 0.2f);

		public static Font Font
		{
			get
			{
				if (_font == null)
				{
					_font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
					if (_font == null)
					{
						_font = Resources.GetBuiltinResource<Font>("Arial.ttf");
					}
				}

				return _font;
			}
		}

		public static Sprite RoundedSprite => FindSprite("Circle64x64");

		public static Sprite BackArrowSprite => FindSprite("BackArrow");

		public static Sprite CloudsSprite => FindSprite("Clouds");

		public static Font TitleFont
		{
			get
			{
				if (_titleFont == null)
				{
					_titleFont = FindFont("Product Sans Bold");
					if (_titleFont == null)
					{
						_titleFont = Font;
					}
				}

				return _titleFont;
			}
		}

		static Font _font;
		static Font _titleFont;

		public static Sprite FindSprite(string spriteName)
		{
			var sprites = Resources.FindObjectsOfTypeAll<Sprite>();
			for (var i = 0; i < sprites.Length; i++)
			{
				if (sprites[i] != null && sprites[i].name == spriteName)
				{
					return sprites[i];
				}
			}

			return null;
		}

		public static Font FindFont(string fontName)
		{
			var fonts = Resources.FindObjectsOfTypeAll<Font>();
			for (var i = 0; i < fonts.Length; i++)
			{
				if (fonts[i] != null && fonts[i].name == fontName)
				{
					return fonts[i];
				}
			}

			return null;
		}

		public static void Stretch(RectTransform rect)
		{
			rect.anchorMin = Vector2.zero;
			rect.anchorMax = Vector2.one;
			rect.offsetMin = Vector2.zero;
			rect.offsetMax = Vector2.zero;
			rect.localScale = Vector3.one;
		}

		public static Text CreateText(Transform parent, string name, string value, int fontSize, FontStyle style, Color color, TextAnchor alignment)
		{
			var go = new GameObject(name, typeof(RectTransform), typeof(Text));
			go.transform.SetParent(parent, false);
			var text = go.GetComponent<Text>();
			text.font = Font;
			text.fontSize = fontSize;
			text.fontStyle = style;
			text.color = color;
			text.alignment = alignment;
			text.text = value;
			text.raycastTarget = false;
			return text;
		}
	}
}
