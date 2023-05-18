using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using HttpMethod = System.Net.Http.HttpMethod;

namespace MavisAnalyticsSDK
{
    public static class AnalyticsServer
    {
        public static void PostAPI(string url, MavisAnalyticsRequest data, UnityAction<Error, string> cb)
        {
            try
            {
                UnityWebRequest request = Request(url, HttpMethod.Post, data, cb);
                AnalyticsRunner.StartCoroutine(SendWebRequest(request, cb));
            }
            catch (Exception ex)
            {
                Debug.LogError("Exception occurred while preparing to send analytics request: " + ex.Message);
                cb(Error.New(-1, ex.Message), null);
            }
        }

        private static UnityWebRequest Request(string url, HttpMethod method, MavisAnalyticsRequest data, UnityAction<Error, string> callback)
        {
            var request = new UnityWebRequest(url, "POST");

            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", "Basic " + $"{MavisAnalytics.GetApiKey()}:".ToBase64());
            request.method = method.ToString().ToUpper();
            request.downloadHandler = new DownloadHandlerBuffer();

            try
            {
                var json = JsonConvert.SerializeObject(data, serializerSettings);
                var jsonBytes = new UTF8Encoding().GetBytes(json);
                request.uploadHandler = new UploadHandlerRaw(jsonBytes);
            }
            catch (Exception ex)
            {
                Debug.LogError("Exception occurred while serializing analytics request: " + ex.Message);
                callback(Error.New(-1, ex.Message), null);
            }

            return request;
        }

        private static IEnumerator SendWebRequest(UnityWebRequest request, UnityAction<Error, string> onComplete)
        {
            using (request)
            {
                yield return request.SendWebRequest();

                if (request.responseCode != 200 || !string.IsNullOrEmpty(request.error))
                {
                    onComplete(Error.New(request.responseCode, request.error), null);
                    Debug.Log("Event Failed to  send");
                }
                else
                {
                    onComplete(null, request.downloadHandler?.text);
                    Debug.Log("Event Successfully sent" + request.downloadHandler?.text);
                }
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