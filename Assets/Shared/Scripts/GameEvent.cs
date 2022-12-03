using System.Collections.Generic;
using UnityEngine;

namespace PocketHeroes
{
    [CreateAssetMenu(menuName = ScriptableObjects.MENU_PREFIX + "GameEvent")]
    public class GameEvent : ScriptableObject
    {
        private List<GameEventListener> _listeners = new List<GameEventListener>();

        public void Raise()
        {
            for (int i = _listeners.Count - 1; i >= 0; i--)
                _listeners[i].OnEventRaised();
        }

        public void RegisterListener(GameEventListener listener)
        {
            if (_listeners.Contains(listener)) return;
            _listeners.Add(listener);
        }

        public void UnregisterListener(GameEventListener listener)
        {
            if (!_listeners.Contains(listener)) return;
            _listeners.Remove(listener);
        }
    }
}