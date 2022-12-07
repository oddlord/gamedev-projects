using System;
using UnityEngine;

namespace PocketHeroes
{
    public abstract class PartyController : MonoBehaviour
    {
        [SerializeField] private Unit[] _unitPrefabs;
        [SerializeField] private Transform _spawnPointsContainer;

        [Tooltip("Optional. Characters will be generated if not specified.")]
        [SerializeField] private HeroGroupState _premadeHeroes;
        [Tooltip("Number of units to spawn. Ignored if Premade Heroes is specified.")]
        [SerializeField] private int _count = 1;

        public Action OnTurnFinished;

        [HideInInspector] public Unit[] Units;

        protected Unit[] _enemyUnits;

        public virtual void Initialize()
        {
            _count = _premadeHeroes != null ? _premadeHeroes.Heroes.Count : _count;

            Units = new Unit[_count];
            SpawnPoint[] spawnPoints = _spawnPointsContainer.GetComponentsInChildren<SpawnPoint>();

            for (int i = 0; i < _count; i++)
            {
                Unit unitPrefab = _unitPrefabs[UnityEngine.Random.Range(0, _unitPrefabs.Length)];
                SpawnPoint spawnPoint = spawnPoints[i % spawnPoints.Length];

                Character character;
                if (unitPrefab is HeroUnit)
                {
                    if (_premadeHeroes != null) character = _premadeHeroes.Heroes[i];
                    else character = HeroGenerator.Generate();
                }
                else character = MonsterGenerator.Generate();

                Unit playerUnit = Instantiate(unitPrefab, spawnPoint.Position, Quaternion.identity, transform);
                playerUnit.Initialize(character);
                Units[i] = playerUnit;
            }
        }

        public void SetEnemyUnits(Unit[] enemyUnits)
        {
            _enemyUnits = enemyUnits;
        }

        // Override this to true for Player controllers
        public virtual bool IsPlayer() => false;

        public abstract void DoTurn();

        public void OnBattleOver(bool won)
        {
            foreach (Unit unit in Units)
            {
                unit.OnBattleOver(won);
            }
        }

        public bool HasAliveUnits()
        {
            foreach (Unit unit in Units)
            {
                if (!unit.IsDead) return true;
            }

            return false;
        }

        protected void OnAttacked()
        {
            OnTurnFinished?.Invoke();
        }
    }
}