using UnityEngine;

namespace Oddlord.Easing
{
    public class EaseOutQuart : EasingFunction
    {
        public override void Initialize()
        {
            Curve = EasingCurve.EaseOutQuart;
        }

        public override float Ease(float x)
        {
            return 1 - Mathf.Pow(1 - x, 4);
        }
    }
}
