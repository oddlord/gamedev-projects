using System.Collections.Generic;
using UnityEngine;

namespace PocketHeroes
{
    [CreateAssetMenu(menuName = ScriptableObjects.MENU_PREFIX + HeroRepository.MENU_PREFIX + "PlayerPrefs")]
    public class PlayerPrefsHeroRepository : HeroRepository
    {
        private const string _COLLECTED_HEROES_KEY = "CollectedHeroes";

        public override List<Hero> GetHeroes()
        {
            string heroGroupJson = PlayerPrefs.GetString(_COLLECTED_HEROES_KEY);
            HeroGroup heroGroup;
            if (heroGroupJson != null && heroGroupJson.Length > 0)
            {
                heroGroup = JsonUtility.FromJson<HeroGroup>(heroGroupJson);
            }
            else
            {
                heroGroup = new HeroGroup();
            }
            return heroGroup.Heroes;
        }

        public override void SetHeroes(List<Hero> heroes)
        {
            HeroGroup heroGroup = new HeroGroup(heroes);
            string heroGroupJson = JsonUtility.ToJson(heroGroup);
            PlayerPrefs.SetString(_COLLECTED_HEROES_KEY, heroGroupJson);
        }
    }
}
