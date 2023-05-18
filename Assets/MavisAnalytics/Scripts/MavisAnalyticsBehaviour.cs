using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MavisAnalyticsSDK
{
    public class MavisAnalyticsBehaviour : MonoBehaviour
    {
        private void Awake()
        {
            MavisAnalytics.InitAnalytics();
        }
        private void Start()
        {
            MavisAnalytics.SetUserId("random_user_test_id");
        }

        public void TrackEv(string eventName)
        {
            MavisAnalytics.TrackEvent(eventName,"test");
        }

        public void StartGame(string eventName)
        {
            Dictionary<string, object> metadata = new Dictionary<string, object>()
            {
                {"level", 1 }
            };
            MavisAnalytics.TrackEvent(eventName, "progression", metadata);
        }
        public void GameEnded(string eventName)
        {
            Dictionary<string, object> metadata = new Dictionary<string, object>()
            {
                {"level", 1 },
                {"won", false },
                { "items" , new List<string>(){"weapon1","weapon2" } }
            };
            MavisAnalytics.TrackEvent(eventName, "progression", metadata);
        }
    }
}

