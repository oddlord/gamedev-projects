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
        [HideInInspector] public int FragmentsToSpawn = 2;

        protected override void OnHit()
        {
            if (SplitOnHit)
            {
                for (int i = 0; i < FragmentsToSpawn; i++)
                {
                    Quaternion spawnRotation = Utils.GetRandom2DRotation();
                    Obstacle obstacle = Instantiate(FragmentPrefab, transform.position, spawnRotation, transform.parent);
                    obstacle.Initialize();
                }
            }

            OnDestroyed?.Invoke(this);
            Destroy(this.gameObject);
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(Asteroid))]
    public class Asteroid_Editor : Editor
    {
        private Asteroid _asteroid;

        private SerializedProperty _splitOnHit;
        private SerializedProperty _fragmentPrefab;
        private SerializedProperty _fragmentsToSpawn;

        void OnEnable()
        {
            _asteroid = (Asteroid)target;

            _splitOnHit = this.serializedObject.FindProperty("SplitOnHit");
            _fragmentPrefab = this.serializedObject.FindProperty("FragmentPrefab");
            _fragmentsToSpawn = this.serializedObject.FindProperty("FragmentsToSpawn");
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            serializedObject.Update();

            EditorGUILayout.LabelField("Asteroid", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_splitOnHit);
            if (_splitOnHit.boolValue)
            {
                EditorGUILayout.PropertyField(_fragmentPrefab);
                EditorGUILayout.PropertyField(_fragmentsToSpawn);
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}
