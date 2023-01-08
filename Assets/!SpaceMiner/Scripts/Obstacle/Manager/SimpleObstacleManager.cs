using System;
using System.Collections.Generic;
using Zenject;

namespace SpaceMiner
{
    public class SimpleObstacleManager : IObstacleManager
    {
        public Action<Obstacle> OnObstacleDestroyed { get; set; }
        public Action OnAllObstaclesDestroyed { get; set; }

        private List<Obstacle> _obstacles;

        private IObstacleSpawner _obstacleSpawner;

        [Inject]
        public void Init(IObstacleSpawner obstacleSpawner)
        {
            _obstacleSpawner = obstacleSpawner;

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
    }
}