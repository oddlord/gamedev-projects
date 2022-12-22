using System;
using UnityEngine;

namespace SpaceMiner
{
    public abstract class Actor : MonoBehaviour
    {
        public abstract void HandleForwardInput(float amount);
        public abstract void HandleSideInput(float amount);
        public abstract void Attack();
    }
}
