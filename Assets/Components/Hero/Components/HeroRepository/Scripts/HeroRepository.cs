using System.Collections.Generic;
using UnityEngine;

namespace PocketHeroes
{
    // This abstract class can be implemented in different ways to change where/how we store the collected Heroes
    // TODO rename to CollectedHeroesRepository
    public abstract class HeroRepository : ScriptableObject
    {
        public const string MENU_PREFIX = "Hero Repositories/";

        public abstract List<Hero> Get();
        public abstract void Set(List<Hero> heroes);
    }
}
