using System;
using System.Linq;
using Oddlord.RequireInterface;
using UnityEngine;
namespace SpaceMiner
{
    public class ActorSelector : MonoBehaviour
    {
        [Serializable]
        private struct _InternalSetup
        {
            public Transform ActorEntriesContainer;
        }

        [SerializeField] private ActorEntry _actorEntryPrefab;
        [SerializeField] private Actor[] _actors;

        [Header("__Internal Setup__")]
        [SerializeField] private _InternalSetup _internalSetup;

        private Action<Actor> _onSelected;

        public void Show(Action<Actor> onSelected)
        {
            gameObject.SetActive(true);
            _onSelected = onSelected;
            foreach (Actor actor in _actors)
            {
                ActorEntry shipEntry = Instantiate(_actorEntryPrefab, _internalSetup.ActorEntriesContainer);
                shipEntry.Initialize(actor.GetSprite(), () => OnActorSelected(actor));
            }
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void OnActorSelected(Actor actorPrefab)
        {
            _onSelected(actorPrefab);
        }
    }
}