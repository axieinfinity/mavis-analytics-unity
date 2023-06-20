using UnityEditor;
using UnityEngine;

namespace MavisAnalyticsSDK
{
    [CustomEditor(typeof(MavisAnalyticsSettings))]
    public class MavisAnalyticsSettingsInspector : Editor
    {
        private bool _showError;
        public override void OnInspectorGUI()
        {

            var settings = (MavisAnalyticsSettings)target;

            EditorGUILayout.LabelField("Mavis Analytics Settings", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            EditorGUI.BeginChangeCheck(); // Add this line

            settings.APIKey = EditorGUILayout.TextField("API Key", settings.APIKey);

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.TextField("API URL", settings.APIUrl);
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.Space();

            if (GUILayout.Button("Sync Settings"))
            {
                if (string.IsNullOrEmpty(settings.APIKey))
                {
                    _showError = true;
                }
                else
                {
                    _showError = false;
                    MavisAnalytics.SyncSettings(settings.APIKey, settings.APIUrl);
                }
            }

            if (_showError)
            {
                EditorGUILayout.HelpBox("Please type in an API key to sync.", MessageType.Error);
            }

            if (EditorGUI.EndChangeCheck()) // Add these lines
            {
                EditorUtility.SetDirty(settings);
                AssetDatabase.SaveAssets();
            }
        }
    }
}
