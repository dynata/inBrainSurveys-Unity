using UnityEngine;

namespace InBrain
{
    public enum InBrainWallOption
    {
        ALL = 0,
        SURVEYS = 1,
        OFFERS = 2
    }

    public static class InBrainWallOptionExtensions
    {
        public static AndroidJavaObject ToAJO(this InBrainWallOption option)
        {
            if (Application.platform != RuntimePlatform.Android)
                return null;

            using var wallOptionClass = new AndroidJavaClass(Constants.InBrainWallOptionClass);

            using var companion = wallOptionClass.GetStatic<AndroidJavaObject>(Constants.CompanionJavaField);

            return companion.Call<AndroidJavaObject>(Constants.FromRawJavaMethod, (int)option);
        }

        public static InBrainWallOption FromWallOptionAJO(this AndroidJavaObject ajo)
        {
            if (ajo == null) return InBrainWallOption.ALL;

            return (InBrainWallOption)ajo.Call<int>(Constants.GetRawJavaMethod);
        }
    }
}