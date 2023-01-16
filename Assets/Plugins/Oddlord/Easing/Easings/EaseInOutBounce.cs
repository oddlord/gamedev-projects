using UnityEngine;

namespace Oddlord.Easing
{
    public class EaseInOutBounce : EasingFunction
    {
        private EaseOutBounce _easeOutBounce;

        public override void Initialize()
        {
            Curve = EasingCurve.EaseInOutBounce;

            _easeOutBounce = new EaseOutBounce();
            _easeOutBounce.Initialize();
        }

        public override float Ease(float x)
        {
            return x < 0.5f
              ? (1 - _easeOutBounce.Ease(1 - 2 * x)) / 2
              : (1 + _easeOutBounce.Ease(2 * x - 1)) / 2;
        }
    }
}
