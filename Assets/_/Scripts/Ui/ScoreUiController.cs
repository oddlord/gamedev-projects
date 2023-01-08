using System;
using UnityEngine;
using TMPro;
using Zenject;

namespace SpaceMiner
{
    public class ScoreUiController : MonoBehaviour
    {
        [Serializable]
        private struct _InternalSetup
        {
            public TextMeshProUGUI Text;
        }

        [Header("__Internal Setup__")]
        [SerializeField] private _InternalSetup _internalSetup;

        private IntState _scoreState;

        [Inject]
        public void Init(
            [Inject(Id = LevelInjectIds.SCORE_STATE)] IntState scoreState
        )
        {
            _scoreState = scoreState;
        }

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