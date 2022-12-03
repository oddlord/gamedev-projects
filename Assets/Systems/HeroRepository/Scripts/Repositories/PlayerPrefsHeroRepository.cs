using System.Collections.Generic;
using UnityEngine;

namespace PocketHeroes
{
    [CreateAssetMenu(menuName = ScriptableObjects.MENU_PREFIX + HeroRepository.MENU_PREFIX + "PlayerPrefs")]
    public class PlayerPrefsHeroRepository : HeroRepository
    {
        private const string _HERO_COLLECTION_KEY = "HeroCollection";

        public override List<Hero> GetHeroes()
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

        public override void SetHeroes(List<Hero> heroes)
        {
            HeroCollection heroCollection = new HeroCollection() { Heroes = heroes };
            string heroCollectionJson = JsonUtility.ToJson(heroCollection);
            PlayerPrefs.SetString(_HERO_COLLECTION_KEY, heroCollectionJson);
        }
    }
}
