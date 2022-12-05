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

        public static string[] GetHeroUnitTooltipRows(HeroUnit heroUnit)
        {
            return new string[]{
                $"Name: {heroUnit.Hero.Name}",
                $"Health: {heroUnit.CurrentHealth}/{heroUnit.Hero.Health}",
                $"Attack Power: {heroUnit.Hero.AttackPower}",
                $"Level: {heroUnit.Hero.Level}",
                $"Experience: {heroUnit.Hero.Experience}",
            };
        }

        public static string[] GetMonsterUnitTooltipRows(MonsterUnit monsterUnit)
        {
            return new string[]{
                $"Name: {monsterUnit.Monster.Name}",
                $"Health: {monsterUnit.CurrentHealth}/{monsterUnit.Monster.Health}",
                $"Attack Power: {monsterUnit.Monster.AttackPower}",
            };
        }
    }
}