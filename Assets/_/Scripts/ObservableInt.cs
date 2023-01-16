namespace SpaceMiner
{
    public class ObservableInt
    {
        private int _value;
        public int Value
        {
            get => _value;
            set
            {
                int delta = value - _value;
                _value = value;
                if (delta != 0) OnChange?.Invoke(_value, delta);
            }
        }

        public delegate void ChangeEvent(int newValue, int delta);
        public ChangeEvent OnChange;

        public ObservableInt(int initialValue = 0)
        {
            _value = initialValue;
        }

        public void Add(int value)
        {
            Value += value;
        }

        public void Subtract(int value)
        {
            Add(-value);
        }
    }
}