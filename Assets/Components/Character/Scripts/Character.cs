using System;
using UnityEngine;

namespace PocketHeroes
{
    [Serializable]
    public class Character
    {
        [SerializeField] protected string _name;
        public string Name { get => _name; protected set { _name = value; } }

        [SerializeField] protected int _health;
        public int Health { get => _health; protected set { _health = value; } }

        [SerializeField] protected int _attackPower;
        public int AttackPower { get => _attackPower; protected set { _attackPower = value; } }

        public Character(string name, int health, int attackPower)
        {
            _name = name;
            _health = health;
            _attackPower = attackPower;
        }

        public static bool operator ==(Character character1, Character character2)
        {
            return character1.Name == character2.Name
                && character1.Health == character2.Health
                && character1.AttackPower == character2.AttackPower;
        }

        public static bool operator !=(Character character1, Character character2) => !(character1 == character2);

        public override bool Equals(object obj)
        {
            return this == (obj as Character);
        }
        public override int GetHashCode()
        {
            return $"{_name}{_health}{_attackPower}".GetHashCode();
        }

        public override string ToString() => JsonUtility.ToJson(this, true);
    }
}
