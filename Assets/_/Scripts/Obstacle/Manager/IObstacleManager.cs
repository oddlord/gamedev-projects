using System;

namespace SpaceMiner
{
    public interface IObstacleManager
    {
        public Action<IObstacle> OnObstacleDestroyed { get; set; }
        public Action OnAllObstaclesDestroyed { get; set; }
    }
}