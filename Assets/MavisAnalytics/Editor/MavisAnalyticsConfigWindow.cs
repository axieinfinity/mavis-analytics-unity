using UnityEditor;
using UnityEngine;

namespace MavisAnalyticsSDK
{
    public class MavisAnalyticsConfigWindow : EditorWindow
    {
        private MavisAnalyticsSettings _settings;
        private bool _showError;
        private const string SettingsPath = "Assets/MavisAnalytics/Resources/MavisAnalyticsSettings.asset";

        [MenuItem("MavisAnalyticsConfig/Mavis Settings")]
        public static void ShowWindow()
        {
            GetWindow<MavisAnalyticsConfigWindow>("Mavis Settings");
        }

        private void OnEnable()
        {
            LoadOrCreateSettings();
        }

        private void LoadOrCreateSettings()
        {
            _settings = AssetDatabase.LoadAssetAtPath<MavisAnalyticsSettings>(SettingsPath);

            if (_settings == null)
            {
                _settings = CreateInstance<MavisAnalyticsSettings>();

                if (!AssetDatabase.IsValidFolder("Assets/MavisAnalytics/Resources"))
                {
                    AssetDatabase.CreateFolder("Assets/MavisAnalytics", "Resources");
                }

                AssetDatabase.CreateAsset(_settings, SettingsPath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Mavis Analytics Settings", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            _settings.APIKey = EditorGUILayout.TextField("API Key", _settings.APIKey);

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.TextField("API URL", _settings.APIUrl);
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.Space();

            if (GUILayout.Button("Sync Settings"))
            {
                if (string.IsNullOrEmpty(_settings.APIKey))
                {
                    _showError = true;
                }
                else
                {
                    _showError = false;
                    MavisAnalytics.SyncSettings(_settings.APIKey,_settings.APIUrl);
                }
            }

            if (_showError)
            {
                EditorGUILayout.HelpBox("Please type in an API key to sync.", MessageType.Error);
            }
        }
    }
}