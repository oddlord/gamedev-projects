using System;
using UnityEngine;

namespace PocketHeroes
{
    public class BattleManager : MonoBehaviour
    {
        [Serializable]
        private struct _Config
        {
            public HeroUnit HeroUnitPrefab;
        }

        [SerializeField] private HeroGroupState _battleParty;
        [SerializeField] private Transform _heroSpawnPointsContainer;
        [SerializeField] private Transform _monsterSpawnPointsContainer;

        [Header("Config")]
        [SerializeField] private _Config _config;

        void Start()
        {
            SpawnPoint[] heroSpawnPoints = _heroSpawnPointsContainer.GetComponentsInChildren<SpawnPoint>();
            for (int i = 0; i < _battleParty.Heroes.Count; i++)
            {
                Hero hero = _battleParty.Heroes[i];
                SpawnPoint spawnPoint = heroSpawnPoints[i % heroSpawnPoints.Length];

                HeroUnit heroUnit = Instantiate(_config.HeroUnitPrefab, spawnPoint.Position, Quaternion.identity, transform);
                heroUnit.Initialize(hero);
            }

            // TODO spawn monster
        }
    }
}