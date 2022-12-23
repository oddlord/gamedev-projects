using UnityEngine;

namespace Oddlord.Easing
{
    public class EaseOutCubic : EasingFunction
    {
        public override void Initialize()
        {
            Curve = EasingCurve.EaseOutCubic;
        }

        public override float Ease(float x)
        {
            return 1 - Mathf.Pow(1 - x, 3);
        }
    }
}
