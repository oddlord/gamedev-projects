using System;
using UnityEngine;

namespace SpaceMiner
{
    public interface IObstacleSpawner
    {
        public Action<IObstacle> OnObstacleSpawned { get; set; }

        public IObstacle[] SpawnWave(int amount);
        public IObstacle SpawnObstacle(IObstacle obstaclePrefab, Vector3 spawnPosition);
    }
}