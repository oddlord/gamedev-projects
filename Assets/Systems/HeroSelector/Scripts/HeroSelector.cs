using UnityEngine;

namespace PocketHeroes
{
    public class HeroSelector : MonoBehaviour
    {
        [SerializeField] private HeroGroupState _collectedHeroes;

        void Awake()
        {
            Debug.Log($"Heroes: {string.Join(", ", _collectedHeroes.Heroes)}");
        }

        public void OnAddHeroPressed()
        {
            _collectedHeroes.AddHero(HeroGenerator.Generate());
        }

        public void OnLevelUpHeroPressed()
        {
            Hero hero = _collectedHeroes.Heroes[0];
            hero.GainExperience(6);
        }
    }
}