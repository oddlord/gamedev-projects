using UnityEngine;

namespace Oddlord.Easing
{
    public class EaseOutCirc : EasingFunction
    {
        public override void Initialize()
        {
            Curve = EasingCurve.EaseOutCirc;
        }

        public override float Ease(float x)
        {
            return Mathf.Sqrt(1 - Mathf.Pow(x - 1, 2));
        }
    }
}
