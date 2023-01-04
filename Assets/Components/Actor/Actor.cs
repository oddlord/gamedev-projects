using System;
using UnityEngine;
using Zenject;

namespace SpaceMiner
{
    public abstract class Actor : MonoBehaviour
    {
        public class Factory : PlaceholderFactory<UnityEngine.Object, Actor> { }

        private IntState _maxLivesState;
        protected IntState _livesState;

        public Action<Actor> OnDeath;

        [Inject]
        public void Inject(
            [Inject(Id = LevelInjectIds.MAX_LIVES_STATE)] IntState maxLivesState,
            [Inject(Id = LevelInjectIds.LIVES_STATE)] IntState livesState
        )
        {
            _maxLivesState = maxLivesState;
            _livesState = livesState;
        }

        public virtual void Start()
        {
            _livesState.OnChange += HandleLivesChanged;
            _livesState.Set(_maxLivesState);
        }

        public abstract void HandleForwardInput(float amount);
        public abstract void HandleSideInput(float amount);
        public abstract void Attack();

        public bool IsDead => _livesState.Value <= 0;

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
            _livesState.OnChange -= HandleLivesChanged;
        }
    }
}
