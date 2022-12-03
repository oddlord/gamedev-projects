using UnityEngine;
using UnityEngine.Events;

namespace PocketHeroes
{
    public class GameEventListener : MonoBehaviour
    {
        public GameEvent Event;
        public UnityEvent Response;

        public void OnEventRaised()
        {
            Response.Invoke();
        }

        void OnEnable()
        {
            Event.RegisterListener(this);
        }

        void OnDisable()
        {
            Event.UnregisterListener(this);
        }
    }
}