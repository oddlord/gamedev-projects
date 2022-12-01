using UnityEngine;

namespace PocketHeroes
{
    public class GameLoader : MonoBehaviour
    {
        [SerializeField] private HeroManager _heroManagerPrefab;

        void Awake()
        {
            HeroManager heroManager = Instantiate(_heroManagerPrefab);
            heroManager.Initialize();
            DontDestroyOnLoad(heroManager.gameObject);
        }
    }
}