using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace PocketHeroes
{
    public class HeroSelectionManager : MonoBehaviour
    {
        private const int _PARTY_SIZE = 3;

        [SerializeField] private HeroGroupState _collectedHeroes;
        [SerializeField] private HeroGroupState _selectedHeroes;
        [SerializeField] private HeroSelector _heroSelector;
        [SerializeField] private Button _battleButton;

        void Start()
        {
            _selectedHeroes.Clear(false);
            _heroSelector.SetHeroes(_collectedHeroes.Heroes);

            _collectedHeroes.OnChange += OnCollectedHeroesChanged;
            _selectedHeroes.OnChange += OnSelectedHeroesChanged;
            _battleButton.onClick.AddListener(OnBattlePressed);
        }

        public void OnBattlePressed()
        {
            SceneManager.LoadScene(Scenes.BATTLE);
        }

        private void OnCollectedHeroesChanged(HeroGroupState _)
        {
            _heroSelector.SetHeroes(_collectedHeroes.Heroes);
        }

        private void OnSelectedHeroesChanged(HeroGroupState _)
        {
            bool fullParty = _selectedHeroes.Heroes.Count == _PARTY_SIZE;
            _heroSelector.SetSelectionEnabled(!fullParty);
            _battleButton.interactable = fullParty;
        }

        void OnDestroy()
        {
            _collectedHeroes.OnChange -= OnCollectedHeroesChanged;
            _selectedHeroes.OnChange -= OnSelectedHeroesChanged;
            _battleButton.onClick.RemoveListener(OnBattlePressed);
        }
    }
}