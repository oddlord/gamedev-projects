namespace Oddlord.Easing
{
    public class EaseOutQuad : EasingFunction
    {
        public override void Initialize()
        {
            Curve = EasingCurve.EaseOutQuad;
        }

        public override float Ease(float x)
        {
            return 1 - (1 - x) * (1 - x);
        }
    }
}
