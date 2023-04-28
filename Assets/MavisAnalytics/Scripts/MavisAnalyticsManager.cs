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

        protected override void InitManager()
        {
            analyticsDataList = AnalyticsDataList.LoadFromDevice();
        }

        public void StartSession()
        {
            SessionId = Guid.NewGuid().ToString();
            recentRequestTime = float.MinValue;
            IsInitialised = true;
            Debug.Log("MavisAnalytics Session Start");
        }

        public void AddEvent(EventTypes type, object data)
        {
            if (!IsInitialised)
                return;
            var analyticsEvent = new MavisAnalyticsEvent(type, data);
            AddEvent(analyticsEvent);
        }
        public void AddEvent(EventTypes type, string eventName, string refEventName)
        {
            if (!IsInitialised)
                return;
            var analyticsEvent = new MavisAnalyticsEvent(type, new { @event = eventName, @ref = refEventName });
            AddEvent(analyticsEvent);
        }
        public void AddEvent(EventTypes type, object data, string eventName, string refEventName)
        {
            if (!IsInitialised)
                return;
            var analyticsEvent = new MavisAnalyticsEvent(type, data);
            analyticsEvent.MergeData(new { @event = eventName, @ref = refEventName });
            AddEvent(analyticsEvent);
        }
        public void AddEvent(MavisAnalyticsEvent analyticsEvent)
        {
            if (!IsInitialised)
                return;
            analyticsEvent.MergeData(GetCommonFields());
            analyticsDataList.eventsDataList.Add(analyticsEvent);
            Debug.Log(analyticsEvent);
            analyticsDataList.SaveToDevice();
        }

        private static Dictionary<string , object> GetCommonFields()
        {
            var internetType = "unkown";
            if(Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
            {
                internetType = "wifi_or_cable";
            } else if(Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
            {
                internetType = "data_network";
            }

            var commonFields = new Dictionary<string, object>
            {
                {"uuid", Guid.NewGuid().ToString() },
                { "timestamp", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss") },
                { "user_id", Instance.userId },
                { "session_id", Instance.SessionId },
                { "build_version", $"{Instance.BuildVersion}::apk" },
                { "device_name", SystemInfo.deviceModel },
                { "device_id", SystemInfo.deviceUniqueIdentifier },
                { "platform_name", Application.platform.ToString() },
                { "platform_version", SystemInfo.operatingSystem },
                { "internet_type", internetType }
            };
            // Add custom fields
            return commonFields;
        }

         
    }
}

