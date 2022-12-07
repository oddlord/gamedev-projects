using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace PocketHeroes
{
    public class HealthBar : MonoBehaviour
    {
        [Serializable]
        private struct _Config
        {
            public Transform FilledBar;
            public TextMeshProUGUI DeltaText;
        }

        private const float _ANIMATION_SPEED = 1;
        private const float _DELTA_FADE_TIME = 2.5f;

        private static readonly Color _NEGATIVE_DELTA_COLOR = new Color(1f, 0.3333333f, 0.3333333f, 1f);
        private static readonly Color _POSITIVE_DELTA_COLOR = new Color(0.1316454f, 0.6698113f, 0.116901f, 1f);

        [Header("Config")]
        [SerializeField] private _Config _config;

        private float _max = 1;
        private float _amount = 1;

        private float _fillPercentage
        {
            get => _config.FilledBar.localScale.x;
            set
            {
                Vector3 localScale = _config.FilledBar.localScale;
                localScale.x = value;
                _config.FilledBar.localScale = localScale;
            }
        }

        private float _targetFillPercentage => _amount / _max;

        private Coroutine _deltaCoroutine;

        void Update()
        {
            float speed = _ANIMATION_SPEED * Time.deltaTime;
            float newPercentage = Mathf.MoveTowards(_fillPercentage, _targetFillPercentage, speed);
            SetBar(newPercentage);
        }

        public void Initialize(float max)
        {
            _max = max;
            SetFill(max, false);
        }

        public void SetFill(float amount, bool animate = true)
        {
            if (amount == _amount) return;

            float newAmount = Mathf.Clamp(amount, 0, _max);
            float delta = newAmount - _amount;
            _amount = newAmount;

            if (animate)
            {
                if (_deltaCoroutine != null) StopCoroutine(_deltaCoroutine);
                _deltaCoroutine = StartCoroutine(ShowDeltaCoroutine(delta));
            }
            else SetBar(_targetFillPercentage);
        }

        private void SetBar(float percentage)
        {
            _fillPercentage = percentage;
        }

        private IEnumerator ShowDeltaCoroutine(float delta)
        {
            string sign = delta > 0 ? "+" : "";
            Color color = delta > 0 ? _POSITIVE_DELTA_COLOR : _NEGATIVE_DELTA_COLOR;

            string deltaStr = $"{sign}{delta}";
            _config.DeltaText.text = deltaStr;
            _config.DeltaText.color = color;
            SetDeltaAlpha(1);
            SetActiveDelta(true);

            float t = 0;
            while (t <= 1)
            {
                t += Time.deltaTime / _DELTA_FADE_TIME;
                // TODO use some nicer easing here rather than a linear one
                float alpha = Mathf.Lerp(1, 0, t);
                SetDeltaAlpha(alpha);

                yield return null;
            }

            SetActiveDelta(false);
        }

        private void SetActiveDelta(bool active)
        {
            _config.DeltaText.gameObject.SetActive(active);
        }

        private void SetDeltaAlpha(float alpha)
        {
            Color deltaColor = _config.DeltaText.color;
            deltaColor.a = alpha;
            _config.DeltaText.color = deltaColor;
        }
    }
}