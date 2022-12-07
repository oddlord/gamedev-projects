namespace Oddlord.Easing
{
    public class EaseInQuint : EasingFunction
    {
        public override void Initialize()
        {
            Curve = EasingCurve.EaseInQuint;
        }

        public override float Ease(float x)
        {
            return x * x * x * x * x;
        }
    }
}
