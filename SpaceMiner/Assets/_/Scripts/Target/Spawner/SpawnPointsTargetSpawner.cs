using System;
using UnityEngine;
using Zenject;

namespace SpaceMiner
{
    public class SpawnPointsTargetSpawner : ITargetSpawner
    {
        public Action<Target> OnTargetSpawned { get; set; }

        private Target.Factory _targetFactory;
        private Target[] _waveTargetPrefabs;
        private SpawnPointsContainer _spawnPointsContainer;

        [Inject]
        public void Init(Target.Factory targetFactory, Target[] waveTargetPrefabs, SpawnPointsContainer spawnPointsContainer)
        {
            _targetFactory = targetFactory;
            _waveTargetPrefabs = waveTargetPrefabs;
            _spawnPointsContainer = spawnPointsContainer;
        }

        public Target[] SpawnWave(int amount)
        {
            SpawnPoint[] spawnPoints = Utils.ShuffleArray(_spawnPointsContainer.SpawnPoints);

            Target[] targets = new Target[amount];
            for (int i = 0; i < amount; i++)
            {
                Target targetPrefab = Utils.GetRandomArrayElement(_waveTargetPrefabs);
                SpawnPoint spawnPoint = spawnPoints[i % spawnPoints.Length];
                Target target = SpawnTarget(targetPrefab, spawnPoint.Position);
                targets[i] = target;
            }

            return targets;
        }

        public Target SpawnTarget(Target targetPrefab, Vector3 spawnPosition)
        {
            Quaternion spawnRotation = Utils.GetRandom2DRotation();
            Target target = _targetFactory.Create(targetPrefab);
            target.transform.SetPositionAndRotation(spawnPosition, spawnRotation);
            target.SetTag(Tags.TARGET);
            target.Hittable.HitTags = new string[] { Tags.PLAYER };
            OnTargetSpawned?.Invoke(target);
            return target;
        }
    }
}