using System;
using UnityEngine;

namespace PocketHeroes
{
    [CreateAssetMenu(menuName = ScriptableObjects.MENU_PREFIX + "BattlesFoughtState")]
    public class BattlesFoughtState : ScriptableObject
    {
        [SerializeField] private int _amount;
        public int Amount { get => _amount; private set { _amount = value; } }

        public Action<BattlesFoughtState> OnChange;

        public BattlesFoughtState(int amount)
        {
            Initialize(amount);
        }

        public BattlesFoughtState()
        {
            Initialize();
        }

        public void Initialize(int amount = 0)
        {
            _amount = amount;
        }

        public void Increment()
        {
            _amount++;
            OnChange?.Invoke(this);
        }
    }
}