using System;
using UnityEngine;
using UnityEngine.Localization.Components;
using TMPro;
using System.Collections;
using Oddlord.Easing;

namespace SpaceMiner
{
    public class WaveTextController : MonoBehaviour
    {
        [Serializable]
        private struct _InternalSetup
        {
            public TextMeshProUGUI WaveText;
            public LocalizeStringEvent WaveLocalizedText;
        }

        [SerializeField] private float _fadeDuration = 1f;
        [Tooltip("The duration in seconds of the gap between fade-in and fade-out")]
        [SerializeField] private float _fadeGapDuration = 1f;

        [Header("__Internal Setup__")]
        [SerializeField] private _InternalSetup _internalSetup;

        [HideInInspector] public int WaveNumber = 0;

        private Coroutine _fadeCoroutine;

        void Awake()
        {
            SetAlpha(0);
        }

        public void Show(int waveNum)
        {
            WaveNumber = waveNum;
            Utils.RefreshLocalizeStringEvent(_internalSetup.WaveLocalizedText);

            if (_fadeCoroutine != null) StopCoroutine(_fadeCoroutine);
            StartCoroutine(FadeCoroutine());
        }

        private IEnumerator FadeCoroutine()
        {
            SetAlpha(0);

            float t = 0;
            while (t < 1)
            {
                t += Time.deltaTime / _fadeDuration;
                float alpha = Easing.Lerp(0, 1, t, EasingCurve.EaseInCubic);
                SetAlpha(alpha);
                yield return null;
            }
            SetAlpha(1);

            yield return new WaitForSeconds(_fadeGapDuration);

            t = 0;
            while (t < 1)
            {
                t += Time.deltaTime / _fadeDuration;
                float alpha = Easing.Lerp(1, 0, t, EasingCurve.EaseInCubic);
                SetAlpha(alpha);
                yield return null;
            }
            SetAlpha(0);

            _fadeCoroutine = null;
        }

        private void SetAlpha(float alpha)
        {
            Color color = _internalSetup.WaveText.color;
            color.a = alpha;
            _internalSetup.WaveText.color = color;
        }
    }
}