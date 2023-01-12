using UnityEngine;

namespace Oddlord.Easing
{
    public class EaseInCirc : EasingFunction
    {
        public override void Initialize()
        {
            Curve = EasingCurve.EaseInCirc;
        }

        public override float Ease(float x)
        {
            return 1 - Mathf.Sqrt(1 - Mathf.Pow(x, 2));
        }
    }
}
