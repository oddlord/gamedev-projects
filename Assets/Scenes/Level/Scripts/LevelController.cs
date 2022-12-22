using UnityEngine;

namespace SpaceMiner
{
    public class LevelController : MonoBehaviour
    {
        [Header("Level Parameters")]
        [SerializeField] private int _initialObstacleCount = 4;
        [SerializeField] private int _obstacleCountIncreasePerWave = 1;

        [Header("Services")]
        [SerializeField] private ObstacleManager _obstacleManager;
        [SerializeField] private ObstacleWaveSpawner _obstacleWaveSpawner;

        private int _wave;

        void Awake()
        {
            _wave = 0;

            _obstacleManager.OnAllObstaclesDestroyed += OnAllObstaclesDestroyed;
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
            // TODO
        }

        void OnDestroy()
        {
            _obstacleManager.OnAllObstaclesDestroyed -= OnAllObstaclesDestroyed;
        }
    }
}