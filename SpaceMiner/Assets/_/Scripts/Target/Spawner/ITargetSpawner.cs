using System;
using UnityEngine;

namespace SpaceMiner
{
    public interface ITargetSpawner
    {
        public Action<Target> OnTargetSpawned { get; set; }

        public Target[] SpawnWave(int amount);
        public Target SpawnTarget(Target targetPrefab, Vector3 spawnPosition);
    }
}