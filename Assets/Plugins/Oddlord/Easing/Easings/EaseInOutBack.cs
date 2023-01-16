using UnityEngine;

namespace Oddlord.Easing
{
    public class EaseInOutBack : EasingFunction
    {
        public override void Initialize()
        {
            Curve = EasingCurve.EaseInOutBack;
        }

        public override float Ease(float x)
        {
            float c1 = 1.70158f;
            float c2 = c1 * 1.525f;

            return x < 0.5f
              ? Mathf.Pow(2 * x, 2) * ((c2 + 1) * 2 * x - c2) / 2
              : (Mathf.Pow(2 * x - 2, 2) * ((c2 + 1) * (x * 2 - 2) + c2) + 2) / 2;
        }
    }
}
