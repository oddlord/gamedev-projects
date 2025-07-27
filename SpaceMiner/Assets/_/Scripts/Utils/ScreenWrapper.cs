using UnityEngine;

namespace SpaceMiner
{
    public class ScreenWrapper : MonoBehaviour
    {
        private Camera _camera;

        void Awake()
        {
            _camera = Camera.main;
        }

        void Update()
        {
            Vector3 viewportPosition = _camera.WorldToViewportPoint(transform.position);
            float x = Mathf.Repeat(viewportPosition.x, 1f);
            float y = Mathf.Repeat(viewportPosition.y, 1f);
            float z = Mathf.Abs(transform.position.z - _camera.transform.position.z);
            transform.position = _camera.ViewportToWorldPoint(new Vector3(x, y, z));
        }
    }
}
