using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MavisAnalytics
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
            MavisAnalytics.TrackEvent(eventName);
        }
    }
}

