using UnityEngine;

namespace PocketHeroes
{
    public class HeroSelector : MonoBehaviour
    {
        [SerializeField] private GameState _gameState;

        void Awake()
        {
            Debug.Log($"Heroes: {string.Join(", ", _gameState.CollectedHeroes)}");
        }
    }
}