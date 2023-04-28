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

        public static void SyncSettings(string apiKey, string trackingUrl)
        {
            _trackingUrl = trackingUrl;
            _apiKey = apiKey;
            Debug.Log("Mavis Analytics settings synced. API key: " + _apiKey);
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

            Debug.Log(_apiKey + _trackingUrl);
            // Initialise Session
            MavisAnalyticsManager.Instance.StartSession();
        }


    }
}

