using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using Object = UnityEngine.Object;


namespace MavisAnalyticsSDK
{
    public static class AnalyticsRunner
    {
        public delegate void Function();
        private static AnalyticsCoroutineRunner _runner;
        private static readonly object lockCall = new();
        private static readonly List<Function> functions = new List<Function>();

        public static AnalyticsCoroutineRunner runner
        {
            get
            {
                if (_runner == null || _runner.gameObject == null)
                {
                    var runnerObject = new GameObject("_AnalyticsRunner");
                    _runner = runnerObject.AddComponent<AnalyticsCoroutineRunner>();
                    _runner.StartCoroutine(MainThreadUpdater());
                    Object.DontDestroyOnLoad(_runner);
                }
                return _runner;
            }
        }

        public static Coroutine StartCoroutine(IEnumerator coroutine, MonoBehaviour target = null)
        {
            if (target != null && target.gameObject.activeInHierarchy)
            {
                return target.StartCoroutine(coroutine);
            }
            return runner.StartCoroutine(coroutine);
        }

        public static void StopCoroutine(Coroutine coroutine, MonoBehaviour target = null)
        {
            if (coroutine != null)
            {
                if (target != null && target.gameObject.activeInHierarchy)
                {
                    target.StopCoroutine(coroutine);
                }
                else runner.StopCoroutine(coroutine);
            }
        }

        public static IEnumerator MainThreadUpdater()
        {
            while(true)
            {
                lock (lockCall)
                {
                    if(functions.Count > 0 )
                    {
                        foreach (var f in functions) f();
                        functions.Clear();
                    }
                }
                yield return new WaitForEndOfFrame();
            }
        }
        public class AnalyticsCoroutineRunner : MonoBehaviour { }
    }


}

