using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace PocketHeroes
{
    public static class HeroGenerator
    {
        [Serializable]
        private class _HeroNameList
        {
            public string[] Names;
        }

        private const string _HERO_NAMES_JSON_ADDRESS = "Assets/Components/Hero/Json/first-names.json";

        private const int _BASE_HEALTH = 100;
        private const int _EXTRA_HEALTH_PER_IV = 20;
        private const int _MAX_HEALTH_IVS = 5;

        private const int _BASE_ATTACK_POWER = 10;
        private const int _EXTRA_ATTACK_POWER_PER_IV = 2;
        private const int _MAX_ATTACK_POWER_IVS = 5;

        private static string[] _names;

        static HeroGenerator()
        {
            AsyncOperationHandle<TextAsset> loadHandle = Addressables.LoadAssetAsync<TextAsset>(_HERO_NAMES_JSON_ADDRESS);
            TextAsset jsonFile = loadHandle.WaitForCompletion();

            _names = JsonUtility.FromJson<_HeroNameList>($"{{\"Names\":{jsonFile.text}}}").Names;

            Addressables.Release(loadHandle);
        }

        public static Hero Generate()
        {
            string name = GetRandomName();
            int health = CharacterGeneratorUtils.GetValueWithIvs(_BASE_HEALTH, _MAX_HEALTH_IVS, _EXTRA_HEALTH_PER_IV);
            int attackPower = CharacterGeneratorUtils.GetValueWithIvs(_BASE_ATTACK_POWER, _MAX_ATTACK_POWER_IVS, _EXTRA_ATTACK_POWER_PER_IV);
            return new Hero(name, health, attackPower, 0, 1);
        }

        private static string GetRandomName()
        {
            int i = UnityEngine.Random.Range(0, _names.Length);
            string name = _names[i];

            return name;
        }
    }
}