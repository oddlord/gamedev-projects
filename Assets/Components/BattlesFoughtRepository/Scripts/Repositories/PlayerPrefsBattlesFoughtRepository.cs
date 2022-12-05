using System;
using UnityEngine;

namespace PocketHeroes
{
    [CreateAssetMenu(menuName = ScriptableObjects.MENU_PREFIX + BattlesFoughtRepository.MENU_PREFIX + "PlayerPrefs")]
    public class PlayerPrefsBattlesFoughtRepository : BattlesFoughtRepository
    {
        [Serializable]
        private class _BattlesFought
        {
            public int Amount;
        }

        private const string _KEY = "BattlesFought";

        public override int Get()
        {
            string json = PlayerPrefs.GetString(_KEY);
            _BattlesFought battlesFought;
            if (json != null && json.Length > 0) battlesFought = JsonUtility.FromJson<_BattlesFought>(json);
            else battlesFought = new _BattlesFought() { Amount = 0 };
            return battlesFought.Amount;
        }

        public override void Set(int amount)
        {
            _BattlesFought battlesFought = new _BattlesFought() { Amount = amount };
            string json = JsonUtility.ToJson(battlesFought);
            PlayerPrefs.SetString(_KEY, json);
        }
    }
}
