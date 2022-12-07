using System;
using UnityEngine;

namespace PocketHeroes
{
    [Serializable]
    public class Hero : Character
    {
        private const int _EXPERIENCE_PER_LEVEL = 5;
        private const float _ATTRIBUTES_GROWTH_FACTOR_PER_LEVEL = 0.1f;

        /// <summary>
        ///     This event is triggered every time the Hero gains some experience points.
        ///     Parameters:
        ///     <list type="bullet">
        ///         <item><c>experienceGained</c></item>
        ///         <item><c>healthDelta</c></item>
        ///         <item><c>attackPowerDelta</c></item>
        ///         <item><c>levelDelta</c></item>
        ///     </list>
        /// </summary>
        public Action<int, int, int, int> OnExperienceGain;

        [SerializeField] private int _experience;
        public int Experience { get => _experience; private set => _experience = value; }

        [SerializeField] private int _level;
        public int Level { get => _level; private set => _level = value; }

        public Action<Hero> OnChange;

        public Hero(string name, int health, int attackPower, int experience, int level) : base(name, health, attackPower)
        {
            _experience = experience;
            _level = level;
        }

        public void GainExperience(int experiencePoints = 1)
        {
            int oldHealth = Health;
            int oldAttackPower = AttackPower;
            int oldLevel = Level;

            _experience += experiencePoints;
            while (CanLevelUp()) LevelUp();

            OnChange?.Invoke(this);
            OnExperienceGain(experiencePoints, Health - oldHealth, AttackPower - oldAttackPower, Level - oldLevel);
        }

        private bool CanLevelUp() => _experience >= _EXPERIENCE_PER_LEVEL;

        private void LevelUp()
        {
            Health += (int)Math.Round(Health * _ATTRIBUTES_GROWTH_FACTOR_PER_LEVEL);
            AttackPower += (int)Math.Round(AttackPower * _ATTRIBUTES_GROWTH_FACTOR_PER_LEVEL);
            _experience -= _EXPERIENCE_PER_LEVEL;
            _level++;
        }

        public static bool operator ==(Hero hero1, Hero hero2)
        {
            return (hero1 as Character) == (hero2 as Character)
                && hero1.Experience == hero2.Experience
                && hero1.Level == hero2.Level;
        }

        public static bool operator !=(Hero hero1, Hero hero2) => !(hero1 == hero2);

        public override bool Equals(object obj)
        {
            return this == (obj as Hero);
        }
        public override int GetHashCode()
        {
            return $"{Name}{Health}{AttackPower}{Experience}{Level}".GetHashCode();
        }

        public override string ToString() => JsonUtility.ToJson(this, true);
    }
}
