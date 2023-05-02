using System.Collections;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using HttpMethod = System.Net.Http.HttpMethod;


namespace MavisAnalytics
{
    public static class AnalyticsServer
    {
        public static void PostAPI(string url, MavisAnalyticsRequest data, UnityAction<Error, string> cb)
        {
            Request(url, HttpMethod.Post, data, cb);
        }

        private static void Request(string url, HttpMethod method, MavisAnalyticsRequest data, UnityAction<Error, string> callback)
        {
            var request = new UnityWebRequest(url, "POST");

            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", "Basic " + $"{MavisAnalytics.GetApiKey()}:".ToBase64());
            request.method = method.ToString().ToUpper();
            request.downloadHandler = new DownloadHandlerBuffer();
            var json = JsonConvert.SerializeObject(data, serializerSettings);
            var jsonBytes = new UTF8Encoding().GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(jsonBytes);

            AnalyticsRunner.StartCoroutine(SendWebRequest(request, callback));

        }

        private static IEnumerator SendWebRequest(UnityWebRequest wr, UnityAction<Error, string> onComplete)
        {
            yield return wr.SendWebRequest();

            if (wr.responseCode != 200 || !string.IsNullOrEmpty(wr.error))
            {
                onComplete(Error.New(wr.responseCode, wr.error), null);
                Debug.Log("Event Failed to  send");
            } else
            {
                onComplete(null, wr.downloadHandler?.text);
                Debug.Log("Event Successfully sent" + wr.downloadHandler?.text);
            }
        }

        public static readonly JsonSerializerSettings serializerSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            Converters = new List<JsonConverter> {
                new StringEnumConverter()
            }
        };
    }

    public class Error
    {
        public int code;
        public string message;

        public static Error New(long errorCode, string errorMessage)
        {
            var err = new Error
            {
                code = (int)errorCode,
                message = errorMessage
            };

            return err;
        }

        public override string ToString()
        {
            return message;
        }
    }
}

