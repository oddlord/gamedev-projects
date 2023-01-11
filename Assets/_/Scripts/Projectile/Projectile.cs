using System;
using UnityEngine;

namespace SpaceMiner
{
    // TODO make into an interface
    public abstract class Projectile : MonoBehaviour
    {
        [Serializable]
        private struct _InternalSetup
        {
            public AudioSource AudioSource;
        }

        [Header("Audio")]
        [SerializeField] private AudioClip _fireSound;

        [Header("__Internal Setup__")]
        [SerializeField] private _InternalSetup _internalSetup;

        protected bool _fired;

        protected virtual void Awake()
        {
            _fired = false;
        }

        public virtual void Fire()
        {
            _fired = true;
            PlayAudio(_fireSound);
        }

        private void PlayAudio(AudioClip clip)
        {
            _internalSetup.AudioSource.clip = clip;
            _internalSetup.AudioSource.Play();
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(Tags.OBSTACLE)) Destroy(this.gameObject);
        }
    }
}