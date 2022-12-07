namespace Oddlord.Easing
{
    public class EaseLinear : EasingFunction
    {
        public override void Initialize()
        {
            Curve = EasingCurve.EaseLinear;
        }

        public override float Ease(float x)
        {
            return x;
        }
    }
}
