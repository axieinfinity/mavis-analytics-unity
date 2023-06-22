using System.IO;
using UnityEngine;

public static class GlobalInfo
{
    public static bool NetworkAvailable = true;

    public static int BuildNumber
    {
        get
        {
#if UNITY_EDITOR
            return 0;

#else
            string path = Application.streamingAssetsPath + "/BuildNumber.txt";

            try
            {
                if (File.Exists(path))
                {
                    string buildNumberText = File.ReadAllText(path);
                    return int.Parse(buildNumberText);
                }
                else
                {
                    Debug.LogError("Build number file not found");
                    return 0;
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError("Failed to read build number: " + e.Message);
                return 0;
            }
#endif
        }
    }
}
