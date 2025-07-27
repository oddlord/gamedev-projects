using UnityEngine;

namespace SpaceMiner
{
    public class RandomFloater : MonoBehaviour
    {
        private const float _SPEED_MULTIPLIER = 0.01f;
        private const float _ROTATION_SPEED_MULTIPLIER = 0.5f;

        [SerializeField] private float _speed = 1f;
        [SerializeField] private float _rotationSpeed = 1f;

        private Vector2 _direction;
        private int _rotationDirection;

        void Awake()
        {
            _direction = Random.insideUnitCircle;
            _rotationDirection = Random.Range(0, 2) * 2 - 1;
        }

        void FixedUpdate()
        {
            Vector2 moveStep = _direction * _speed * _SPEED_MULTIPLIER;
            transform.position += (Vector3)moveStep;

            Quaternion rotationStep = Quaternion.Euler(0, 0, _rotationDirection * _rotationSpeed * _ROTATION_SPEED_MULTIPLIER);
            transform.rotation *= rotationStep;
        }
    }
}
