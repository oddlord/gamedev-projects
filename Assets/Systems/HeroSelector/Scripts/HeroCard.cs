using System;
using TMPro;
using UnityEngine;

namespace PocketHeroes
{
    public class HeroCard : MonoBehaviour
    {
        [Serializable]
        private struct _Config
        {
            public TextMeshProUGUI Name;
            public TextMeshProUGUI Level;
        }

        [SerializeField] private _Config _config;

        public void Initialize(Hero hero)
        {
            _config.Name.text = hero.Name;
            // TODO replace with localized string with parameter
            _config.Level.text = $"lv. {hero.Level}";
        }
    }
}