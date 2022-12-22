using UnityEngine;

namespace SpaceMiner
{
    public class Rocket : Projectile
    {
        private const float _MAX_SPEED_MULTIPLIER = 0.001f;

        [SerializeField] private float _maxSpeed = 120;
        [SerializeField] private float _acceleration = 10;

        private bool _fired;
        private float _speed;

        void Awake()
        {
            _fired = false;
            _speed = 0;
        }

        void FixedUpdate()
        {
            if (!_fired) return;

            _speed = Mathf.MoveTowards(_speed, _maxSpeed, _acceleration);
            transform.position += transform.right * _speed * _MAX_SPEED_MULTIPLIER;
        }

        public override void Fire()
        {
            _fired = true;
        }
    }
}