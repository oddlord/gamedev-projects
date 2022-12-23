using System;
using UnityEngine;

namespace SpaceMiner
{
    public class ActorSelector : MonoBehaviour
    {
        [Serializable]
        private struct _ActorSelectionEntry
        {
            public Sprite Sprite;
            public Actor Prefab;
        }

        [Serializable]
        private struct _InternalSetup
        {
            public Transform ActorEntriesContainer;
        }

        [SerializeField] ActorEntry _actorEntryPrefab;
        [SerializeField] _ActorSelectionEntry[] _actors;

        [Header("__Internal Setup__")]
        [SerializeField] private _InternalSetup _internalSetup;

        private Action<Actor> _onSelected;

        public void Show(Action<Actor> onSelected)
        {
            gameObject.SetActive(true);
            _onSelected = onSelected;
            foreach (_ActorSelectionEntry shipSelectionEntry in _actors)
            {
                ActorEntry shipEntry = Instantiate(_actorEntryPrefab, _internalSetup.ActorEntriesContainer);
                shipEntry.Initialize(shipSelectionEntry.Sprite, () => OnActorSelected(shipSelectionEntry.Prefab));
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