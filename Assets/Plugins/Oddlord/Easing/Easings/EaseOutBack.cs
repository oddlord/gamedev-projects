using UnityEngine;

namespace Oddlord.Easing
{
    public class EaseOutBack : EasingFunction
    {
        public override void Initialize()
        {
            Curve = EasingCurve.EaseOutBack;
        }

        public override float Ease(float x)
        {
            float c1 = 1.70158f;
            float c3 = c1 + 1;

            return 1 + c3 * Mathf.Pow(x - 1, 3) + c1 * Mathf.Pow(x - 1, 2);
        }
    }
}
