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

        private ObservableInt _score;

        [Inject]
        public void Init(ObservableInt score)
        {
            _score = score;
        }

        void Awake()
        {
            _score.OnChange += OnScoreChanged;
        }

        void Start()
        {
            SetScoreText(_score.Value);
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
            _score.OnChange -= OnScoreChanged;
        }
    }
}