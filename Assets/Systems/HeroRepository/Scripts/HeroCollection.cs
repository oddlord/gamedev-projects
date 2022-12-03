using System;
using System.Collections.Generic;

namespace PocketHeroes
{
    [Serializable]
    public class HeroCollection
    {
        public List<Hero> Heroes;

        public HeroCollection(List<Hero> heroes)
        {
            Heroes = heroes;
        }

        public HeroCollection()
        {
            Heroes = new List<Hero>();
        }
    }
}