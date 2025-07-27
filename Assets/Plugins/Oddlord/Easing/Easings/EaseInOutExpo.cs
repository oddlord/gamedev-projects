using UnityEngine;

namespace Oddlord.Easing
{
    public class EaseInOutExpo : EasingFunction
    {
        public override void Initialize()
        {
            Curve = EasingCurve.EaseInOutExpo;
        }

        public override float Ease(float x)
        {
            return x == 0
              ? 0
              : x == 1
              ? 1
              : x < 0.5f ? Mathf.Pow(2, 20 * x - 10) / 2
              : (2 - Mathf.Pow(2, -20 * x + 10)) / 2;
        }
    }
}
