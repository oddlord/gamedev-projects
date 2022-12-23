using UnityEngine;

namespace Oddlord.Easing
{
    public class EaseOutElastic : EasingFunction
    {
        public override void Initialize()
        {
            Curve = EasingCurve.EaseOutElastic;
        }

        public override float Ease(float x)
        {
            float c4 = 2 * Mathf.PI / 3;
            return x == 0 ? 0 : x == 1 ? 1 : Mathf.Pow(2, -10 * x) * Mathf.Sin((x * 10 - 0.75f) * c4) + 1;
        }
    }
}
