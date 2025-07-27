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

        public Action<Target> OnDestroyed;

        protected virtual void Awake()
        {
            Hittable.OnHit += OnHit;
        }

        public virtual void SetTag(string tag)
        {
            gameObject.tag = tag;
        }

        protected abstract void OnHit();

        protected virtual void OnDestroy()
        {
            Hittable.OnHit -= OnHit;
        }
    }
}
