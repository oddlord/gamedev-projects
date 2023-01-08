using System;

namespace SpaceMiner
{
    public interface IObstacleManager
    {
        public Action<Obstacle> OnObstacleDestroyed { get; set; }
        public Action OnAllObstaclesDestroyed { get; set; }
    }
}