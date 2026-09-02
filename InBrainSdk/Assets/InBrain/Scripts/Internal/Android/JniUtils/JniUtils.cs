using System;
using JetBrains.Annotations;
using UnityEngine;

namespace InBrain
{
	[PublicAPI]
	public static class JniUtils
	{
		static AndroidJavaObject _activity;

		public static AndroidJavaObject Activity
		{
			get
			{
				if (_activity == null)
				{
					var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
					_activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
				}

				return _activity;
			}
		}

		public static void RunOnUiThread(Action action)
		{
			Activity.Call("runOnUiThread", new AndroidJavaRunnable(action));
		}

		public static bool IsJavaNull(this AndroidJavaObject ajo)
		{
			return ajo == null || ajo.GetRawObject().ToInt32() == 0;
		}

		public static int ToJavaColor(this Color color)
		{
			using (var c = new AndroidJavaClass("android.graphics.Color"))
			{
				return c.CallStaticInt("argb", color.a, color.r, color.g, color.b);
			}
		}

		public static string FromJavaDate(AndroidJavaObject dateAjo)
		{
			if (dateAjo.IsJavaNull())
			{
				return null;
			}

			var millis = dateAjo.Call<long>("getTime");
			return DateTimeOffset.FromUnixTimeMilliseconds(millis).UtcDateTime.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
		}
	}
}