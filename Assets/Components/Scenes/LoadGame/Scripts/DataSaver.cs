using UnityEngine;

namespace PocketHeroes
{
    public class DataSaver : MonoBehaviour
    {
        [Header("Repositories")]
        [SerializeField] private HeroRepository _collectedHeroesRepository;
        [SerializeField] private BattlesFoughtRepository _battlesFoughtRepository;

        [Header("States")]
        [SerializeField] private HeroGroupState _collectedHeroes;
        [SerializeField] private BattlesFoughtState _battlesFought;

        void Awake()
        {
            DontDestroyOnLoad(gameObject);

            _collectedHeroes.OnChange += OnCollectedHeroesChanged;
            _battlesFought.OnChange += OnBattlesFoughtChanged;
        }

        private void OnCollectedHeroesChanged(HeroGroupState state)
        {
            _collectedHeroesRepository.Set(state.Heroes);
        }

        private void OnBattlesFoughtChanged(BattlesFoughtState state)
        {
            _battlesFoughtRepository.Set(state.Amount);
        }

        void OnDestroy()
        {
            _collectedHeroes.OnChange -= OnCollectedHeroesChanged;
            _battlesFought.OnChange -= OnBattlesFoughtChanged;
        }
    }
}