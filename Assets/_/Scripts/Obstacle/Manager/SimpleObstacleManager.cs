using System;
using System.Collections.Generic;
using Zenject;

namespace SpaceMiner
{
    public class SimpleObstacleManager : IObstacleManager
    {
        public Action<IObstacle> OnObstacleDestroyed { get; set; }
        public Action OnAllObstaclesDestroyed { get; set; }

        private List<IObstacle> _obstacles;

        private IObstacleSpawner _obstacleSpawner;

        [Inject]
        public void Init(IObstacleSpawner obstacleSpawner)
        {
            _obstacleSpawner = obstacleSpawner;

            _obstacles = new List<IObstacle>();
            _obstacleSpawner.OnObstacleSpawned += OnObstacleSpawned;
        }

        private void OnObstacleSpawned(IObstacle obstacle)
        {
            _obstacles.Add(obstacle);
            obstacle.OnDestroyed += OnObstacleDestroyedHandler;
        }

        private void OnObstacleDestroyedHandler(IObstacle obstacle)
        {
            _obstacles.Remove(obstacle);
            obstacle.OnDestroyed -= OnObstacleDestroyedHandler;

            OnObstacleDestroyed?.Invoke(obstacle);
            if (_obstacles.Count == 0) OnAllObstaclesDestroyed?.Invoke();
        }
    }
}