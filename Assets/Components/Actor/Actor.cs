using System;
using UnityEngine;

namespace SpaceMiner
{
    public abstract class Actor : MonoBehaviour
    {
        [Header("Lives States")]
        [SerializeField] private IntState _maxLivesState;
        [SerializeField] protected IntState _livesState;

        public Action<Actor> OnDeath;

        protected virtual void Awake()
        {
            _livesState.OnChange += HandleLivesChanged;
        }

        void Start()
        {
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
