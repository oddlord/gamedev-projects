using System;
using UnityEngine;

namespace SpaceMiner
{
    public abstract class Obstacle : MonoBehaviour
    {
        // TODO get rid of this with Dependency Injection
        public static Action<Obstacle> OnInitialized;

        [Header("Obstacle")]
        public int PointsWorth;

        public Action<Obstacle> OnDestroyed;

        public void Initialize()
        {
            OnInitialized?.Invoke(this);
        }
    }
}
