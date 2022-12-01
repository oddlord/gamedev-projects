using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace PocketHeroes
{
    public static class HeroGenerator
    {
        private const string _FIRST_NAMES_JSON_ADDRESS = "Assets/Systems/Hero/Json/first-names.json";

        private const int _BASE_HEALTH = 100;
        private const int _EXTRA_HEALTH_PER_IV = 20;
        private const int _MAX_HEALTH_IVS = 5;

        private const int _BASE_ATTACK_POWER = 10;
        private const int _EXTRA_ATTACK_POWER_PER_IV = 2;
        private const int _MAX_ATTACK_POWER_IVS = 5;

        private static string[] _names;

        public static Hero Generate()
        {
            string name = GetRandomName();
            int health = GetValueWithIvs(_BASE_HEALTH, _MAX_HEALTH_IVS, _EXTRA_HEALTH_PER_IV);
            int attackPower = GetValueWithIvs(_BASE_ATTACK_POWER, _MAX_ATTACK_POWER_IVS, _EXTRA_ATTACK_POWER_PER_IV);
            return new Hero(name, health, attackPower, 0, 1);
        }

        private static string GetRandomName()
        {
            if (_names == null) LoadNames();

            int i = UnityEngine.Random.Range(0, _names.Length);
            string name = _names[i];

            return name;
        }

        private static void LoadNames()
        {
            AsyncOperationHandle<TextAsset> loadHandle = Addressables.LoadAssetAsync<TextAsset>(_FIRST_NAMES_JSON_ADDRESS);
            TextAsset jsonFile = loadHandle.WaitForCompletion();

            string json = jsonFile.text;
            json = json.Replace("[", "");
            json = json.Replace("]", "");
            json = json.Replace("]", "");
            json = json.Replace("\n", "");
            _names = json.Split(",");

            Addressables.Release(loadHandle);
        }

        // IV = Individual Value
        // Each IV makes the a specific attribute of the Hero a little bit stronger
        // Concept stolen from Pokemon: https://bulbapedia.bulbagarden.net/wiki/Individual_values
        private static int GetValueWithIvs(int baseAmount, int maxIvs, int extraAmountPerIv)
        {
            int ivs = UnityEngine.Random.Range(0, maxIvs);
            return baseAmount + ivs * extraAmountPerIv;
        }
    }
}