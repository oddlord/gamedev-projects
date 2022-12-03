using System;
using System.Collections.Generic;
using UnityEngine;

namespace PocketHeroes
{
    public class HeroSelector : MonoBehaviour
    {
        [Serializable]
        private struct _Config
        {
            public RectTransform Grid;
        }

        [SerializeField] private HeroCard _heroCardPrefab;

        [Header("Config")]
        [SerializeField] private _Config _config;

        public void SetHeroes(List<Hero> heroes)
        {
            ClearGrid();

            foreach (Hero hero in heroes)
            {
                HeroCard card = Instantiate(_heroCardPrefab, _config.Grid);
                card.Initialize(hero);
            }
        }

        private void ClearGrid()
        {
            foreach (RectTransform card in _config.Grid) Destroy(card.gameObject);
        }
    }
}