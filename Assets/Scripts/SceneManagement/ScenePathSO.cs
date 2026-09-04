using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif
[CreateAssetMenu(fileName = "ScenePathSO", menuName = "Custom/ScenePathSO")]
public sealed class ScenePathSO : ScriptableObject
{
    public string Path => _path;
    [SerializeField, HideInInspector] private string _path;
#if UNITY_EDITOR
    [SerializeField] private SceneAsset _sceneAsset;

    private void OnValidate()
    {
        _path = _sceneAsset != null ? AssetDatabase.GetAssetPath(_sceneAsset) : String.Empty;
    }
#endif
}
