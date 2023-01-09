using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace SpaceMiner
{
    public class LivesDisplay : MonoBehaviour
    {
        [SerializeField] private LifeToken _lifeTokenPrefab;

        private List<LifeToken> _tokens;

        private ActorState _state;

        [Inject]
        public void Init(ActorState state)
        {
            _state = state;
        }

        void Awake()
        {
            _tokens = new List<LifeToken>();
        }

        void Start()
        {
            InstantiateTokens(_state.MaxLives);
            SetTokensEnabled(_state.Lives);

            _state.OnMaxLivesChange += OnMaxLivesChanged;
            _state.OnLivesChange += OnLivesChanged;
        }

        private void OnMaxLivesChanged(int newValue, int delta)
        {
            InstantiateTokens(newValue);
            SetTokensEnabled(_state.Lives);
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
            enabledCount = Mathf.Min(enabledCount, _state.MaxLives);
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
            _state.OnMaxLivesChange -= OnMaxLivesChanged;
            _state.OnLivesChange -= OnLivesChanged;
        }
    }
}