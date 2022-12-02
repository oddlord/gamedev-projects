using System.Collections.Generic;
using UnityEngine;

namespace PocketHeroes
{
    // TODO make it have a single list and split the game state into two scriptable objects (one for collected, one for selected)
    [CreateAssetMenu(menuName = ScriptableObjects.MENU_PREFIX + "GameState")]
    public class GameState : ScriptableObject
    {
        public List<Hero> CollectedHeroes;
        public List<Hero> SelectedHeroes;
    }
}