namespace PocketHeroes
{
    public static class TooltipUtils
    {
        // TODO this function doesn't belong with either the Hero or the tooltip classes
        // Find the proper place for this function, same for the other functions
        public static string[] GetHeroTooltipRows(Hero hero)
        {
            return new string[]{
                $"Name: {hero.Name}",
                $"Health: {hero.Health}",
                $"Attack Power: {hero.AttackPower}",
                $"Level: {hero.Level}",
                $"Experience: {hero.Experience}",
            };
        }
    }
}