using UnityEngine;

namespace Oddlord.Easing
{
    public class EaseInSine : EasingFunction
    {
        public override void Initialize()
        {
            Curve = EasingCurve.EaseInSine;
        }

        public override float Ease(float x)
        {
            return 1 - Mathf.Cos((x * Mathf.PI) / 2);
        }
    }
}
