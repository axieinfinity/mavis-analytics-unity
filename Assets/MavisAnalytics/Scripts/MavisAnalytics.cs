using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MavisAnalyticsSDK
{
    public static class MavisAnalytics
    {
        static MavisAnalyticsSettings _settings = Resources.Load<MavisAnalyticsSettings>("MavisAnalyticsSettings");
        private static string _apiKey = _settings.APIKey;
        private static string _trackingUrl = _settings.APIUrl;
        static MavisAnalyticsManager _analytics;
        public static void SyncSettings(string apiKey, string trackingUrl)
        {
            _trackingUrl = trackingUrl;
            _apiKey = apiKey;
        }

        public static void TrackEvent(string eventName, string refEventName)
        {
            _analytics.AddEvent(EventTypes.track, eventName, refEventName);
        }
        public static void TrackEvent(string eventName, string refEventName, object metadata)
        {
            var data = new
            {
                action = eventName, action_properties = metadata
            };
            _analytics.AddEvent(EventTypes.track, data ,eventName,refEventName);
        }

        public static string GetApiKey()
        {
            return _apiKey;
        }
        public static string GetTrackinUrl()
        {
            return _trackingUrl;
        }
        public static void InitAnalytics()
        {
            _analytics = MavisAnalyticsManager.Instance;
            // Initialise Session
            _analytics.StartSession();
        }
        public static void SetUserId(string userId)
        {
            _analytics.userId = userId;
            _analytics.AddEvent(EventTypes.identify, "identify","identify");
        }


    }
}

