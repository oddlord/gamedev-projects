using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace PocketHeroes
{
    public static class MonsterGenerator
    {
        private const string _MONSTER_NAMES_JSON_ADDRESS = "Assets/Components/Monster/Json/monsters.json";

        private const int _BASE_HEALTH = 250;
        private const int _EXTRA_HEALTH_PER_IV = 50;
        private const int _MAX_HEALTH_IVS = 5;

        private const int _BASE_ATTACK_POWER = 15;
        private const int _EXTRA_ATTACK_POWER_PER_IV = 3;
        private const int _MAX_ATTACK_POWER_IVS = 5;

        private static List<string> _names;

        static MonsterGenerator()
        {
            _names = new List<string>();

            AsyncOperationHandle<TextAsset> loadHandle = Addressables.LoadAssetAsync<TextAsset>(_MONSTER_NAMES_JSON_ADDRESS);
            TextAsset jsonFile = loadHandle.WaitForCompletion();
            string json = jsonFile.text;

            // TODO find out why JsonUtility doesn't seem to work well with Dictionary and use a more elegant deserialiser than this
            json = json.Substring(1);
            while (true)
            {
                int openingQuotationMarkIdx = json.IndexOf("\"");
                if (openingQuotationMarkIdx < 0) break;
                json = json.Substring(openingQuotationMarkIdx + 1);

                int closingQuotationMarkIdx = json.IndexOf("\"");
                string name = json.Substring(0, closingQuotationMarkIdx);
                _names.Add(name);

                json = json.Substring(closingQuotationMarkIdx + 1);
                int closingCurlyBracketIdx = json.IndexOf("}");
                json = json.Substring(closingCurlyBracketIdx + 1);
            }

            Addressables.Release(loadHandle);
        }

        public static Monster Generate()
        {
            string name = GetRandomName();
            int health = CharacterGeneratorUtils.GetValueWithIvs(_BASE_HEALTH, _MAX_HEALTH_IVS, _EXTRA_HEALTH_PER_IV);
            int attackPower = CharacterGeneratorUtils.GetValueWithIvs(_BASE_ATTACK_POWER, _MAX_ATTACK_POWER_IVS, _EXTRA_ATTACK_POWER_PER_IV);
            return new Monster(name, health, attackPower);
        }

        private static string GetRandomName()
        {
            int i = UnityEngine.Random.Range(0, _names.Count);
            string name = _names[i];

            return name;
        }
    }
}