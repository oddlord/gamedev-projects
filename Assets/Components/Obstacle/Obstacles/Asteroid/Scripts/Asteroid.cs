using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SpaceMiner
{
    public class Asteroid : Obstacle
    {
        [Header("Asteroid")]
        [HideInInspector] public bool SplitOnHit;
        [HideInInspector] public Obstacle FragmentPrefab;
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(Asteroid))]
    public class Asteroid_Editor : Editor
    {
        private Asteroid _asteroid;

        private SerializedProperty _splitOnHit;
        private SerializedProperty _fragmentPrefab;

        void OnEnable()
        {
            _asteroid = (Asteroid)target;

            _splitOnHit = this.serializedObject.FindProperty("SplitOnHit");
            _fragmentPrefab = this.serializedObject.FindProperty("FragmentPrefab");
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            serializedObject.Update();

            EditorGUILayout.LabelField("Asteroid", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_splitOnHit);
            if (_splitOnHit.boolValue)
                EditorGUILayout.PropertyField(_fragmentPrefab);

            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}
