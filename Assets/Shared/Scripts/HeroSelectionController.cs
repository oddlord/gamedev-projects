using UnityEngine;

namespace PocketHeroes
{
    public class HeroSelectionController : MonoBehaviour
    {
        [SerializeField] private HeroGroupState _collectedHeroes;
        [SerializeField] private HeroSelector _heroSelector;

        void Start()
        {
            _heroSelector.SetHeroes(_collectedHeroes.Heroes);
            _collectedHeroes.OnChange += OnHeroesChanged;
        }

        private void OnHeroesChanged(HeroGroupState _)
        {
            _heroSelector.SetHeroes(_collectedHeroes.Heroes);
        }

        void OnDestroy()
        {
            _collectedHeroes.OnChange -= OnHeroesChanged;
        }
    }
}