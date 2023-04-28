using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MavisAnalytics
{
    [Serializable]
    public class MavisAnalyticsRequest
    {
        public List<MavisAnalyticsEvent> events;

        public MavisAnalyticsRequest(List<MavisAnalyticsEvent> eventsList)
        {
            events = eventsList;
        }
    }

}