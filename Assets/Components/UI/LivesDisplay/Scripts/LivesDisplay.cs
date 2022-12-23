using System.Collections.Generic;
using UnityEngine;

namespace SpaceMiner
{
    public class LivesDisplay : MonoBehaviour
    {
        [SerializeField] private LifeToken _lifeTokenPrefab;

        [Header("States")]
        [SerializeField] private IntState _maxLivesState;
        [SerializeField] private IntState _livesState;

        private List<LifeToken> _tokens;

        void Awake()
        {
            _tokens = new List<LifeToken>();

            InstantiateTokens(_maxLivesState.Value);

            _maxLivesState.OnChange += OnMaxLivesChanged;
            _livesState.OnChange += OnLivesChanged;
        }

        void Start()
        {
            SetTokensEnabled(_livesState.Value);
        }

        private void OnMaxLivesChanged(int newValue, int delta)
        {
            InstantiateTokens(newValue);
            SetTokensEnabled(_livesState.Value);
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
            enabledCount = Mathf.Min(enabledCount, _maxLivesState.Value);
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

        void OnDestroy()
        {
            _maxLivesState.OnChange -= OnMaxLivesChanged;
            _livesState.OnChange -= OnLivesChanged;
        }
    }
}