using System;
using UnityEngine;
using System.Collections.Generic;
using SpaceMiner.Localization;
using UnityEngine.Localization.Components;
using Zenject;

namespace SpaceMiner
{
    public class GameOverScreen : MonoBehaviour
    {
        [Serializable]
        private struct _InternalSetup
        {
            public LocalizeStringEvent ScoreLocalizedText;
        }

        [Header("__Internal Setup__")]
        [SerializeField] private _InternalSetup _internalSetup;

        public Action OnPlayAgain;
        public Action OnBack;

        private ObservableInt _score;

        [Inject]
        public void Init(ObservableInt score)
        {
            _score = score;
        }

        public int Score => _score.Value;

        public void Show()
        {
            Utils.RefreshLocalizeStringEvent(_internalSetup.ScoreLocalizedText);
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