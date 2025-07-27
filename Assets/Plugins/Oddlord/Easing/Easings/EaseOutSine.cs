using UnityEngine;

namespace Oddlord.Easing
{
    public class EaseOutSine : EasingFunction
    {
        public override void Initialize()
        {
            Curve = EasingCurve.EaseOutSine;
        }

        public override float Ease(float x)
        {
            return Mathf.Sin((x * Mathf.PI) / 2);
        }
    }
}
