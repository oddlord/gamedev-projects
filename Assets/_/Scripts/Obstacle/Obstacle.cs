using System;
using UnityEngine;
using Zenject;

namespace SpaceMiner
{
    // TODO make into an interface
    public abstract class Obstacle : MonoBehaviour
    {
        public class Factory : PlaceholderFactory<UnityEngine.Object, Obstacle> { }

        public int PointsWorth;

        public Action<Obstacle> OnDestroyed;

        protected IObstacleSpawner _obstacleSpawner;

        [Inject]
        public void Init(IObstacleSpawner obstacleSpawner)
        {
            _obstacleSpawner = obstacleSpawner;
        }

        protected abstract void OnHit();

        void OnTriggerEnter2D(Collider2D other)
        {
            bool hitByProjectile = other.CompareTag(Tags.PROJECTILE);
            bool hitByActor = other.CompareTag(Tags.ACTOR);
            if (hitByProjectile || hitByActor) OnHit();
        }
    }
}
