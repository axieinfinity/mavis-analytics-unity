using UnityEngine;

public class MavisAnalyticsSettings : ScriptableObject
{

    [SerializeField]
    private string _apiKey;

    public string APIKey
    {
        get => _apiKey;
        set => _apiKey = value;
    }

    [SerializeField]
    private string _apiUrl = "https://x.skymavis.com/track";
    public string APIUrl => _apiUrl;


}
