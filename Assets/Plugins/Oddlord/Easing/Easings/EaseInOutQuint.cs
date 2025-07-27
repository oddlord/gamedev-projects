using UnityEngine;

namespace Oddlord.Easing
{
    public class EaseInOutQuint : EasingFunction
    {
        public override void Initialize()
        {
            Curve = EasingCurve.EaseInOutQuint;
        }

        public override float Ease(float x)
        {
            return x < 0.5f ? 16 * x * x * x * x * x : 1 - Mathf.Pow(-2 * x + 2, 5) / 2;
        }
    }
}
