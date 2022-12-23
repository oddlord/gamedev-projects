using System;
using UnityEngine;
using System.Collections.Generic;
using SpaceMiner.Localization;
using UnityEngine.Localization.Components;

namespace SpaceMiner
{
    public class GameOverScreen : MonoBehaviour
    {
        [Serializable]
        private struct _InternalSetup
        {
            public LocalizeStringEvent ScoreLocalizedText;
        }

        [SerializeField] private IntState _scoreState;

        [Header("__Internal Setup__")]
        [SerializeField] private _InternalSetup _internalSetup;

        public Action OnPlayAgain;
        public Action OnBack;

        public void Show()
        {
            Dictionary<string, object> arguments = new Dictionary<string, object>
            {
                { SentencesLocalization.Arguments.SCORE, _scoreState.Value }
            };
            Utils.SetLocalizedString(_internalSetup.ScoreLocalizedText, SentencesLocalization.Keys.FINAL_SCORE, arguments);

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