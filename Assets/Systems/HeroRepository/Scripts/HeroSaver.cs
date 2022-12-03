using System.Collections.Generic;
using UnityEngine;

namespace PocketHeroes
{
    public class HeroSaver : MonoBehaviour
    {
        [SerializeField] private HeroRepository _repository;
        [SerializeField] private GameState _gameState;

        private string _collectedHeroesJson;

        void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        void Start()
        {
            _collectedHeroesJson = GetCollectedHeroesJson();
        }

        void Update()
        {
            // TODO use on-changed events instead of polling
            string collectedHeroesJson = GetCollectedHeroesJson();
            if (_collectedHeroesJson != collectedHeroesJson)
            {
                UpdateHeroes(_gameState.CollectedHeroes);
                _collectedHeroesJson = collectedHeroesJson;
            }
        }

        private void UpdateHeroes(List<Hero> heroes)
        {
            _repository.SetHeroes(heroes);
        }

        private string GetCollectedHeroesJson()
        {
            HeroCollection heroCollection = new HeroCollection() { Heroes = _gameState.CollectedHeroes };
            return JsonUtility.ToJson(heroCollection);
        }
    }
}