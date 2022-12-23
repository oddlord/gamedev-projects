using UnityEngine;

namespace Oddlord.Easing
{
    public class EaseOutQuint : EasingFunction
    {
        public override void Initialize()
        {
            Curve = EasingCurve.EaseOutQuint;
        }

        public override float Ease(float x)
        {
            return 1 - Mathf.Pow(1 - x, 5);
        }
    }
}
