using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MavisAnalytics
{
    public class MavisAnalyticsBehaviour : MonoBehaviour
    {
        private void Awake()
        {
            MavisAnalytics.InitAnalytics();
        }
    }
}

