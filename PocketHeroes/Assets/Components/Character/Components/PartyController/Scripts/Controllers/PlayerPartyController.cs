using System.Collections.Generic;
using System.Linq;

namespace PocketHeroes
{
    public class PlayerPartyController : PartyController
    {
        private bool _canAttack;

        public override void Initialize()
        {
            base.Initialize();
            foreach (Unit unit in Units) unit.OnPress += OnUnitPressed;
        }

        public override bool IsPlayer() => true;

        public override void DoTurn()
        {
            _canAttack = true;
        }

        private void OnUnitPressed(Unit unit)
        {
            if (!_canAttack || unit.IsDead) return;

            _canAttack = false;

            Unit[] aliveEnemyUnits = _enemyUnits.Where(u => !u.IsDead).ToArray();
            Unit targetUnit = aliveEnemyUnits[UnityEngine.Random.Range(0, aliveEnemyUnits.Length)];
            unit.Attack(targetUnit, OnAttacked);
        }
    }
}