using UnityEngine;

namespace SpaceMiner
{
    public class Bullet : Projectile
    {
        private const float _SPEED_MULTIPLIER = 0.001f;

        [SerializeField] private float _speed = 50;

        private bool _fired;

        void Awake()
        {
            _fired = false;
        }

        void FixedUpdate()
        {
            if (!_fired) return;

            transform.position += transform.right * _speed * _SPEED_MULTIPLIER;
        }

        public override void Fire()
        {
            _fired = true;
        }
    }
}