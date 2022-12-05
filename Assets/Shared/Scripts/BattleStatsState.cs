using System;
using UnityEngine;

namespace PocketHeroes
{
    [CreateAssetMenu(menuName = ScriptableObjects.MENU_PREFIX + "BattleStatsState")]
    public class BattleStatsState : ScriptableObject
    {
        [SerializeField] private int _battlesFought;
        public int BattlesFought { get => _battlesFought; private set { _battlesFought = value; } }

        public Action<BattleStatsState> OnChange;

        public BattleStatsState(int battleFought)
        {
            _battlesFought = battleFought;
        }

        public void IncrementBattlesFought()
        {
            _battlesFought++;
            OnChange?.Invoke(this);
        }
    }
}