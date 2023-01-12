using System;
using UnityEngine;
using Zenject;

namespace SpaceMiner
{
    public class SimpleObstacleSpawner : IObstacleSpawner
    {
        public Action<IObstacle> OnObstacleSpawned { get; set; }

        private IObstacle.Factory _obstacleFactory;
        private IObstacle[] _waveObstaclePrefabs;
        private SpawnPointsContainer _spawnPointsContainer;

        [Inject]
        public void Init(IObstacle.Factory obstacleFactory, IObstacle[] waveObstaclePrefabs, SpawnPointsContainer spawnPointsContainer)
        {
            _obstacleFactory = obstacleFactory;
            _waveObstaclePrefabs = waveObstaclePrefabs;
            _spawnPointsContainer = spawnPointsContainer;
        }

        public IObstacle[] SpawnWave(int amount)
        {
            SpawnPoint[] spawnPoints = Utils.ShuffleArray(_spawnPointsContainer.SpawnPoints);

            IObstacle[] obstacles = new IObstacle[amount];
            for (int i = 0; i < amount; i++)
            {
                IObstacle obstaclePrefab = Utils.GetRandomArrayElement(_waveObstaclePrefabs);
                SpawnPoint spawnPoint = spawnPoints[i % spawnPoints.Length];
                IObstacle obstacle = SpawnObstacle(obstaclePrefab, spawnPoint.Position);
                obstacles[i] = obstacle;
            }

            return obstacles;
        }

        public IObstacle SpawnObstacle(IObstacle obstaclePrefab, Vector3 spawnPosition)
        {
            Quaternion spawnRotation = Utils.GetRandom2DRotation();
            IObstacle obstacle = _obstacleFactory.Create(obstaclePrefab.GetGO());
            obstacle.GetGO().transform.SetPositionAndRotation(spawnPosition, spawnRotation);
            OnObstacleSpawned?.Invoke(obstacle);
            return obstacle;
        }
    }
}