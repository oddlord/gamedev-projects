using System;
using UnityEngine;

namespace PocketHeroes
{
    public class PlayerController
    {
        public Action OnTurnFinished;

        public HeroUnit[] Units;

        private MonsterUnit _enemyUnit;
        private bool _canAttack;

        public PlayerController(HeroUnit[] units, MonsterUnit enemyUnit)
        {
            Units = units;
            _enemyUnit = enemyUnit;

            foreach (HeroUnit heroUnit in units)
            {
                heroUnit.OnPress += OnHeroUnitPressed;
            }
        }

        public void DoTurn()
        {
            _canAttack = true;
        }

        public bool HasAliveUnits()
        {
            foreach (HeroUnit unit in Units)
            {
                if (!unit.IsDead) return true;
            }

            return false;
        }

        private void OnHeroUnitPressed(HeroUnit heroUnit)
        {
            if (!_canAttack || heroUnit.IsDead) return;

            _canAttack = false;
            heroUnit.Attack(_enemyUnit, OnAttacked);
        }

        private void OnAttacked()
        {
            OnTurnFinished?.Invoke();
        }
    }
}