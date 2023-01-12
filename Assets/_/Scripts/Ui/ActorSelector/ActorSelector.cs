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

        [Serializable]
        private struct _IActorListElement
        {
            [SerializeField]
            [RequireInterface(typeof(IActor))]
            private UnityEngine.Object _actor;
            public IActor Actor => _actor as IActor;
        }

        [SerializeField] private ActorEntry _actorEntryPrefab;

        [SerializeField] private _IActorListElement[] _actors;
        private IActor[] _iActors => _actors.Select(a => a.Actor).ToArray();

        [Header("__Internal Setup__")]
        [SerializeField] private _InternalSetup _internalSetup;

        private Action<IActor> _onSelected;

        public void Show(Action<IActor> onSelected)
        {
            gameObject.SetActive(true);
            _onSelected = onSelected;
            foreach (IActor actor in _iActors)
            {
                ActorEntry shipEntry = Instantiate(_actorEntryPrefab, _internalSetup.ActorEntriesContainer);
                shipEntry.Initialize(actor.GetSprite(), () => OnActorSelected(actor));
            }
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void OnActorSelected(IActor actorPrefab)
        {
            _onSelected(actorPrefab);
        }
    }
}