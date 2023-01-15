using System;
using UnityEngine;
using System.Linq;

namespace SpaceMiner
{
    public class Hittable : MonoBehaviour
    {
        public Action OnHit;

        public string[] HitTags;

        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (HitTags == null) return;
            if (HitTags.Contains(other.tag)) OnHit?.Invoke();
        }
    }
}
