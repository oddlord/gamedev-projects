using System.Linq;

namespace PocketHeroes
{
    public class AiPartyController : PartyController
    {
        public AiPartyController(CharacterUnit[] units, CharacterUnit[] enemyUnits) : base(units, enemyUnits) { }

        public override void DoTurn()
        {
            CharacterUnit[] aliveUnits = _units.Where(u => !u.IsDead).ToArray();
            CharacterUnit unit = aliveUnits[UnityEngine.Random.Range(0, aliveUnits.Length)];

            CharacterUnit[] aliveEnemyUnits = _enemyUnits.Where(u => !u.IsDead).ToArray();
            CharacterUnit targetUnit = aliveEnemyUnits[UnityEngine.Random.Range(0, aliveEnemyUnits.Length)];

            unit.Attack(targetUnit, OnAttacked);
        }
    }
}