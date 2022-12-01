using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PocketHeroes
{
    public class HeroManager : MonoBehaviour
    {
        private const int _INITIAL_HERO_AMOUNT = 3;

        private HeroRepository _repository;
        private List<Hero> _heroes;

        public void Initialize()
        {
            // Swap the repo implementation here to store the Heroes somewhere else
            _repository = new PlayerPrefsHeroRepository();

            _heroes = _repository.GetHeroes().ToList();

            // Ensures we always start with at least 3 heroes
            for (int i = _heroes.Count; i < _INITIAL_HERO_AMOUNT; i++)
            {
                Hero newHero = GetNewHero();
                AddHero(newHero);
            }

            Debug.Log($"_heroes: [{string.Join(", ", _heroes)}]");
        }

        private Hero GetNewHero() => HeroGenerator.Generate();

        private void AddHero(Hero hero)
        {
            _heroes.Add(hero);
            OnHeroesUpdated();
        }

        private void OnHeroesUpdated()
        {
            _repository.SetHeroes(_heroes);
        }
    }
}
