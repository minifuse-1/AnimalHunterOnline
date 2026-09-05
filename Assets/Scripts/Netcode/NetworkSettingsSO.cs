using UnityEngine;

[CreateAssetMenu(fileName = "NetworkSettingsSO", menuName = "Custom/NetworkSettingsSO")]
public sealed class NetworkSettingsSO : ScriptableObject
{
    [SerializeField] private string _baseUrl;
    public string BaseURL => _baseUrl.Trim('/');
}
