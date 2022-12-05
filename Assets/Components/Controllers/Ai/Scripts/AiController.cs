using System;
using System.Linq;
using UnityEngine;

namespace PocketHeroes
{
    public class AiController
    {
        public Action OnTurnFinished;

        private MonsterUnit Unit;

        private HeroUnit[] _enemyUnits;

        public AiController(MonsterUnit unit, HeroUnit[] enemyUnits)
        {
            Unit = unit;
            _enemyUnits = enemyUnits;
        }

        public void DoTurn()
        {
            HeroUnit[] aliveEnemyUnits = _enemyUnits.Where(u => !u.IsDead).ToArray();
            HeroUnit targetUnit = aliveEnemyUnits[UnityEngine.Random.Range(0, aliveEnemyUnits.Length)];
            Unit.Attack(targetUnit, OnAttacked);
        }

        public bool HasAliveUnits() => !Unit.IsDead;

        private void OnAttacked()
        {
            OnTurnFinished?.Invoke();
        }
    }
}