using UnityEngine;

namespace Oddlord.Easing
{
    public class EaseInExpo : EasingFunction
    {
        public override void Initialize()
        {
            Curve = EasingCurve.EaseInExpo;
        }

        public override float Ease(float x)
        {
            return x == 0 ? 0 : Mathf.Pow(2, 10 * x - 10);
        }
    }
}
