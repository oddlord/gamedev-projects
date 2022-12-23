using System;
using UnityEngine;

namespace SpaceMiner
{
    public abstract class Obstacle : MonoBehaviour
    {
        // TODO get rid of this with Dependency Injection
        public static Action<Obstacle> OnInitialized;

        public int PointsWorth;

        public Action<Obstacle> OnDestroyed;

        public void Initialize()
        {
            OnInitialized?.Invoke(this);
        }

        protected abstract void OnHit();

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(Tags.PROJECTILE)) OnHit();
        }
    }
}
