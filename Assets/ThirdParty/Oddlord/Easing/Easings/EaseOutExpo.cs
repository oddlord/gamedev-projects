using UnityEngine;

namespace Oddlord.Easing
{
    public class EaseOutExpo : EasingFunction
    {
        public override void Initialize()
        {
            Curve = EasingCurve.EaseOutExpo;
        }

        public override float Ease(float x)
        {
            return x == 1 ? 1 : 1 - Mathf.Pow(2, -10 * x);
        }
    }
}
