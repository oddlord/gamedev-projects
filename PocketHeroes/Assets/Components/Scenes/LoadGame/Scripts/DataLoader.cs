using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PocketHeroes
{
    public class DataLoader : MonoBehaviour
    {
        private const int _MIN_HERO_AMOUNT = 3;

        [Header("Repositories")]
        [SerializeField] private CollectedHeroesRepository _collectedHeroesRepository;
        [SerializeField] private BattlesFoughtRepository _battlesFoughtRepository;

        [Header("States")]
        [SerializeField] private HeroGroupState _collectedHeroes;
        [SerializeField] private BattlesFoughtState _battlesFought;

        void Awake()
        {
            List<Hero> heroes = _collectedHeroesRepository.Get();
            _collectedHeroes.Initialize(heroes);

            int battlesFought = _battlesFoughtRepository.Get();
            _battlesFought.Initialize(battlesFought);
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