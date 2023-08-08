using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

class BuildNumberPreprocess : IPreprocessBuildWithReport
{
    public int callbackOrder { get { return 0; } }

    public void OnPreprocessBuild(BuildReport report)
    {
        int buildNumber;
#if UNITY_ANDROID
        buildNumber = PlayerSettings.Android.bundleVersionCode;
#elif UNITY_IOS
        buildNumber = int.Parse(PlayerSettings.iOS.buildNumber);
#else
        buildNumber = int.Parse(PlayerSettings.macOS.buildNumber);
#endif
        string directoryPath = Application.streamingAssetsPath;
        string filePath = directoryPath + "/BuildNumber.txt";

        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        File.WriteAllText(filePath, buildNumber.ToString());
        AssetDatabase.Refresh();
    }
}
