using System;
using UnityEngine;

namespace SpaceMiner
{
    [Serializable]
    public class ActorState
    {
        public delegate void ChangeEvent(int newValue, int delta);
        public ChangeEvent OnLivesChange;
        public ChangeEvent OnMaxLivesChange;

        [SerializeField] private int _lives;
        public int Lives
        {
            get => _lives;
            set
            {
                int delta = value - _lives;
                _lives = value;
                if (delta != 0) OnLivesChange?.Invoke(_lives, delta);
            }
        }

        [SerializeField] private int _maxLives;
        public int MaxLives
        {
            get => _maxLives;
            set
            {
                int delta = value - _maxLives;
                _maxLives = value;
                if (delta != 0) OnMaxLivesChange?.Invoke(_maxLives, delta);
            }
        }

        public void Init(int maxLives)
        {
            MaxLives = maxLives;
        }

        public void SetFullLives()
        {
            Lives = _maxLives;
        }
    }
}
