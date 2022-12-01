using System;
using UnityEngine;

namespace PocketHeroes
{
    [Serializable]
    public class Hero
    {
        private const int _EXPERIENCE_PER_LEVEL = 5;
        private const float _ATTRIBUTES_GROWTH_FACTOR_PER_LEVEL = 0.1f;

        public string Name;
        public int Health;
        public int AttackPower;
        public int Experience;
        public int Level;

        public Hero(string name, int health, int attackPower, int experience, int level)
        {
            Name = name;
            Health = health;
            AttackPower = attackPower;
            Experience = experience;
            Level = level;
        }

        public void GainExperience(int experiencePoints = 1)
        {
            Experience += experiencePoints;
            while (CanLevelUp()) LevelUp();
        }

        private bool CanLevelUp() => Experience >= _EXPERIENCE_PER_LEVEL;

        private void LevelUp()
        {
            Health += (int)Math.Round(Health * _ATTRIBUTES_GROWTH_FACTOR_PER_LEVEL);
            AttackPower += (int)Math.Round(AttackPower * _ATTRIBUTES_GROWTH_FACTOR_PER_LEVEL);
            Experience -= _EXPERIENCE_PER_LEVEL;
            Level++;
        }

        public override string ToString()
        {
            return JsonUtility.ToJson(this, true);
        }
    }
}
