using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace SpaceMiner
{
    public class ObstacleManager : MonoBehaviour
    {
        public Action<Obstacle> OnObstacleDestroyed;
        public Action OnAllObstaclesDestroyed;

        private List<Obstacle> _obstacles;

        private ObstacleSpawner _obstacleSpawner;

        [Inject]
        public void Inject(ObstacleSpawner obstacleSpawner)
        {
            _obstacleSpawner = obstacleSpawner;
        }

        void Awake()
        {
            _obstacles = new List<Obstacle>();

            _obstacleSpawner.OnObstacleSpawned += OnObstacleSpawned;
        }

        private void OnObstacleSpawned(Obstacle obstacle)
        {
            _obstacles.Add(obstacle);
            obstacle.OnDestroyed += OnObstacleDestroyedHandler;
        }

        private void OnObstacleDestroyedHandler(Obstacle obstacle)
        {
            _obstacles.Remove(obstacle);
            obstacle.OnDestroyed -= OnObstacleDestroyedHandler;

            OnObstacleDestroyed?.Invoke(obstacle);
            if (_obstacles.Count == 0) OnAllObstaclesDestroyed?.Invoke();
        }

        void OnDestroy()
        {
            foreach (Obstacle obstacle in _obstacles)
                obstacle.OnDestroyed -= OnObstacleDestroyedHandler;
            _obstacleSpawner.OnObstacleSpawned -= OnObstacleSpawned;
        }
    }
}