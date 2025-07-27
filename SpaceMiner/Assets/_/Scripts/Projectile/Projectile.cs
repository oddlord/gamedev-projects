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

        public Hittable Hittable;

        [Header("Audio")]
        [SerializeField] private AudioClip _fireSound;

        [Header("__Internal Setup__")]
        [SerializeField] private _InternalSetup _internalSetup;

        protected bool _fired;

        protected virtual void Awake()
        {
            _fired = false;

            Hittable.OnHit += OnHit;
        }

        public virtual void Fire()
        {
            _fired = true;
            PlayAudio(_fireSound);
        }

        public virtual void SetTag(string tag)
        {
            gameObject.tag = tag;
        }

        private void PlayAudio(AudioClip clip)
        {
            _internalSetup.AudioSource.clip = clip;
            _internalSetup.AudioSource.Play();
        }

        private void OnHit()
        {
            Destroy(this.gameObject);
        }

        void OnDestroy()
        {
            Hittable.OnHit -= OnHit;
        }
    }
}