using UnityEngine;

namespace Oddlord.Easing
{
    public class EaseInOutQuart : EasingFunction
    {
        public override void Initialize()
        {
            Curve = EasingCurve.EaseInOutQuart;
        }

        public override float Ease(float x)
        {
            return x < 0.5 ? 8 * x * x * x * x : 1 - Mathf.Pow(-2 * x + 2, 4) / 2;
        }
    }
}
