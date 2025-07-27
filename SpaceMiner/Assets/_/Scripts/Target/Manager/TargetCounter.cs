using System;
using System.Collections.Generic;
using Zenject;

namespace SpaceMiner
{
    public class TargetCounter : ITargetManager
    {
        public Action<Target> OnTargetDestroyed { get; set; }
        public Action OnAllTargetsDestroyed { get; set; }

        private List<Target> _targets;

        private ITargetSpawner _targetSpawner;

        [Inject]
        public void Init(ITargetSpawner targetSpawner)
        {
            _targetSpawner = targetSpawner;

            _targets = new List<Target>();
            _targetSpawner.OnTargetSpawned += OnTargetSpawned;
        }

        private void OnTargetSpawned(Target target)
        {
            _targets.Add(target);
            target.OnDestroyed += OnTargetDestroyedHandler;
        }

        private void OnTargetDestroyedHandler(Target target)
        {
            _targets.Remove(target);
            target.OnDestroyed -= OnTargetDestroyedHandler;

            OnTargetDestroyed?.Invoke(target);
            if (_targets.Count == 0) OnAllTargetsDestroyed?.Invoke();
        }
    }
}