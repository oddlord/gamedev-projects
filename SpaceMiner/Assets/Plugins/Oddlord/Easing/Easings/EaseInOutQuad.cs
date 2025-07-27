using UnityEngine;

namespace Oddlord.Easing
{
    public class EaseInOutQuad : EasingFunction
    {
        public override void Initialize()
        {
            Curve = EasingCurve.EaseInOutQuad;
        }

        public override float Ease(float x)
        {
            return x < 0.5f ? 2 * x * x : 1 - Mathf.Pow(-2 * x + 2, 2) / 2;
        }
    }
}
