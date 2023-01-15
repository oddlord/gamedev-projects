using System;
using UnityEngine;
using Zenject;

namespace SpaceMiner
{
    public abstract class Target : MonoBehaviour
    {
        public class Factory : PlaceholderFactory<UnityEngine.Object, Target> { }

        public int PointsWorth;

        public Hittable Hittable;

        protected virtual void Awake()
        {
            Hittable.OnHit += OnHit;
        }

        public Action<Target> OnDestroyed;

        protected abstract void OnHit();

        protected virtual void OnDestroy()
        {
            Hittable.OnHit -= OnHit;
        }
    }
}
