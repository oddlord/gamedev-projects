using System;
using TMPro;
using UnityEngine;

namespace PocketHeroes
{
    // TODO make a Unit superclass so that the MonsterUnit can extend it too
    // TODO make it take a Controller (input / AI)
    public class HeroUnit : MonoBehaviour
    {
        [Serializable]
        private struct _Config
        {
            public HealthBar HealthBar;
            public TextMeshPro Name;
        }

        [Header("Config")]
        [SerializeField] private _Config _config;

        private Hero _hero;
        private int _currentHealth;

        public void Initialize(Hero hero)
        {
            _hero = hero;

            _config.Name.text = hero.Name;
            SetCurrentHealth(hero.Health);
        }

        private void SetCurrentHealth(int health, bool animate = false)
        {
            _currentHealth = health;
            _config.HealthBar.SetFill(_currentHealth, _hero.Health, !animate);
        }
    }
}