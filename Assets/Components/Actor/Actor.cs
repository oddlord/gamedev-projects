using System;
using UnityEngine;

namespace SpaceMiner
{
    public abstract class Actor : MonoBehaviour
    {
        public Action<Actor> OnDeath;

        public abstract void HandleForwardInput(float amount);
        public abstract void HandleSideInput(float amount);
        public abstract void Attack();
    }
}
