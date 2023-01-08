using System;
using System.Collections;
using Oddlord.Easing;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMiner
{
    public class LifeToken : MonoBehaviour
    {
        private static readonly Color _ENABLED_COLOR = Color.white;

        [Serializable]
        private struct _InternalSetup
        {
            public Image Image;
        }

        [SerializeField] private float _fadeDuration = 1f;
        [SerializeField] private Color _disabledColor = new Color(0.5f, 0.5f, 0.5f);

        [Header("__Internal Setup__")]
        [SerializeField] private _InternalSetup _internalSetup;

        private bool _enabled;
        private Coroutine _fadeCoroutine;

        void Awake()
        {
            _enabled = true;
        }

        public void SetEnabled(bool enabledValue)
        {
            if (enabledValue != _enabled)
            {
                Color targetColor = enabledValue ? _ENABLED_COLOR : _disabledColor;
                if (_fadeCoroutine != null) StopCoroutine(_fadeCoroutine);
                _fadeCoroutine = StartCoroutine(FadeCoroutine(targetColor));
            }

            _enabled = enabledValue;
        }

        private IEnumerator FadeCoroutine(Color targetColor)
        {
            float t = 0;
            Color startColor = _internalSetup.Image.color;
            while (t < 1)
            {
                t += Time.deltaTime / _fadeDuration;
                Color color = Easing.Lerp(startColor, targetColor, t, EasingCurve.EaseInOutQuad);
                SetColor(color);
                yield return null;
            }

            SetColor(targetColor);
            _fadeCoroutine = null;
        }

        private void SetColor(Color color)
        {
            _internalSetup.Image.color = color;
        }
    }
}