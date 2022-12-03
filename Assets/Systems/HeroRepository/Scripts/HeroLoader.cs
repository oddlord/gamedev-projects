using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PocketHeroes
{
    public class HeroLoader : MonoBehaviour
    {
        private const int _MIN_HERO_AMOUNT = 3;

        [SerializeField] private HeroRepository _repository;
        [SerializeField] private GameState _gameState;

        private string _collectedHeroesJson;

        void Awake()
        {
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
        }

        void Start()
        {
            SceneManager.LoadScene(Scenes.HERO_SELECTION);
        }

        private void UpdateHeroes(List<Hero> heroes)
        {
            _repository.SetHeroes(heroes);
        }
    }
}