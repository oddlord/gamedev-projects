using UnityEngine;

namespace Oddlord.Easing
{
    public class EaseInOutSine : EasingFunction
    {
        public override void Initialize()
        {
            Curve = EasingCurve.EaseInOutSine;
        }

        public override float Ease(float x)
        {
            return -(Mathf.Cos(Mathf.PI * x) - 1) / 2;
        }
    }
}
