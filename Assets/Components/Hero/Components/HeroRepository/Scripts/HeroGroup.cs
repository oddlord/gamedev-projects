using System;
using System.Collections.Generic;

namespace PocketHeroes
{
    [Serializable]
    // This class is needed to easily (de)serialize a list of Heroes
    public class HeroGroup
    {
        public List<Hero> Heroes;

        public HeroGroup(List<Hero> heroes)
        {
            Heroes = heroes;
        }

        public HeroGroup()
        {
            Heroes = new List<Hero>();
        }
    }
}