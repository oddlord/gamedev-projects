using System;
using UnityEngine;

namespace SpaceMiner
{
    public interface IObstacleSpawner
    {
        public Action<Obstacle> OnObstacleSpawned { get; set; }

        public Obstacle[] SpawnWave(int amount);
        public Obstacle SpawnObstacle(Obstacle obstaclePrefab, Vector3 spawnPosition);
    }
}