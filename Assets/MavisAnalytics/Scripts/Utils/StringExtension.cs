using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;
using UnityEngine;
using System.Text.RegularExpressions;

namespace MavisAnalyticsSDK
{
    public static class StringExtension
    {
        public static string ToBase64(this string currentString)
        {
            var toEncodeAsBytes = Encoding.UTF8.GetBytes(currentString);
            return Convert.ToBase64String(toEncodeAsBytes);
        }
       
    }
}

