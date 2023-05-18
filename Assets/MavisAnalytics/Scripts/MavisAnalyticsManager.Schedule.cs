using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MavisAnalyticsSDK
{
    public partial class MavisAnalyticsManager
    {
        private void Start()
        {
            StartCoroutine(CheckTimer());
        }

        private IEnumerator CheckTimer()
        {
            var wait = new WaitForSeconds(1);
            while (true)
            {
                if (IsInitialised)
                {
                    if(Time.realtimeSinceStartup - recentRequestTime > RequestInterval)
                    {
                        recentRequestTime = Time.realtimeSinceStartup;
                        CheckAndSendRequest();
                    }
                }

                yield return wait;
            }
        }

        private void CheckAndSendRequest()
        {
            Debug.Log(analyticsDataList.eventsDataList.Count);
            if (analyticsDataList == null || analyticsDataList.eventsDataList.Count == 0)
                return;
            for (int i = 0; i < analyticsDataList.eventsDataList.Count; i++)
            {
                Debug.Log("AnalyticsEvent #" + i + " Type: " + analyticsDataList.eventsDataList[i].type + " Data: " + analyticsDataList.eventsDataList[i].data);
            }
            SendAnalyticsRequest();
        }

        private void SendAnalyticsRequest(MavisAnalyticsRequest analyticsRequest = null)
        {
            if (!IsInitialised)
                return;
            analyticsRequest ??= createNewRequest();
            if (analyticsRequest == null) return;

            AnalyticsServer.PostAPI(PostUrl, analyticsRequest, (err, data) =>
            {
                if (err != null || string.IsNullOrEmpty(data))
                {
                    var tryCount = 0;
                    if (memoRetryRequests.ContainsKey(analyticsRequest))
                    {
                        tryCount = memoRetryRequests[analyticsRequest];
                    }
                    tryCount++;
                    if (tryCount < RetryAttempts)
                    {
                        memoRetryRequests[analyticsRequest] = tryCount;
                        StartCoroutine(RequestAfter(analyticsRequest, Mathf.Pow(2, tryCount)));
                    }
                    else
                    {
                        FinishJob(analyticsRequest);
                    }
                }
                else
                {
                    FinishJob(analyticsRequest);
                }

            });

            MavisAnalyticsRequest createNewRequest()
            {
                var eventsList = new List<MavisAnalyticsEvent>(analyticsDataList.eventsDataList);
                foreach (var request in memoRetryRequests)
                {
                    foreach (var e in request.Key.events)
                    {
                        eventsList.Remove(e);
                    }
                }
                eventsList = eventsList.Take(EventPerRequest).ToList();
                if (eventsList.Count == 0)
                    return null;
                return new MavisAnalyticsRequest(eventsList);
            }

            void FinishJob(MavisAnalyticsRequest request)
            {
                memoRetryRequests.Remove(request);
                foreach (var e in request.events)
                {
                    analyticsDataList.eventsDataList.Remove(e);
                }
                analyticsDataList.SaveToDevice();
            }
        }

        private IEnumerator RequestAfter(MavisAnalyticsRequest analyticsRequest, float seconds)
        {
            yield return new WaitForSeconds(seconds);
            SendAnalyticsRequest(analyticsRequest);
        }
    }
}

