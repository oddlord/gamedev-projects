using System;
using UnityEngine;

namespace SpaceMiner
{
    [CreateAssetMenu(menuName = ScriptableObjects.MENU_PREFIX + "IntState")]
    public class IntState : ScriptableObject
    {
        [SerializeField] private int _value;
        public int Value { get => _value; private set => _value = value; }

        public Action<int> OnChange;

        public void Set(int value)
        {
            bool changed = value != _value;
            _value = value;
            if (changed) OnChange?.Invoke(value);
        }

        public void Set(IntState otherState)
        {
            Set(otherState.Value);
        }
    }
}