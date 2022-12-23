using UnityEngine;

namespace SpaceMiner
{
    public class Bullet : Projectile
    {
        private const float _SPEED_MULTIPLIER = 0.001f;

        [Header("Movement Parameters")]
        [SerializeField] private float _speed = 50;

        void FixedUpdate()
        {
            if (!_fired) return;

            transform.position += transform.right * _speed * _SPEED_MULTIPLIER;
        }
    }
}