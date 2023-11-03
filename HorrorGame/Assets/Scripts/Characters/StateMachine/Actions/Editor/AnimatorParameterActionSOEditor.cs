using UnityEngine;
using UnityEditor;

namespace Horror
{
    [CustomEditor(typeof(AnimatorParameterActionSO)), CanEditMultipleObjects]
    public class AnimatorParameterActionSOEditor : CustomBaseEditor
    {
        public override void OnInspectorGUI()
        {
            base.DrawNonEditableScriptReference<AnimatorParameterActionSO>();

            serializedObject.Update();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("description"));
            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("whenToRun"));
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Animator Parameter", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("parameterName"), new GUIContent("Name"));

            SerializedProperty animParamValue = serializedObject.FindProperty("parameterType");

            EditorGUILayout.PropertyField(animParamValue, new GUIContent("Type"));

            switch (animParamValue.intValue)
            {
                case (int)AnimatorParameterActionSO.ParameterType.Bool:
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("boolValue"), new GUIContent("Desired value"));
                    break;
                case (int)AnimatorParameterActionSO.ParameterType.Int:
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("intValue"), new GUIContent("Desired value"));
                    break;
                case (int)AnimatorParameterActionSO.ParameterType.Float:
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("floatValue"), new GUIContent("Desired value"));
                    break;

            }

            serializedObject.ApplyModifiedProperties();
        }
    }

    public class CustomBaseEditor : Editor
    {
        public void DrawNonEditableScriptReference<T>() where T : Object
        {
            GUI.enabled = false;

            if (typeof(ScriptableObject).IsAssignableFrom(typeof(T)))
                EditorGUILayout.ObjectField("Script", MonoScript.FromScriptableObject((ScriptableObject)target), typeof(T), false);
            else if (typeof(MonoBehaviour).IsAssignableFrom(typeof(T)))
                EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour((MonoBehaviour)target), typeof(T), false);

            GUI.enabled = true;
        }
    }

}
