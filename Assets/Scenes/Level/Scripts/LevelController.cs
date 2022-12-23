using UnityEngine;

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

        [Header("Player")]
        [SerializeField] private Actor _playerActor;

        private int _wave;

        void Awake()
        {
            _wave = 0;
            _scoreState.Set(0);

            _obstacleManager.OnAllObstaclesDestroyed += OnAllObstaclesDestroyed;
            _obstacleManager.OnObstacleDestroyed += OnObstacleDestroyed;
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
        }

        private void OnAllObstaclesDestroyed()
        {
            StartNextWave();
        }

        private void OnObstacleDestroyed(Obstacle obstacle)
        {
            _scoreState.Add(obstacle.PointsWorth);
        }

        private void OnPlayerDeath(Actor actor)
        {
            Debug.Log("YOU LOST");
            // TODO
        }

        void OnDestroy()
        {
            _obstacleManager.OnAllObstaclesDestroyed -= OnAllObstaclesDestroyed;
            _obstacleManager.OnObstacleDestroyed -= OnObstacleDestroyed;
            _playerActor.OnDeath += OnPlayerDeath;
        }
    }
}