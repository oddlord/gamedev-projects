using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace SpaceMiner
{
    // TODO this class is too big, split into multiple sub-components
    public class LevelController : MonoBehaviour
    {
        [Serializable]
        private struct _InternalSetup
        {
            public AudioSource AudioSource;
        }

        [Header("Level Parameters")]
        [SerializeField] private int _initialTargetCount = 2;
        [SerializeField] private int _targetIncreasePerWave = 1;

        [Header("Services")]
        [SerializeField] private GameOverScreen _gameOverScreen;
        [SerializeField] private WaveTextController _waveTextController;
        [SerializeField] private ActorSelector _actorSelector;

        [Header("__Internal Setup__")]
        [SerializeField] private _InternalSetup _internalSetup;

        private int _wave;
        private Actor _playerActor;

        private IActorController _actorController;
        private Actor.Factory _actorFactory;
        private ITargetManager _targetManager;
        private ITargetSpawner _targetSpawner;
        private ObservableInt _score;
        private LivesDisplay _livesDisplay;

        [Inject]
        public void Init(
            IActorController actorController, Actor.Factory actorFactory,
            ITargetManager targetManager, ITargetSpawner targetSpawner,
            ObservableInt score, LivesDisplay livesDisplay
        )
        {
            _actorController = actorController;
            _actorFactory = actorFactory;
            _targetManager = targetManager;
            _targetSpawner = targetSpawner;
            _score = score;
            _livesDisplay = livesDisplay;
        }

        void Awake()
        {
            _wave = 0;
            _score.Value = 0;

            _targetManager.OnAllTargetsDestroyed += OnAllTargetsDestroyed;
            _targetManager.OnTargetDestroyed += OnTargetDestroyed;
            _gameOverScreen.OnPlayAgain += OnPlayAgain;
            _gameOverScreen.OnBack += OnBack;
        }

        void Start()
        {
            _actorSelector.Show(OnActorSelected);
        }

        private void OnActorSelected(Actor actorPrefab)
        {
            _playerActor = _actorFactory.Create(actorPrefab);
            _playerActor.transform.SetPositionAndRotation(Vector3.zero, Quaternion.Euler(0, 0, 90));
            _playerActor.Hittable.HitTags = new string[] { Tags.TARGET };
            _playerActor.OnDeath += OnPlayerDeath;

            _actorController.SetActor(_playerActor);
            _livesDisplay.Init(_playerActor.Lives, _playerActor.MaxLives);

            _internalSetup.AudioSource.Play();
            _actorSelector.Hide();
            StartNextWave();
        }

        private void StartNextWave()
        {
            _wave++;
            int targetCount = _initialTargetCount + _targetIncreasePerWave * (_wave - 1);
            _targetSpawner.SpawnWave(targetCount);
            _waveTextController.Show(_wave);
        }

        private void OnAllTargetsDestroyed()
        {
            StartNextWave();
        }

        private void OnTargetDestroyed(Target target)
        {
            _score.Value += target.PointsWorth;
        }

        private void OnPlayAgain()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        private void OnBack()
        {
            SceneManager.LoadScene(Scenes.START_SCREEN);
        }

        private void OnPlayerDeath(Actor actor)
        {
            _gameOverScreen.Show();
        }

        void OnDestroy()
        {
            _targetManager.OnAllTargetsDestroyed -= OnAllTargetsDestroyed;
            _targetManager.OnTargetDestroyed -= OnTargetDestroyed;
            _gameOverScreen.OnPlayAgain -= OnPlayAgain;
            _gameOverScreen.OnBack -= OnBack;
            if (_playerActor != null) _playerActor.OnDeath -= OnPlayerDeath;
        }
    }
}