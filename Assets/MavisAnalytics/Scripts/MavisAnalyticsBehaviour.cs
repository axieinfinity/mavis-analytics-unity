using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MavisAnalyticsSDK;

public class MavisAnalyticsBehaviour : MonoBehaviour
{
    private void Awake()
    {
        MavisAnalytics.InitAnalytics();
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        // Make sure to Set the actual User id on Login
        string randUserId = System.Guid.NewGuid().ToString();
        MavisAnalytics.SetUserId(randUserId);
    }
 
}


