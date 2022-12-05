using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace PocketHeroes
{
    public class BattleManager : MonoBehaviour
    {
        [Serializable]
        private struct _Config
        {
            public HeroUnit HeroUnitPrefab;
            public MonsterUnit MonsterUnitPrefab;
        }

        private const int _BATTLES_PER_NEW_HERO = 5;
        private const int _MAX_HEROES = 10;

        [SerializeField] private HeroGroupState _collectedHeroes;
        [SerializeField] private HeroGroupState _battleParty;
        [SerializeField] private BattlesFoughtState _battlesFought;
        [SerializeField] private Transform _heroSpawnPointsContainer;
        [SerializeField] private Transform _monsterSpawnPointsContainer;
        [SerializeField] private TextMeshProUGUI _resultMessage;
        [SerializeField] private Button _backButton;

        [Header("Config")]
        [SerializeField] private _Config _config;

        private PlayerController _playerController;
        private AiController _aiController;
        private int _turn;

        void Start()
        {
            Initialize();
            StartNextTurn();
        }

        private void Initialize()
        {
            _turn = 0;

            HeroUnit[] heroUnits = new HeroUnit[_battleParty.Heroes.Count];
            SpawnPoint[] heroSpawnPoints = _heroSpawnPointsContainer.GetComponentsInChildren<SpawnPoint>();
            for (int i = 0; i < _battleParty.Heroes.Count; i++)
            {
                Hero hero = _battleParty.Heroes[i];
                SpawnPoint spawnPoint = heroSpawnPoints[i % heroSpawnPoints.Length];

                HeroUnit heroUnit = Instantiate(_config.HeroUnitPrefab, spawnPoint.Position, Quaternion.identity, transform);
                heroUnit.Initialize(hero);
                heroUnits[i] = heroUnit;
            }

            SpawnPoint[] monsterSpawnPoints = _monsterSpawnPointsContainer.GetComponentsInChildren<SpawnPoint>();
            SpawnPoint monsterSpawnPoint = monsterSpawnPoints[0];
            Monster monster = MonsterGenerator.Generate();
            MonsterUnit monsterUnit = Instantiate(_config.MonsterUnitPrefab, monsterSpawnPoint.Position, Quaternion.identity, transform);
            monsterUnit.Initialize(monster);

            _playerController = new PlayerController(heroUnits, monsterUnit);
            _playerController.OnTurnFinished += StartNextTurn;

            _aiController = new AiController(monsterUnit, heroUnits);
            _aiController.OnTurnFinished += StartNextTurn;

            _backButton.onClick.AddListener(OnBackPressed);
        }

        private void StartNextTurn()
        {
            bool playerWon = !_aiController.HasAliveUnits();
            bool aiWon = !_playerController.HasAliveUnits();
            if (playerWon || aiWon)
            {
                if (playerWon)
                {
                    _resultMessage.text = "You Won!";
                    foreach (HeroUnit unit in _playerController.Units)
                    {
                        if (!unit.IsDead) unit.Hero.GainExperience();
                    }
                }
                else if (aiWon)
                {
                    _resultMessage.text = "You Lost :(";
                }

                _battlesFought.Increment();
                if (_battlesFought.Amount % _BATTLES_PER_NEW_HERO == 0 && _collectedHeroes.Heroes.Count < _MAX_HEROES)
                {
                    Hero newHero = HeroGenerator.Generate();
                    _collectedHeroes.AddHero(newHero);
                }

                _backButton.gameObject.SetActive(true);
                _resultMessage.gameObject.SetActive(true);

                return;
            }

            _turn++;

            if (IsPlayersTurn) _playerController.DoTurn();
            else _aiController.DoTurn();
        }

        private bool IsPlayersTurn => _turn % 2 == 1;

        private void OnBackPressed()
        {
            SceneManager.LoadScene(Scenes.HERO_SELECTION);
        }

        void OnDestroy()
        {
            _backButton.onClick.RemoveListener(OnBackPressed);
        }
    }
}