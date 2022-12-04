using UnityEngine;

namespace PocketHeroes
{
    public class HeroSaver : MonoBehaviour
    {
        [SerializeField] private HeroRepository _repository;
        [SerializeField] private HeroGroupState _collectedHeroes;

        void Awake()
        {
            DontDestroyOnLoad(gameObject);

            _collectedHeroes.OnChange += OnHeroesChanged;
        }

        private void OnHeroesChanged(HeroGroupState _)
        {
            _repository.SetHeroes(_collectedHeroes.Heroes);
        }

        void OnDestroy()
        {
            _collectedHeroes.OnChange -= OnHeroesChanged;
        }
    }
}