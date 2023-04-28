using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;


namespace MavisAnalytics
{
    public partial class MavisAnalyticsManager : UnitySingleton<MavisAnalyticsManager>
    {
        public string PostUrl => "";
        public string ApiKey => "";
        public string userId;

        public string BuildVersion => $"{Application.version}-{GlobalInfo.BuildNumber}";

        public int RequestInterval => 5; //TODO : get from settings or Constants
        public int RetryAttempts => 5;  // TODO : get from settings or Constants

        public int EventPerRequest => 20; // TODO : get from settings or Constants

        public string SessionId { get; private set; }
        public bool IsInitialised { get; private set; }

        private static AnalyticsDataList analyticsDataList = new AnalyticsDataList(AnalyticsDataList.analyticsDataList);

        // Scheduling Requests for Batching
        private float recentRequestTime;

        private readonly Dictionary<MavisAnalyticsRequest, int> memoRetryRequests = new Dictionary<MavisAnalyticsRequest, int>();




        public void StartSession()
        {
            SessionId = Guid.NewGuid().ToString();
            recentRequestTime = float.MinValue;
            IsInitialised = true;
            Debug.Log("MavisAnalytics Session Start");
        }

    }
}

