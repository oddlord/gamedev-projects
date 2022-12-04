using System.Collections.Generic;
using UnityEngine;

namespace PocketHeroes
{
    // This abstract class can be implemented in different ways to change where/how we store the collected Heroes
    public abstract class HeroRepository : ScriptableObject
    {
        public const string MENU_PREFIX = "Hero Repositories/";

        public abstract List<Hero> GetHeroes();
        public abstract void SetHeroes(List<Hero> heroes);
    }
}
