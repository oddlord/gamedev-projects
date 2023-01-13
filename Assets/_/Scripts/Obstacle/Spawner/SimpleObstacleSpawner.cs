using System;
using UnityEngine;
using Zenject;

namespace SpaceMiner
{
    public class SimpleObstacleSpawner : IObstacleSpawner
    {
        public Action<Obstacle> OnObstacleSpawned { get; set; }

        private Obstacle.Factory _obstacleFactory;
        private Obstacle[] _waveObstaclePrefabs;
        private SpawnPointsContainer _spawnPointsContainer;

        [Inject]
        public void Init(Obstacle.Factory obstacleFactory, Obstacle[] waveObstaclePrefabs, SpawnPointsContainer spawnPointsContainer)
        {
            _obstacleFactory = obstacleFactory;
            _waveObstaclePrefabs = waveObstaclePrefabs;
            _spawnPointsContainer = spawnPointsContainer;
        }

        public Obstacle[] SpawnWave(int amount)
        {
            SpawnPoint[] spawnPoints = Utils.ShuffleArray(_spawnPointsContainer.SpawnPoints);

            Obstacle[] obstacles = new Obstacle[amount];
            for (int i = 0; i < amount; i++)
            {
                Obstacle obstaclePrefab = Utils.GetRandomArrayElement(_waveObstaclePrefabs);
                SpawnPoint spawnPoint = spawnPoints[i % spawnPoints.Length];
                Obstacle obstacle = SpawnObstacle(obstaclePrefab, spawnPoint.Position);
                obstacles[i] = obstacle;
            }

            return obstacles;
        }

        public Obstacle SpawnObstacle(Obstacle obstaclePrefab, Vector3 spawnPosition)
        {
            Quaternion spawnRotation = Utils.GetRandom2DRotation();
            Obstacle obstacle = _obstacleFactory.Create(obstaclePrefab);
            obstacle.transform.SetPositionAndRotation(spawnPosition, spawnRotation);
            OnObstacleSpawned?.Invoke(obstacle);
            return obstacle;
        }
    }
}