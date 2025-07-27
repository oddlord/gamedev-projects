namespace PocketHeroes
{
    public static class CharacterGeneratorUtils
    {
        // IV = Individual Value
        // Each IV makes the a specific attribute of the Hero a little bit stronger
        // Concept stolen from Pokemon: https://bulbapedia.bulbagarden.net/wiki/Individual_values
        public static int GetValueWithIvs(int baseAmount, int maxIvs, int extraAmountPerIv)
        {
            int ivs = UnityEngine.Random.Range(0, maxIvs);
            return baseAmount + ivs * extraAmountPerIv;
        }
    }
}