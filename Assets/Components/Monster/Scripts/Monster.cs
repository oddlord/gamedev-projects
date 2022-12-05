using System;
using UnityEngine;

namespace PocketHeroes
{
    [Serializable]
    public class Monster
    {
        [SerializeField] protected string _name;
        public string Name { get => _name; protected set { _name = value; } }

        [SerializeField] protected int _health;
        public int Health { get => _health; protected set { _health = value; } }

        [SerializeField] protected int _attackPower;
        public int AttackPower { get => _attackPower; protected set { _attackPower = value; } }

        public Monster(string name, int health, int attackPower)
        {
            _name = name;
            _health = health;
            _attackPower = attackPower;
        }

        public static bool operator ==(Monster monster1, Monster monster2)
        {
            return monster1.Name == monster2.Name
                && monster1.Health == monster2.Health
                && monster1.AttackPower == monster2.AttackPower;
        }

        public static bool operator !=(Monster monster1, Monster monster2) => !(monster1 == monster2);

        public override bool Equals(object obj)
        {
            return this == (obj as Monster);
        }
        public override int GetHashCode()
        {
            return $"{_name}{_health}{_attackPower}".GetHashCode();
        }

        public override string ToString() => JsonUtility.ToJson(this, true);
    }
}
