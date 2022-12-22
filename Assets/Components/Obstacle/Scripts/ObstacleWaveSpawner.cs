using System.Collections.Generic;
using UnityEngine;

namespace SpaceMiner
{
    public class ObstacleWaveSpawner : MonoBehaviour
    {
        [SerializeField] private List<Obstacle> _obstaclePrefabs;
        [SerializeField] private Transform _spawnPointsContainer;

        public void Spawn(int amount)
        {
            SpawnPoint[] spawnPoints = _spawnPointsContainer.GetComponentsInChildren<SpawnPoint>();
            SpawnPoint[] shuffledSpawnPoints = Utils.ShuffleArray(spawnPoints);

            for (int i = 0; i < amount; i++)
            {
                Obstacle obstaclePrefab = Utils.GetRandomListElement(_obstaclePrefabs);
                SpawnPoint spawnPoint = shuffledSpawnPoints[i % shuffledSpawnPoints.Length];
                Quaternion spawnRotation = Utils.GetRandom2DRotation();

                Obstacle obstacle = Instantiate(obstaclePrefab, spawnPoint.Position, spawnRotation, transform);
                obstacle.Initialize();
            }
        }
    }
}