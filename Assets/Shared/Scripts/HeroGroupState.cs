using System;
using System.Collections.Generic;
using UnityEngine;

namespace PocketHeroes
{
    [CreateAssetMenu(menuName = ScriptableObjects.MENU_PREFIX + "HeroGroupState")]
    // TODO find a way to have a class be both a ScriptableObject and Serializable and merge with HeroGroup
    public class HeroGroupState : ScriptableObject
    {
        [SerializeField] private List<Hero> _heroes;
        // TODO make this return an immutable list so that the developer is forced to call Add/Remove
        public List<Hero> Heroes { get => _heroes; private set { _heroes = value; } }

        public Action<HeroGroupState> OnChange;

        public HeroGroupState(List<Hero> heroes)
        {
            Initialize(heroes);
        }

        public HeroGroupState()
        {
            Initialize();
        }

        public void Initialize(List<Hero> heroes = null)
        {
            _heroes = heroes ?? new List<Hero>();
            foreach (Hero hero in _heroes) SubscribeToHero(hero);
        }

        public void AddHero(Hero hero, bool triggerChange = true)
        {
            if (_heroes.Contains(hero)) return;
            _heroes.Add(hero);
            SubscribeToHero(hero);
            if (triggerChange) OnChange?.Invoke(this);
        }

        public void RemoveHero(Hero hero, bool triggerChange = true)
        {
            if (!_heroes.Contains(hero)) return;
            _heroes.Remove(hero);
            UnsubscribeFromHero(hero);
            if (triggerChange) OnChange?.Invoke(this);
        }

        public void Clear(bool triggerChange = true)
        {
            foreach (Hero hero in _heroes.ToArray()) RemoveHero(hero, false);
            if (triggerChange) OnChange?.Invoke(this);
        }

        private void SubscribeToHero(Hero hero)
        {
            hero.OnChange += OnHeroChanged;
        }

        private void UnsubscribeFromHero(Hero hero)
        {
            hero.OnChange -= OnHeroChanged;
        }

        private void OnHeroChanged(Hero hero)
        {
            OnChange?.Invoke(this);
        }

        void OnDestroy()
        {
            foreach (Hero hero in _heroes) UnsubscribeFromHero(hero);
        }
    }
}