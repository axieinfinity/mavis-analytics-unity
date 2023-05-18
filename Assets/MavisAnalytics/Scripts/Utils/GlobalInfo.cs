using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MavisAnalyticsSDK
{
    public static class GlobalInfo
    {
        public static bool NetworkAvailable = true;
        static GlobalInfo() { }

        public static int BuildNumber
        {
            get
            {
#if UNITY_EDITOR
#if UNITY_ANDROID
                return UnityEditor.PlayerSettings.Android.bundleVersionCode;
#elif UNITY_IOS
                return int.Parse(UnityEditor.PlayerSettings.iOS.buildNumber);
#else
                return int.Parse(UnityEditor.PlayerSettings.macOS.buildNumber);
#endif
#endif
            }
        }
    }
}
