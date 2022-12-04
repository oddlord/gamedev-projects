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
            public CharacterTooltip CharacterTooltip;
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
            _config.CharacterTooltip.Initialize(Utils.GetHeroTooltipRows(hero));
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

        void OnDestroy()
        {
            _selectedHeroes.OnChange -= OnSelectedHeroesChanged;
        }
    }
}