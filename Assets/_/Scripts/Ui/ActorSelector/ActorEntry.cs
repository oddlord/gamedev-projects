using System;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMiner
{
    public class ActorEntry : MonoBehaviour
    {
        [Serializable]
        private struct _InternalSetup
        {
            public Image Image;
        }

        [Header("__Internal Setup__")]
        [SerializeField] private _InternalSetup _internalSetup;

        private Action _onSelected;

        public void Initialize(Sprite sprite, Action onSelected)
        {
            _onSelected = onSelected;
            _internalSetup.Image.sprite = sprite;
        }

        public void Select()
        {
            _onSelected();
        }
    }
}