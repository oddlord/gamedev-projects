using UnityEngine;

namespace SpaceMiner
{
    public class PlayerActorController : MonoBehaviour, IActorController
    {
        public IActor Actor { get; set; }

        void FixedUpdate()
        {
            if (Actor == null) return;

            float verticalInput = Input.GetAxis("Vertical");
            Actor.HandleForwardInput(verticalInput);

            float horizontalInput = Input.GetAxis("Horizontal");
            Actor.HandleSideInput(horizontalInput);

            float fireInput = Input.GetAxis("Fire1");
            if (fireInput > 0) Actor.Attack();
        }
    }
}