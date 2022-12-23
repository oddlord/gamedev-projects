using UnityEngine;

namespace SpaceMiner
{
    [CreateAssetMenu(menuName = ScriptableObjects.MENU_PREFIX + "IntState")]
    public class IntState : ScriptableObject
    {
        [SerializeField] private int _value;
        public int Value { get => _value; private set => _value = value; }

        public delegate void ChangeEvent(int newValue, int delta);
        public ChangeEvent OnChange;

        public void Set(int value)
        {
            int delta = value - _value;
            _value = value;
            if (delta != 0) OnChange?.Invoke(value, delta);
        }

        public void Set(IntState otherState)
        {
            Set(otherState.Value);
        }

        public void Add(int value)
        {
            Set(_value + value);
        }
    }
}