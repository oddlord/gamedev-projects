using System;
using UnityEngine;
using Zenject;

namespace SpaceMiner
{
    public abstract class Actor : MonoBehaviour
    {
        public class Factory : PlaceholderFactory<UnityEngine.Object, Actor> { }

        [SerializeField] private int _initialMaxLives = 3;

        [SerializeField] protected ActorState _state;

        public Action<Actor> OnDeath;

        [Inject]
        public void Init(ActorState state)
        {
            _state = state;
        }

        public virtual void Awake()
        {
            _state.Init(_initialMaxLives);
            _state.SetFullLives();

            _state.OnLivesChange += HandleLivesChanged;
        }

        public abstract void HandleForwardInput(float amount);
        public abstract void HandleSideInput(float amount);
        public abstract void Attack();

        public bool IsDead => _state.Lives <= 0;

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
            _state.OnLivesChange -= HandleLivesChanged;
        }
    }
}
