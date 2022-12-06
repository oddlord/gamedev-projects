using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace PocketHeroes
{
    public class BattleManager : MonoBehaviour
    {
        private const int _BATTLES_PER_NEW_HERO = 5;
        private const int _MAX_HEROES = 10;

        [SerializeField] private HeroGroupState _collectedHeroes;
        [SerializeField] private BattlesFoughtState _battlesFought;
        [SerializeField] private PartyController _p1Controller;
        [SerializeField] private PartyController _p2Controller;
        [SerializeField] private TextMeshProUGUI _resultMessage;
        [SerializeField] private Button _backButton;

        private int _turn;

        void Start()
        {
            _turn = 0;

            _p1Controller.Initialize();
            _p2Controller.Initialize();

            _p1Controller.SetEnemyUnits(_p2Controller.Units);
            _p1Controller.OnTurnFinished += OnTurnFinished;

            _p2Controller.SetEnemyUnits(_p1Controller.Units);
            _p2Controller.OnTurnFinished += OnTurnFinished;

            _backButton.onClick.AddListener(OnBackPressed);

            StartNextTurn();
        }

        private void OnTurnFinished()
        {
            bool p1Won = !_p2Controller.HasAliveUnits();
            bool p2Won = !_p1Controller.HasAliveUnits();
            bool battleOver = p1Won || p2Won;

            if (battleOver)
            {
                PartyController winner = p1Won ? _p1Controller : _p2Controller;

                string resultText;
                if (winner.IsPlayer()) resultText = "You Won!";
                else if (IsAnyPlayerInBattle) resultText = "You Lost :(";
                else resultText = "AI Won";
                _resultMessage.text = resultText;

                _p1Controller.OnBattleOver(p1Won);
                _p2Controller.OnBattleOver(p2Won);

                _battlesFought.Increment();
                if (_battlesFought.Amount % _BATTLES_PER_NEW_HERO == 0 && _collectedHeroes.Heroes.Count < _MAX_HEROES)
                {
                    Hero newHero = HeroGenerator.Generate();
                    _collectedHeroes.AddHero(newHero);
                }

                _backButton.gameObject.SetActive(true);
                _resultMessage.gameObject.SetActive(true);
            }
            else StartNextTurn();
        }

        private void StartNextTurn()
        {
            _turn++;

            if (_turn % 2 == 1) _p1Controller.DoTurn();
            else _p2Controller.DoTurn();
        }

        private void OnBackPressed()
        {
            SceneManager.LoadScene(Scenes.HERO_SELECTION);
        }

        private bool IsAnyPlayerInBattle => _p1Controller.IsPlayer() || _p2Controller.IsPlayer();

        void OnDestroy()
        {
            _backButton.onClick.RemoveListener(OnBackPressed);
        }
    }
}