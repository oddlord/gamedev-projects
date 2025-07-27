using UnityEngine;

namespace Oddlord.Easing
{
    public class EaseInOutCubic : EasingFunction
    {
        public override void Initialize()
        {
            Curve = EasingCurve.EaseInOutCubic;
        }

        public override float Ease(float x)
        {
            return x < 0.5f ? 4 * x * x * x : 1 - Mathf.Pow(-2 * x + 2, 3) / 2;
        }
    }
}
