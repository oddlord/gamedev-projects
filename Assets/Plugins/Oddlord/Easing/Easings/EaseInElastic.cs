using UnityEngine;

namespace Oddlord.Easing
{
    public class EaseInElastic : EasingFunction
    {
        public override void Initialize()
        {
            Curve = EasingCurve.EaseInElastic;
        }

        public override float Ease(float x)
        {
            float c4 = 2 * Mathf.PI / 3;

            return x == 0
              ? 0
              : x == 1
              ? 1
              : -Mathf.Pow(2, 10 * x - 10) * Mathf.Sin((x * 10 - 10.75f) * c4);
        }
    }
}
