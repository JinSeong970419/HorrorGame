#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Horror
{
    public class DescriptionBaseSO : ScriptableObject
    {
        [TextArea] public string description;

        [SerializeField, HideInInspector] private string _guid;
        public string Guid => _guid;

#if UNITY_EDITOR
        void OnValidate()
        {
            var path = AssetDatabase.GetAssetPath(this);
            _guid = AssetDatabase.AssetPathToGUID(path);
        }
#endif
    }
}
