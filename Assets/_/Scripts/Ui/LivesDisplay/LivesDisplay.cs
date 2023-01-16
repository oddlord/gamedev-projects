using System.Collections.Generic;
using UnityEngine;

namespace SpaceMiner
{
    public class LivesDisplay : MonoBehaviour
    {
        [SerializeField] private LifeToken _lifeTokenPrefab;

        private bool _initialized;
        private List<LifeToken> _tokens;

        private ObservableInt _lives;
        private ObservableInt _maxLives;

        void Awake()
        {
            _initialized = false;
            _tokens = new List<LifeToken>();
        }

        public void Init(ObservableInt lives, ObservableInt maxLives)
        {
            if (_initialized) Unsubscribe();

            _lives = lives;
            _maxLives = maxLives;

            InstantiateTokens(_maxLives.Value);
            SetTokensEnabled(_lives.Value);

            _maxLives.OnChange += OnMaxLivesChanged;
            _lives.OnChange += OnLivesChanged;

            _initialized = true;
        }

        private void OnMaxLivesChanged(int newValue, int delta)
        {
            InstantiateTokens(newValue);
            SetTokensEnabled(_lives.Value);
        }

        private void OnLivesChanged(int newValue, int delta)
        {
            SetTokensEnabled(newValue);
        }

        private void InstantiateTokens(int count)
        {
            ClearTokens();
            for (int i = 0; i < count; i++)
            {
                LifeToken token = Instantiate(_lifeTokenPrefab, transform);
                _tokens.Add(token);
            }
        }

        private void SetTokensEnabled(int enabledCount)
        {
            enabledCount = Mathf.Min(enabledCount, _maxLives.Value);
            for (int i = 0; i < _tokens.Count; i++)
            {
                bool tokenEnabled = i < enabledCount;
                _tokens[i].SetEnabled(tokenEnabled);
            }
        }

        private void ClearTokens()
        {
            foreach (LifeToken token in _tokens)
                Destroy(token.gameObject);
            _tokens.Clear();
        }

        private void Unsubscribe()
        {
            _maxLives.OnChange -= OnMaxLivesChanged;
            _lives.OnChange -= OnLivesChanged;
        }

        void OnDestroy()
        {
            if (_initialized) Unsubscribe();
        }
    }
}