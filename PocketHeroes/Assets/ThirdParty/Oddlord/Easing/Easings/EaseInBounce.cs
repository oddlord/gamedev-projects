namespace Oddlord.Easing
{
    public class EaseInBounce : EasingFunction
    {
        private EaseOutBounce _easeOutBounce;

        public override void Initialize()
        {
            Curve = EasingCurve.EaseInBounce;

            _easeOutBounce = new EaseOutBounce();
            _easeOutBounce.Initialize();
        }

        public override float Ease(float x)
        {
            return 1 - _easeOutBounce.Ease(1 - x);
        }
    }
}
