using UnityEngine;

namespace SpaceMiner
{
    public abstract class ActorController : MonoBehaviour
    {
        [SerializeField] protected Actor _actor;

        public void SetActor(Actor actor)
        {
            _actor = actor;
        }
    }
}