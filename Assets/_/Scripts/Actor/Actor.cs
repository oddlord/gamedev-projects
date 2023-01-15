using System;
using UnityEngine;
using Zenject;

namespace SpaceMiner
{
    public abstract class Actor : MonoBehaviour
    {
        public class Factory : PlaceholderFactory<UnityEngine.Object, Actor> { }

        [Header("Lives Parameters")]
        [SerializeField] private int _initialMaxLives = 3;

        [Header("Misc")]
        public Hittable Hittable;

        public ObservableInt Lives;
        public ObservableInt MaxLives;

        public Action<Actor> OnDeath;

        protected virtual void Awake()
        {
            Lives = new ObservableInt(_initialMaxLives);
            MaxLives = new ObservableInt(_initialMaxLives);

            Hittable.OnHit += OnHit;

            Lives.OnChange += OnLivesChanged;
        }

        public abstract void HandleForwardInput(float amount);
        public abstract void HandleSideInput(float amount);
        public abstract void Attack();

        public abstract Sprite GetSprite();

        public bool IsDead => Lives.Value <= 0;

        public virtual void SetTag(string tag)
        {
            gameObject.tag = tag;
        }

        protected abstract void OnLivesChanged(int newValue, int delta);

        protected abstract void OnHit();

        void OnDestroy()
        {
            Hittable.OnHit -= OnHit;
            Lives.OnChange -= OnLivesChanged;
        }
    }
}
