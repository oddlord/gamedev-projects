using UnityEngine;

namespace SpaceMiner
{
    public class DestroyOnOutOfBounds : MonoBehaviour
    {
        private Camera _camera;

        void Awake()
        {
            _camera = Camera.main;
        }

        void Update()
        {
            Vector3 viewportPosition = _camera.WorldToViewportPoint(transform.position);
            float viewportX = viewportPosition.x;
            float viewportY = viewportPosition.y;

            bool outsideHorizontalBounds = viewportX < 0 || viewportX > 1;
            bool outsideVerticalBounds = viewportY < 0 || viewportY > 1;
            bool outsideBounds = outsideHorizontalBounds || outsideVerticalBounds;
            if (outsideBounds) Destroy(this.gameObject);
        }
    }
}
