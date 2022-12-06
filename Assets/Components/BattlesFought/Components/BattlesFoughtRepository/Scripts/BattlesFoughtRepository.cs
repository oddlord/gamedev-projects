using UnityEngine;

namespace PocketHeroes
{
    // This abstract class can be implemented in different ways to change where/how we store the number of battles fought
    public abstract class BattlesFoughtRepository : ScriptableObject
    {
        public const string MENU_PREFIX = "BattlesFought Repositories/";

        public abstract int Get();
        public abstract void Set(int battlesFought);
    }
}
