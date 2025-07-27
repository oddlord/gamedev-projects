using UnityEngine;

namespace Oddlord.Easing
{
    public class EaseInOutElastic : EasingFunction
    {
        public override void Initialize()
        {
            Curve = EasingCurve.EaseInOutElastic;
        }

        public override float Ease(float x)
        {
            float c5 = 2 * Mathf.PI / 4.5f;

            return x == 0
              ? 0
              : x == 1
              ? 1
              : x < 0.5f
              ? -(Mathf.Pow(2, 20 * x - 10) * Mathf.Sin((20 * x - 11.125f) * c5)) / 2
              : Mathf.Pow(2, -20 * x + 10) * Mathf.Sin((20 * x - 11.125f) * c5) / 2 + 1;
        }
    }
}
