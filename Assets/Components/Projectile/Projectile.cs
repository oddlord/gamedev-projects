using UnityEngine;

namespace SpaceMiner
{
    public abstract class Projectile : MonoBehaviour
    {
        public abstract void Fire();

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(Tags.OBSTACLE)) Destroy(this.gameObject);
        }
    }
}