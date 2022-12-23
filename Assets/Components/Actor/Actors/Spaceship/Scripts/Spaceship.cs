using System;
using UnityEngine;

namespace SpaceMiner
{
    public class Spaceship : Actor
    {
        [Serializable]
        private struct _InternalSetup
        {
            public Transform Nozzle;
        }

        private const float _MAX_SPEED_MULTIPLIER = 0.001f;

        [Header("Movement Parameters")]
        [SerializeField] private float _maxSpeed = 100;
        [SerializeField] private float _acceleration = 7.5f;
        [SerializeField] private float _rotationSpeed = 3;

        [Header("Shooting Parameters")]
        [SerializeField] private Projectile _projectilePrefab;
        [Tooltip("Fire rate expressed in shots/sec")]
        [SerializeField] private float _fireRate = 1;

        [Header("Lives States")]
        [SerializeField] private IntState _maxLivesState;
        [SerializeField] private IntState _livesState;

        [Header("__Internal Setup__")]
        [SerializeField] private _InternalSetup _internalSetup;

        private float _speed;
        private DateTime _lastShot;

        void Awake()
        {
            _lastShot = DateTime.MinValue;
        }

        void Start()
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
            amount = Mathf.Max(0, amount);

            float targetSpeed = amount * _maxSpeed;
            if (targetSpeed == _speed) return;

            float acceleration = amount > 0 ? amount * _acceleration : _acceleration;
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
            TimeSpan timeSinceLastShot = DateTime.Now - _lastShot;
            float secondsPerShot = 1f / _fireRate;
            if (timeSinceLastShot.TotalSeconds < secondsPerShot) return;

            Vector3 spawnPosition = _internalSetup.Nozzle.position;
            Projectile projectile = Instantiate(_projectilePrefab, spawnPosition, transform.rotation);
            projectile.Fire();

            _lastShot = DateTime.Now;
        }
    }
}
