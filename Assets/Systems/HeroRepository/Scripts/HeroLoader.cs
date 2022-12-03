using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PocketHeroes
{
    public class HeroLoader : MonoBehaviour
    {
        private const int _MIN_HERO_AMOUNT = 3;

        [SerializeField] private HeroRepository _repository;
        [SerializeField] private HeroGroupState _collectedHeroes;

        void Awake()
        {
            List<Hero> heroes = _repository.GetHeroes();
            _collectedHeroes.Initialize(heroes);
        }

        void Start()
        {
            // Ensures we always start with at least _MIN_HERO_AMOUNT heroes
            int collectedHeroesCount = _collectedHeroes.Heroes.Count;
            if (collectedHeroesCount < _MIN_HERO_AMOUNT)
            {
                for (int i = collectedHeroesCount; i < _MIN_HERO_AMOUNT; i++)
                {
                    Hero hero = HeroGenerator.Generate();
                    _collectedHeroes.AddHero(hero);
                }
            }

            OnLoaded();
        }

        private void OnLoaded()
        {
            SceneManager.LoadScene(Scenes.HERO_SELECTION);
        }
    }
}