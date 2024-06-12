#if UNITY_EDITOR

using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Framework.Editor
{
    public class FindMissingScriptsWindow : EditorWindow
    {
        [MenuItem("Tools/Made by Luc1f3r/Missing Script Window")]
        private static void Init()
        {
            GetWindow<FindMissingScriptsWindow>("Missing Script Finder").Show();
        }

        public List<GameObject> results = new List<GameObject>();

        private void OnGUI()
        {
            if (GUILayout.Button("Search Project"))
                SearchProject();
            if (GUILayout.Button("Search scene"))
                SearchScene();
            if (GUILayout.Button("Search Selected Objects"))
                SearchSelected();
            if (GUILayout.Button("Remove Selected Objects"))
                RemoveScripts();

            // src: https://answers.unity.com/questions/859554/editorwindow-display-array-dropdown.html
            var so = new SerializedObject(this);
            var resultsProperty = so.FindProperty(nameof(results));
            EditorGUILayout.PropertyField(resultsProperty, true);
            so.ApplyModifiedProperties();
        }

        private void SearchProject()
        {
            results = AssetDatabase.FindAssets("t:Prefab")
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<GameObject>)
                .Where(x => IsMissing(x, true))
                .Distinct()
                .ToList();
        }

        private void SearchScene()
        {
            results = FindObjectsOfType<GameObject>()
                .Where(x => IsMissing(x, true))
                .Distinct()
                .ToList();
        }

        private void SearchSelected()
        {
            results = GetObject()
                .Where(x => IsMissing(x, true))
                .Distinct()
                .ToList();

            return;

            GameObject[] GetObject()
            {
                // Get All child distinct objects
                return Selection.gameObjects
                    .SelectMany(x => x.GetComponentsInChildren<Transform>(true))
                    .Select(x => x.gameObject)
                    .Distinct()
                    .ToArray();
            }
        }

        private void RemoveScripts()
        {
            for (int i = 0; i < results.Count; i++)
            {
                GameObjectUtility.RemoveMonoBehavioursWithMissingScript(results[i]);
            }
        }

        private static bool IsMissing(GameObject go, bool includeChildren)
        {
            var components = includeChildren
                ? go.GetComponentsInChildren<Component>(true)
                : go.GetComponents<Component>();

            return components.Any(x => x == null);
        }
    }
}

#endif