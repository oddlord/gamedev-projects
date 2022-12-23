using UnityEngine;

namespace SpaceMiner
{
    public class PlayerActorController : ActorController
    {
        void FixedUpdate()
        {
            if (_actor == null) return;

            float verticalInput = Input.GetAxis("Vertical");
            _actor.HandleForwardInput(verticalInput);

            float horizontalInput = Input.GetAxis("Horizontal");
            _actor.HandleSideInput(horizontalInput);

            float fireInput = Input.GetAxis("Fire1");
            if (fireInput > 0) _actor.Attack();
        }
    }
}