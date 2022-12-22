using UnityEngine;

namespace SpaceMiner
{
    public class Spaceship : Actor
    {
        private const float _MAX_SPEED_MULTIPLIER = 0.001f;

        [Header("Movement Parameters")]
        [SerializeField] private float _maxSpeed = 100;
        [SerializeField] private float _acceleration = 7.5f;
        [SerializeField] private float _rotationSpeed = 3;

        [Header("Lives States")]
        [SerializeField] private IntState _maxLivesState;
        [SerializeField] private IntState _livesState;

        private float _speed;

        void Awake()
        {
            _livesState.Set(_maxLivesState);
        }

        void FixedUpdate()
        {
            Vector3 translation = transform.right * _speed * _MAX_SPEED_MULTIPLIER;
            transform.position += translation;
        }

        public override void HandleForwardInput(float amount)
        {
            float targetSpeed = amount != 0 ? Mathf.Sign(amount) * _maxSpeed : 0;
            float acceleration = amount != 0 ? Mathf.Abs(amount) * _acceleration : _acceleration;
            _speed = Mathf.MoveTowards(_speed, targetSpeed, acceleration);
        }

        public override void HandleSideInput(float amount)
        {
            if (amount == 0) return;

            float zRotation = -amount * _rotationSpeed;
            Quaternion rotation = Quaternion.Euler(0, 0, zRotation);
            transform.rotation *= rotation;
        }

        public override void Attack()
        {
            // TODO
        }
    }
}
