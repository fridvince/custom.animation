using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace fridvince.Game.Common
{
    public class AutoHeader : MonoBehaviour
    {}

#if UNITY_EDITOR
    [CustomEditor(typeof(AutoHeader))]
    public class AutoHeaderEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            GUIStyle titleStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontSize = 24, fontStyle = FontStyle.Bold };
            GUIStyle subTitleStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontSize = 12 };
            GUIStyle meowStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontSize = 12 };

            EditorGUILayout.Space(4);
            EditorGUILayout.LabelField("designed and powered", subTitleStyle);
            EditorGUILayout.Space(-4);
            EditorGUILayout.LabelField("by fridvince", titleStyle);
            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("meow c:", meowStyle);
            EditorGUILayout.Space(4);

            DrawDefaultInspector();
        }
    }
#endif
}
