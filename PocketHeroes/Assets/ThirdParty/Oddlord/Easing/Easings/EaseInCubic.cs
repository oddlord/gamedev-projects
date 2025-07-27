namespace Oddlord.Easing
{
    public class EaseInCubic : EasingFunction
    {
        public override void Initialize()
        {
            Curve = EasingCurve.EaseInCubic;
        }

        public override float Ease(float x)
        {
            return x * x * x;
        }
    }
}
