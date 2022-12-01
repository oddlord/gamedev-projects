using System.Collections.Generic;
using UnityEngine;

namespace PocketHeroes
{
    public class PlayerPrefsHeroRepository : HeroRepository
    {
        private const string _HERO_COLLECTION_KEY = "HeroCollection";

        public List<Hero> GetHeroes()
        {
            string heroCollectionJson = PlayerPrefs.GetString(_HERO_COLLECTION_KEY);
            HeroCollection heroCollection;
            if (heroCollectionJson != null && heroCollectionJson.Length > 0)
            {
                heroCollection = JsonUtility.FromJson<HeroCollection>(heroCollectionJson);
            }
            else
            {
                heroCollection = new HeroCollection();
            }
            return heroCollection.Heroes;
        }

        public void SetHeroes(List<Hero> heroes)
        {
            HeroCollection heroCollection = new HeroCollection() { Heroes = heroes };
            string heroCollectionJson = JsonUtility.ToJson(heroCollection);
            PlayerPrefs.SetString(_HERO_COLLECTION_KEY, heroCollectionJson);
        }
    }
}
