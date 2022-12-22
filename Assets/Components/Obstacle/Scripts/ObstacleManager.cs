using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceMiner
{
    public class ObstacleManager : MonoBehaviour
    {
        public Action OnAllObstaclesDestroyed;

        private List<Obstacle> _obstacles;

        void Awake()
        {
            _obstacles = new List<Obstacle>();

            Obstacle.OnInitialized += OnObstacleInitialized;
        }

        private void OnObstacleInitialized(Obstacle obstacle)
        {
            obstacle.OnDestroyed += OnObstacleDestroyed;
        }

        private void OnObstacleDestroyed(Obstacle obstacle)
        {
            obstacle.OnDestroyed -= OnObstacleDestroyed;
        }

        void OnDestroy()
        {
            foreach (Obstacle obstacle in _obstacles)
                obstacle.OnDestroyed -= OnObstacleDestroyed;
            Obstacle.OnInitialized -= OnObstacleInitialized;
        }
    }
}