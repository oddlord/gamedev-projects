using System;
using System.Collections;
using UnityEngine;

namespace SpaceMiner
{
    // TODO make Actor into an interface
    // TODO this class is too big, split into multiple sub-components
    public class Spaceship : Actor
    {
        [Serializable]
        private struct _InternalSetup
        {
            public SpriteRenderer SpriteRenderer;
            public Collider2D Collider;
            public AudioSource AudioSource;
            public ParticleSystem ParticleSystem;
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

        [Header("Invulnerability Parameters")]
        [SerializeField] private float _invulnerabilityDuration = 4;
        [SerializeField] private float _invulnerabilityBlinkDuration = 0.5f;
        [SerializeField] private float _invulnerabilityAlpha = 0.75f;

        [Header("Audio")]
        [SerializeField] private AudioClip _hitSound;
        [SerializeField] private AudioClip _destructionSound;

        [Header("__Internal Setup__")]
        [SerializeField] private _InternalSetup _internalSetup;

        private float _speed;
        private DateTime _lastShot;
        private Coroutine _invulnerabilityCoroutine;

        void Awake()
        {
            _lastShot = DateTime.MinValue;
        }

        void FixedUpdate()
        {
            Vector3 translation = transform.right * _speed * _MAX_SPEED_MULTIPLIER;
            transform.position += translation;
        }

        public override void HandleForwardInput(float amount)
        {
            if (IsDead) amount = 0;

            amount = Mathf.Max(0, amount);
            float targetSpeed = amount * _maxSpeed;
            if (targetSpeed == _speed) return;

            float acceleration = amount > 0 ? amount * _acceleration : _acceleration;
            _speed = Mathf.MoveTowards(_speed, targetSpeed, acceleration);
        }

        public override void HandleSideInput(float amount)
        {
            if (IsDead) return;
            if (amount == 0) return;

            float zRotation = -amount * _rotationSpeed;
            Quaternion rotation = Quaternion.Euler(0, 0, zRotation);
            transform.rotation *= rotation;
        }

        public override void Attack()
        {
            if (IsDead) return;

            TimeSpan timeSinceLastShot = DateTime.Now - _lastShot;
            float secondsPerShot = 1f / _fireRate;
            if (timeSinceLastShot.TotalSeconds < secondsPerShot) return;

            Vector3 spawnPosition = _internalSetup.Nozzle.position;
            Projectile projectile = Instantiate(_projectilePrefab, spawnPosition, transform.rotation);
            projectile.Fire();

            _lastShot = DateTime.Now;
        }

        protected override void OnHit()
        {
            if (IsDead) return;
            PlayAudio(_hitSound);
            _livesState.Subtract(1);
        }

        protected override void OnLivesChanged(int newValue, int delta)
        {
            if (delta > 0) return;

            SetColliderEnabled(false);
            if (!IsDead)
            {
                if (_invulnerabilityCoroutine != null) StopCoroutine(_invulnerabilityCoroutine);
                _invulnerabilityCoroutine = StartCoroutine(InvulnerabilityCoroutine());
            }
            else
            {
                if (_invulnerabilityCoroutine != null) StopCoroutine(_invulnerabilityCoroutine);
                SetSpriteAlpha(1);
                PlayAudio(_destructionSound);
                _internalSetup.ParticleSystem.Play();
            }
        }

        private IEnumerator InvulnerabilityCoroutine()
        {
            float t = 0;
            float blinkT = 0;
            while (t < 1)
            {
                t += Time.deltaTime / _invulnerabilityDuration;
                blinkT += 2 * Mathf.PI * Time.deltaTime / _invulnerabilityBlinkDuration;
                float alpha = Mathf.Cos(blinkT) * (1 - _invulnerabilityAlpha) + _invulnerabilityAlpha;
                SetSpriteAlpha(alpha);
                yield return null;
            }

            SetSpriteAlpha(1);
            SetColliderEnabled(true);
            _invulnerabilityCoroutine = null;
        }

        private void SetColliderEnabled(bool enabled)
        {
            _internalSetup.Collider.enabled = enabled;
        }

        private void SetSpriteAlpha(float alpha)
        {
            Color color = _internalSetup.SpriteRenderer.color;
            color.a = alpha;
            _internalSetup.SpriteRenderer.color = color;
        }

        private void PlayAudio(AudioClip clip)
        {
            _internalSetup.AudioSource.clip = clip;
            _internalSetup.AudioSource.Play();
        }
    }
}
