using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace SpaceMiner
{
    public class ObstacleSpawner : MonoBehaviour
    {
        public Action<Obstacle> OnObstacleSpawned;

        private Obstacle.Factory _obstacleFactory;
        private List<Obstacle> _waveObstaclePrefabs;
        private SpawnPointsContainer _spawnPointsContainer;

        [Inject]
        public void Inject(Obstacle.Factory obstacleFactory, List<Obstacle> waveObstaclePrefabs, SpawnPointsContainer spawnPointsContainer)
        {
            _obstacleFactory = obstacleFactory;
            _waveObstaclePrefabs = waveObstaclePrefabs;
            _spawnPointsContainer = spawnPointsContainer;
        }

        public void SpawnWave(int amount)
        {
            SpawnPoint[] spawnPoints = Utils.ShuffleArray(_spawnPointsContainer.SpawnPoints);

            for (int i = 0; i < amount; i++)
            {
                Obstacle obstaclePrefab = Utils.GetRandomListElement(_waveObstaclePrefabs);
                SpawnPoint spawnPoint = spawnPoints[i % spawnPoints.Length];
                SpawnObstacle(obstaclePrefab, spawnPoint.Position);
            }
        }

        public void SpawnObstacle(Obstacle obstaclePrefab, Vector3 spawnPosition)
        {
            Quaternion spawnRotation = Utils.GetRandom2DRotation();
            Obstacle obstacle = _obstacleFactory.Create(obstaclePrefab);
            obstacle.transform.SetPositionAndRotation(spawnPosition, spawnRotation);
            obstacle.transform.SetParent(transform);
            OnObstacleSpawned?.Invoke(obstacle);
        }
    }
}