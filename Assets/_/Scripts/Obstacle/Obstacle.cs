using System;
using UnityEngine;
using Zenject;

namespace SpaceMiner
{
    public abstract class Obstacle : MonoBehaviour
    {
        public class Factory : PlaceholderFactory<UnityEngine.Object, Obstacle> { }

        public int PointsWorth;

        public Action<Obstacle> OnDestroyed;

        protected abstract void OnHit();

        void OnTriggerEnter2D(Collider2D other)
        {
            bool hitByProjectile = other.CompareTag(Tags.PROJECTILE);
            bool hitByActor = other.CompareTag(Tags.ACTOR);
            if (hitByProjectile || hitByActor) OnHit();
        }
    }
}
