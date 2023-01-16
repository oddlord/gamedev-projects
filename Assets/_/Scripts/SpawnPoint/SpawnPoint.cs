using UnityEngine;

namespace SpaceMiner
{
    public class SpawnPoint : MonoBehaviour
    {
        [SerializeField] private Texture2D _gizmoIcon;

        public Vector3 Position
        {
            get => transform.position;
            private set => transform.position = value;
        }

        void OnDrawGizmos()
        {
            if (_gizmoIcon == null) return;
            Gizmos.DrawIcon(transform.position, _gizmoIcon.name, true);
        }
    }
}