using System.Linq;

namespace PocketHeroes
{
    public class AiPartyController : PartyController
    {
        public override void DoTurn()
        {
            Unit[] aliveUnits = Units.Where(u => !u.IsDead).ToArray();
            Unit unit = aliveUnits[UnityEngine.Random.Range(0, aliveUnits.Length)];

            Unit[] aliveEnemyUnits = _enemyUnits.Where(u => !u.IsDead).ToArray();
            Unit targetUnit = aliveEnemyUnits[UnityEngine.Random.Range(0, aliveEnemyUnits.Length)];

            unit.Attack(targetUnit, OnAttacked);
        }
    }
}