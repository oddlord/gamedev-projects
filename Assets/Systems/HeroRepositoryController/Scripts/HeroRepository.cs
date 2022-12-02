using System.Collections.Generic;

namespace PocketHeroes
{
    // This interface can be implemented in different ways to change where/how we store the collected Heroes
    public interface HeroRepository
    {
        public List<Hero> GetHeroes();
        public void SetHeroes(List<Hero> heroes);
    }
}
