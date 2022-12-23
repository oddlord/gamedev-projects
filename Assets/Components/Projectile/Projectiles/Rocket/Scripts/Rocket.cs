using System;
using UnityEngine;

namespace SpaceMiner
{
    public class Rocket : Projectile
    {
        private const float _MAX_SPEED_MULTIPLIER = 0.001f;

        [Header("Movement Parameters")]
        [SerializeField] private float _maxSpeed = 120;
        [SerializeField] private float _acceleration = 10;

        private float _speed;

        protected override void Awake()
        {
            base.Awake();
            _speed = 0;
        }

        void FixedUpdate()
        {
            if (!_fired) return;

            _speed = Mathf.MoveTowards(_speed, _maxSpeed, _acceleration);
            transform.position += transform.right * _speed * _MAX_SPEED_MULTIPLIER;
        }
    }
}