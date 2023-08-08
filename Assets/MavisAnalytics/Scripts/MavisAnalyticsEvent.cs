using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MavisAnalyticsSDK
{
    public enum EventTypes {
        unknown,
        identify,
        screen,
        track
    }
    [Serializable]
    public class MavisAnalyticsEvent
    {
        public EventTypes type;
        public JObject data;

        public MavisAnalyticsEvent(EventTypes type, object data)
        {
            this.type = type;
            this.data = JObject.Parse(JsonConvert.SerializeObject(data));

            switch(type)
            {
                case EventTypes.identify:
                case EventTypes.screen:
                    break;
                case EventTypes.track:
                    this.data["action"] ??= this.data["event"];
                    break;
            }
        }

        public void MergeData(object mergeableData)
        {
            var jObject = JObject.Parse(JsonConvert.SerializeObject(data));
            var mergeableJObject = JObject.Parse(JsonConvert.SerializeObject(mergeableData));
            jObject.Merge(mergeableJObject);
            data = jObject;
        }
    }
}

