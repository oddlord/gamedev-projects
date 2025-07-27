namespace Oddlord.Easing
{
    public class EaseInBack : EasingFunction
    {
        public override void Initialize()
        {
            Curve = EasingCurve.EaseInBack;
        }

        public override float Ease(float x)
        {
            float c1 = 1.70158f;
            float c3 = c1 + 1;

            return c3 * x * x * x - c1 * x * x;
        }
    }
}
