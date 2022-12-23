using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpaceMiner
{
    public class LevelController : MonoBehaviour
    {
        [Header("Level Parameters")]
        [SerializeField] private int _initialObstacleCount = 2;
        [SerializeField] private int _obstacleCountIncreasePerWave = 1;

        [Header("States")]
        [SerializeField] private IntState _scoreState;

        [Header("Services")]
        [SerializeField] private ObstacleManager _obstacleManager;
        [SerializeField] private ObstacleWaveSpawner _obstacleWaveSpawner;
        [SerializeField] private GameOverScreen _gameOverScreen;
        [SerializeField] private WaveTextController _waveTextController;

        [Header("Player")]
        [SerializeField] private Actor _playerActor;

        private int _wave;

        void Awake()
        {
            _wave = 0;
            _scoreState.Set(0);

            _obstacleManager.OnAllObstaclesDestroyed += OnAllObstaclesDestroyed;
            _obstacleManager.OnObstacleDestroyed += OnObstacleDestroyed;
            _gameOverScreen.OnPlayAgain += OnPlayAgain;
            _gameOverScreen.OnBack += OnBack;
            _playerActor.OnDeath += OnPlayerDeath;
        }

        void Start()
        {
            StartNextWave();
        }

        private void StartNextWave()
        {
            _wave++;
            int obstaclesCount = _initialObstacleCount + _obstacleCountIncreasePerWave * (_wave - 1);
            _obstacleWaveSpawner.Spawn(obstaclesCount);
            _waveTextController.Show(_wave);
        }

        private void OnAllObstaclesDestroyed()
        {
            StartNextWave();
        }

        private void OnObstacleDestroyed(Obstacle obstacle)
        {
            _scoreState.Add(obstacle.PointsWorth);
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
            _obstacleManager.OnAllObstaclesDestroyed -= OnAllObstaclesDestroyed;
            _obstacleManager.OnObstacleDestroyed -= OnObstacleDestroyed;
            _gameOverScreen.OnPlayAgain -= OnPlayAgain;
            _gameOverScreen.OnBack -= OnBack;
            _playerActor.OnDeath += OnPlayerDeath;
        }
    }
}