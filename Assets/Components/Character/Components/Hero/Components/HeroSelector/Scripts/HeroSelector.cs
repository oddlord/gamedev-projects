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
            public Tooltip Tooltip;
        }

        [SerializeField] private HeroGroupState _selectedHeroes;
        [SerializeField] private HeroCard _heroCardPrefab;

        [Header("Config")]
        [SerializeField] private _Config _config;

        private List<HeroCard> _cards;
        private bool _selectionEnabled;

        void Awake()
        {
            _cards = new List<HeroCard>();
            _selectionEnabled = true;

            _selectedHeroes.OnChange += OnSelectedHeroesChanged;
        }

        public void SetHeroes(List<Hero> heroes)
        {
            ClearGrid();

            foreach (Hero hero in heroes)
            {
                HeroCard card = Instantiate(_heroCardPrefab, _config.Grid);
                card.Initialize(hero);
                card.OnPress += OnCardPressed;
                card.OnLongPress += OnCardLongPressed;
                _cards.Add(card);
            }

            SetSelectedCards();
        }

        public void SetSelectionEnabled(bool selectionEnabled)
        {
            _selectionEnabled = selectionEnabled;
        }

        private void ClearGrid()
        {
            foreach (HeroCard card in _cards)
            {
                card.OnPress -= OnCardPressed;
                card.OnLongPress -= OnCardLongPressed;
                Destroy(card.gameObject);
            }
            _cards.Clear();
        }

        private void OnCardPressed(Hero hero)
        {
            if (_selectedHeroes.Heroes.Contains(hero)) _selectedHeroes.RemoveHero(hero);
            else if (_selectionEnabled) _selectedHeroes.AddHero(hero);
        }

        private void OnCardLongPressed(Hero hero)
        {
            _config.Tooltip.Initialize(GetHeroTooltipRows(hero));
        }

        private void OnSelectedHeroesChanged(HeroGroupState _)
        {
            SetSelectedCards();
        }

        private void SetSelectedCards()
        {
            foreach (HeroCard card in _cards)
            {
                bool selected = _selectedHeroes.Heroes.Contains(card.Hero);
                card.SetSelected(selected);
            }
        }

        // TODO this function doesn't belong with either the Hero or the tooltip classes
        // Find the proper place for this function, same for the other functions
        private string[] GetHeroTooltipRows(Hero hero)
        {
            return new string[]{
                $"Name: {hero.Name}",
                $"Health: {hero.Health}",
                $"Attack Power: {hero.AttackPower}",
                $"Level: {hero.Level}",
                $"Experience: {hero.Experience}",
            };
        }

        void OnDestroy()
        {
            _selectedHeroes.OnChange -= OnSelectedHeroesChanged;
        }
    }
}