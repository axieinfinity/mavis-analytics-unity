using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MavisAnalytics
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
            Debug.Log("Mavis Analytics settings synced. API key: " + _apiKey);
        }

        public static void TrackEvent(string eventName)
        {
            _analytics.AddEvent(EventTypes.track, eventName, "test");
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
        }


    }
}

