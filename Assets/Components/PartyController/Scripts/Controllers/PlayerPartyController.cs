using System.Linq;

namespace PocketHeroes
{
    public class PlayerPartyController : PartyController
    {
        private bool _canAttack;

        public PlayerPartyController(CharacterUnit[] units, CharacterUnit[] enemyUnits) : base(units, enemyUnits)
        {
            foreach (CharacterUnit unit in units) unit.OnPress += OnHeroUnitPressed;
        }

        public override void DoTurn()
        {
            _canAttack = true;
        }

        private void OnHeroUnitPressed(CharacterUnit heroUnit)
        {
            if (!_canAttack || heroUnit.IsDead) return;

            _canAttack = false;

            CharacterUnit[] aliveEnemyUnits = _enemyUnits.Where(u => !u.IsDead).ToArray();
            CharacterUnit targetUnit = aliveEnemyUnits[UnityEngine.Random.Range(0, aliveEnemyUnits.Length)];
            heroUnit.Attack(targetUnit, OnAttacked);
        }
    }
}