using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using UnityEngine;


namespace MavisAnalytics
{
    [Serializable]
    public class AnalyticsDataList
    {
        public static string analyticsDataList = "analytics_data.json";
        public string savePath;

        public List<MavisAnalyticsEvent> eventsDataList = new List<MavisAnalyticsEvent>();

        public AnalyticsDataList(string savePath)
        {
            this.savePath = savePath;
            this.eventsDataList = new List<MavisAnalyticsEvent>();
        }

        public void SaveToDevice()
        {
            var path = Path.Combine(Application.persistentDataPath, savePath);
            File.WriteAllText(path, JsonConvert.SerializeObject(this, AnalyticsServer.serializerSettings));
        }

        public static AnalyticsDataList LoadFromDevice ()
        {
            try
            {
                var path = Path.Combine(Application.persistentDataPath, analyticsDataList);
                var json = File.ReadAllText(path);
                var data = JsonConvert.DeserializeObject<AnalyticsDataList>(json, AnalyticsServer.serializerSettings);
                if(data != null && data.eventsDataList != null)
                {
                    return data;
                }
            } catch (Exception) { }

            return new AnalyticsDataList(analyticsDataList);
        }

    }
}
