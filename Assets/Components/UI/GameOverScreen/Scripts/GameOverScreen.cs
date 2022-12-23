using System;
using UnityEngine;
using TMPro;

namespace SpaceMiner
{
    public class GameOverScreen : MonoBehaviour
    {
        [Serializable]
        private struct _InternalSetup
        {
            public TextMeshProUGUI ScoreText;
        }

        [SerializeField] private IntState _scoreState;

        [Header("__Internal Setup__")]
        [SerializeField] private _InternalSetup _internalSetup;

        public Action OnPlayAgain;
        public Action OnBack;

        public void Show()
        {
            // TODO replace with localisation with parameter
            _internalSetup.ScoreText.text = $"Score: {_scoreState.Value}";
            gameObject.SetActive(true);
        }

        public void PlayAgain()
        {
            OnPlayAgain?.Invoke();
        }

        public void Back()
        {
            OnBack?.Invoke();
        }
    }
}