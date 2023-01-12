namespace Oddlord.Easing
{
    public class EaseInQuart : EasingFunction
    {
        public override void Initialize()
        {
            Curve = EasingCurve.EaseInQuart;
        }

        public override float Ease(float x)
        {
            return x * x * x * x;
        }
    }
}
