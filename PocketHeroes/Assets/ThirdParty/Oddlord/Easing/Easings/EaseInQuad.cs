namespace Oddlord.Easing
{
    public class EaseInQuad : EasingFunction
    {
        public override void Initialize()
        {
            Curve = EasingCurve.EaseInQuad;
        }

        public override float Ease(float x)
        {
            return x * x;
        }
    }
}
