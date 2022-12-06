using System;
using System.Collections.Generic;
using UnityEngine;

namespace PocketHeroes
{
    [CreateAssetMenu(menuName = ScriptableObjects.MENU_PREFIX + HeroRepository.MENU_PREFIX + "PlayerPrefs")]
    public class PlayerPrefsHeroRepository : HeroRepository
    {
        [Serializable]
        private class _HeroList
        {
            public List<Hero> Heroes;
        }

        private const string _KEY = "CollectedHeroes";

        public override List<Hero> Get()
        {
            string heroListJson = PlayerPrefs.GetString(_KEY);
            _HeroList heroList;
            if (heroListJson != null && heroListJson.Length > 0) heroList = JsonUtility.FromJson<_HeroList>(heroListJson);
            else heroList = new _HeroList();
            return heroList.Heroes;
        }

        public override void Set(List<Hero> heroes)
        {
            _HeroList heroList = new _HeroList() { Heroes = heroes };
            string heroListJson = JsonUtility.ToJson(heroList);
            PlayerPrefs.SetString(_KEY, heroListJson);
        }
    }
}
