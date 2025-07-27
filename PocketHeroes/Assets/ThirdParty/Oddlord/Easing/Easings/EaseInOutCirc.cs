using UnityEngine;

namespace Oddlord.Easing
{
    public class EaseInOutCirc : EasingFunction
    {
        public override void Initialize()
        {
            Curve = EasingCurve.EaseInOutCirc;
        }

        public override float Ease(float x)
        {
            return x < 0.5f
              ? (1 - Mathf.Sqrt(1 - Mathf.Pow(2 * x, 2))) / 2
              : (Mathf.Sqrt(1 - Mathf.Pow(-2 * x + 2, 2)) + 1) / 2;
        }
    }
}
