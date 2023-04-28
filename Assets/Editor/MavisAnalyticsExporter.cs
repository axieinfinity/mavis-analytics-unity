using UnityEditor;
using UnityEngine;
using System.IO;
using System.Linq;
using MavisAnalytics;

// This script is only available to developers of the MavisAnalytics SDK Devs

public static class MavisAnalyticsExporter
{
    private const string MenuPath = "MavisAnalytics/Export Package";
    private const string AssetsFolder = "Assets";
    private const string MavisAnalyticsFolder = "MavisAnalytics";
    private const string ExportFolderName = "Exports";

    [MenuItem(MenuPath)]
    public static void ExportPackage()
    {
        string versionName = MavisAnalyticsMain.VersionName;
        string packageName = "MavisAnalytics-" + versionName + ".unitypackage";
        string exportFolderPath = Path.Combine(Directory.GetParent(Application.dataPath).FullName, ExportFolderName);

        // Create the "Exports" folder if it doesn't exist.
        if (!Directory.Exists(exportFolderPath))
        {
            Directory.CreateDirectory(exportFolderPath);
        }

        string packagePath = Path.Combine(exportFolderPath, packageName);

        // Get all asset paths in the project and only include the ones in the MavisAnalytics folder.
        string[] allAssetPaths = AssetDatabase.GetAllAssetPaths();
        string[] exportPaths = allAssetPaths
            .Where(path => path.StartsWith(AssetsFolder + "/" + MavisAnalyticsFolder))
            .ToArray();

        AssetDatabase.ExportPackage(exportPaths, packagePath, ExportPackageOptions.Recurse);
        Debug.Log("Package exported as: " + packagePath);
    }
}
