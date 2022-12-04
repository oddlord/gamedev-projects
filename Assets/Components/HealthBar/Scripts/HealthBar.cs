using System;
using UnityEngine;

namespace PocketHeroes
{
    public class HealthBar : MonoBehaviour
    {
        [Serializable]
        private struct _Config
        {
            public Transform FilledBar;
        }

        private const float _ANIMATION_SPEED = 2.5f;

        [Header("Config")]
        [SerializeField] private _Config _config;

        private float _percentage
        {
            get => _config.FilledBar.localScale.x;
            set
            {
                Vector3 localScale = _config.FilledBar.localScale;
                localScale.x = value;
                _config.FilledBar.localScale = localScale;
            }
        }

        private float _targetPercentage;

        void Awake()
        {
            SetFill(1);
        }

        void Update()
        {
            float speed = _ANIMATION_SPEED * Time.deltaTime;
            float newPercentage = Mathf.MoveTowards(_percentage, _targetPercentage, speed);
            SetBar(newPercentage);
        }

        public void SetFill(float percentage, bool instantaneous = false)
        {
            SetTarget(percentage);
            if (instantaneous) SetBar(_targetPercentage);
        }

        public void SetFill(float amount, float max, bool instantaneous = false)
        {
            float percentage = amount / max;
            SetFill(percentage, instantaneous);
        }

        private void SetTarget(float percentage)
        {
            _targetPercentage = Mathf.Clamp01(percentage);
        }

        private void SetBar(float percentage)
        {
            _percentage = percentage;
        }
    }
}