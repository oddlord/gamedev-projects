using System;
using UnityEngine;
using TMPro;

namespace SpaceMiner
{
    public class ScoreUiController : MonoBehaviour
    {
        [Serializable]
        private struct _InternalSetup
        {
            public TextMeshProUGUI Text;
        }

        [SerializeField] private IntState _scoreState;

        [Header("__Internal Setup__")]
        [SerializeField] private _InternalSetup _internalSetup;

        void Awake()
        {
            _scoreState.OnChange += OnScoreChanged;
        }

        void Start()
        {
            SetScoreText(_scoreState.Value);
        }

        private void OnScoreChanged(int newValue, int delta)
        {
            SetScoreText(newValue);
        }

        private void SetScoreText(int score)
        {
            _internalSetup.Text.text = score.ToString();
        }

        void OnDestroy()
        {
            _scoreState.OnChange -= OnScoreChanged;
        }
    }
}