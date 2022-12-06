using System;

namespace PocketHeroes
{
    // TODO make this subclasses into scriptable objects
    public abstract class PartyController
    {
        public Action OnTurnFinished;

        protected CharacterUnit[] _units;
        protected CharacterUnit[] _enemyUnits;

        public PartyController(CharacterUnit[] units, CharacterUnit[] enemyUnits)
        {
            _units = units;
            _enemyUnits = enemyUnits;
        }

        public abstract void DoTurn();

        public void OnBattleOver(bool won)
        {
            foreach (CharacterUnit unit in _units)
            {
                unit.OnBattleOver(won);
            }
        }

        public bool HasAliveUnits()
        {
            foreach (CharacterUnit unit in _units)
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