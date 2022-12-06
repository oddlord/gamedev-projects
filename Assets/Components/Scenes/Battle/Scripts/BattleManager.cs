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
            public CharacterUnit[] PlayerUnitPrefabs;
            public CharacterUnit[] AiUnitPrefabs;
        }

        private const int _BATTLES_PER_NEW_HERO = 5;
        private const int _MAX_HEROES = 10;
        private const int _AI_UNITS = 1;

        [SerializeField] private HeroGroupState _collectedHeroes;
        [SerializeField] private HeroGroupState _battleParty;
        [SerializeField] private BattlesFoughtState _battlesFought;
        [SerializeField] private Transform _playerSpawnPointsContainer;
        [SerializeField] private Transform _aiSpawnPointsContainer;
        [SerializeField] private TextMeshProUGUI _resultMessage;
        [SerializeField] private Button _backButton;

        [Header("Config")]
        [SerializeField] private _Config _config;

        private PartyController _playerPartyController;
        private PartyController _aiPartyController;
        private int _turn;

        void Start()
        {
            Initialize();
            StartNextTurn();
        }

        private void Initialize()
        {
            _turn = 0;

            // TODO refactor this into a UnitSpawner
            CharacterUnit[] playerUnits = new CharacterUnit[_battleParty.Heroes.Count];
            SpawnPoint[] playerSpawnPoints = _playerSpawnPointsContainer.GetComponentsInChildren<SpawnPoint>();
            for (int i = 0; i < _battleParty.Heroes.Count; i++)
            {
                CharacterUnit playerUnitPrefab = _config.PlayerUnitPrefabs[UnityEngine.Random.Range(0, _config.PlayerUnitPrefabs.Length)];
                SpawnPoint spawnPoint = playerSpawnPoints[i % playerSpawnPoints.Length];

                Character playerCharacter;
                if (playerUnitPrefab is HeroUnit) playerCharacter = _battleParty.Heroes[i];
                else playerCharacter = MonsterGenerator.Generate();

                CharacterUnit playerUnit = Instantiate(playerUnitPrefab, spawnPoint.Position, Quaternion.identity, transform);
                playerUnit.Initialize(playerCharacter);
                playerUnits[i] = playerUnit;
            }

            CharacterUnit[] aiUnits = new CharacterUnit[_AI_UNITS];
            SpawnPoint[] aiSpawnPoints = _aiSpawnPointsContainer.GetComponentsInChildren<SpawnPoint>();
            for (int i = 0; i < _AI_UNITS; i++)
            {
                CharacterUnit aiUnitPrefab = _config.AiUnitPrefabs[UnityEngine.Random.Range(0, _config.AiUnitPrefabs.Length)];
                SpawnPoint aiSpawnPoint = aiSpawnPoints[i % _AI_UNITS];

                Character aiCharacter;
                if (aiUnitPrefab is HeroUnit) aiCharacter = HeroGenerator.Generate();
                else aiCharacter = MonsterGenerator.Generate();

                CharacterUnit aiUnit = Instantiate(aiUnitPrefab, aiSpawnPoint.Position, Quaternion.identity, transform);
                aiUnit.Initialize(aiCharacter);
                aiUnits[i] = aiUnit;
            }

            _playerPartyController = new PlayerPartyController(playerUnits, aiUnits);
            _playerPartyController.OnTurnFinished += OnTurnFinished;

            _aiPartyController = new AiPartyController(aiUnits, playerUnits);
            _aiPartyController.OnTurnFinished += OnTurnFinished;

            _backButton.onClick.AddListener(OnBackPressed);
        }

        private void OnTurnFinished()
        {
            bool playerWon = !_aiPartyController.HasAliveUnits();
            bool aiWon = !_playerPartyController.HasAliveUnits();
            bool battleOver = playerWon || aiWon;
            if (battleOver)
            {
                _resultMessage.text = playerWon ? "You Won!" : "You Lost :(";

                _playerPartyController.OnBattleOver(playerWon);
                _aiPartyController.OnBattleOver(aiWon);

                _battlesFought.Increment();
                if (_battlesFought.Amount % _BATTLES_PER_NEW_HERO == 0 && _collectedHeroes.Heroes.Count < _MAX_HEROES)
                {
                    Hero newHero = HeroGenerator.Generate();
                    _collectedHeroes.AddHero(newHero);
                }

                _backButton.gameObject.SetActive(true);
                _resultMessage.gameObject.SetActive(true);
            }
            else
            {
                StartNextTurn();
            }
        }

        private void StartNextTurn()
        {
            _turn++;

            if (IsPlayersTurn) _playerPartyController.DoTurn();
            else _aiPartyController.DoTurn();
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