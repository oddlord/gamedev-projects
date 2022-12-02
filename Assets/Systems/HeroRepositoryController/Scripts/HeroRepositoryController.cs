using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PocketHeroes
{
    public class HeroRepositoryController : MonoBehaviour
    {
        private const int _MIN_HERO_AMOUNT = 3;

        [SerializeField] private GameState _gameState;

        private HeroRepository _repository;
        private string _collectedHeroesJson;

        void Awake()
        {
            DontDestroyOnLoad(gameObject);

            // Swap the repo implementation here to store the Heroes somewhere else
            _repository = new PlayerPrefsHeroRepository();

            List<Hero> heroes = _repository.GetHeroes();

            // Ensures we always start with at least 3 heroes
            if (heroes.Count < _MIN_HERO_AMOUNT)
            {
                for (int i = heroes.Count; i < _MIN_HERO_AMOUNT; i++)
                {
                    Hero hero = HeroGenerator.Generate();
                    heroes.Add(hero);
                }
                UpdateHeroes(heroes);
            }

            _gameState.CollectedHeroes = heroes;
            _collectedHeroesJson = GetCollectedHeroesJson();

            OnLoaded();
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

        private void OnLoaded()
        {
            SceneManager.LoadScene(Scenes.HERO_SELECTION);
        }
    }
}