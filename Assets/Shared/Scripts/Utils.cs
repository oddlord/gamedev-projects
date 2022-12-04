namespace PocketHeroes
{
    // TODO rename to TooltipUtils?
    public static class Utils
    {
        // TODO this function doesn't belong with either the Hero or the tooltip classes
        // Find the proper place for this function
        public static string[] GetHeroTooltipRows(Hero hero)
        {
            return new string[]{
                $"Name: {hero.Name}",
                $"Level: {hero.Level}",
                $"Attack Power: {hero.AttackPower}",
                $"Experience: {hero.Experience}",
            };
        }
    }
}