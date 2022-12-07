
namespace Oddlord.Easing
{
    public abstract class EasingFunction
    {
        public EasingCurve Curve;

        public abstract void Initialize();

        public abstract float Ease(float x);
    }
}
