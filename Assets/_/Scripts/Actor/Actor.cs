using System;
using UnityEngine;
using Zenject;

namespace SpaceMiner
{
    // TODO make into an interface
    public abstract class Actor : MonoBehaviour
    {
        public class Factory : PlaceholderFactory<UnityEngine.Object, Actor> { }

        [SerializeField] private int _initialMaxLives = 3;

        public ObservableInt Lives;
        public ObservableInt MaxLives;

        public Action<Actor> OnDeath;

        public virtual void Awake()
        {
            Lives = new ObservableInt(_initialMaxLives);
            MaxLives = new ObservableInt(_initialMaxLives);

            Lives.OnChange += HandleLivesChanged;
        }

        public abstract void HandleForwardInput(float amount);
        public abstract void HandleSideInput(float amount);
        public abstract void Attack();

        public bool IsDead => Lives.Value <= 0;

        protected abstract void OnHit();
        protected abstract void OnLivesChanged(int newValue, int delta);

        private void HandleLivesChanged(int newValue, int delta)
        {
            OnLivesChanged(newValue, delta);
            if (IsDead) OnDeath?.Invoke(this);
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(Tags.OBSTACLE)) OnHit();
        }

        void OnDestroy()
        {
            Lives.OnChange -= HandleLivesChanged;
        }
    }
}
