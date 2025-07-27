using System.Collections.Generic;
using UnityEngine;

namespace PocketHeroes
{
    // This abstract class can be implemented in different ways to change where/how we store the collected Heroes
    public abstract class CollectedHeroesRepository : ScriptableObject
    {
        public const string MENU_PREFIX = "CollectedHero Repositories/";

        public abstract List<Hero> Get();
        public abstract void Set(List<Hero> heroes);
    }
}
