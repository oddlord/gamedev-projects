using System;
using UnityEngine;
using Zenject;

namespace SpaceMiner
{
    public interface IObstacle
    {
        public class Factory : PlaceholderFactory<UnityEngine.Object, IObstacle> { }

        public int PointsWorth { get; set; }

        public Action<IObstacle> OnDestroyed { get; set; }

        // Ensures that any implementation is a MonoBehaviour
        public GameObject GetGO();
    }
}
