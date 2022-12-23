using System.Collections.Generic;
using UnityEngine;

namespace Oddlord.Easing
{
    // Check https://easings.net/
    // for visual examples and more easing functions.
    // To add a new easing, add a new EasingCurve enum, implement a new EasingFunction
    // and instantiate it in the _easings array

    public class Easing
    {
        private static EasingFunction[] _easings = {
            new EaseLinear(),
            new EaseInSine(),
            new EaseOutSine(),
            new EaseInOutSine(),
            new EaseInQuad(),
            new EaseOutQuad(),
            new EaseInOutQuad(),
            new EaseInCubic(),
            new EaseOutCubic(),
            new EaseInOutCubic(),
            new EaseInQuart(),
            new EaseOutQuart(),
            new EaseInOutQuart(),
            new EaseInQuint(),
            new EaseOutQuint(),
            new EaseInOutQuint(),
            new EaseInExpo(),
            new EaseOutExpo(),
            new EaseInOutExpo(),
            new EaseInCirc(),
            new EaseOutCirc(),
            new EaseInOutCirc(),
            new EaseInBack(),
            new EaseOutBack(),
            new EaseInOutBack(),
            new EaseInElastic(),
            new EaseOutElastic(),
            new EaseInOutElastic(),
            new EaseInBounce(),
            new EaseOutBounce(),
            new EaseInOutBounce(),
        };

        private static Dictionary<EasingCurve, EasingFunction> _easingsDict;

        public static float Ease(float x, EasingCurve curve)
        {
            EnsureInitialized();

            x = Mathf.Clamp01(x);
            EasingFunction easing = _easingsDict[curve];
            float easedX = easing.Ease(x);

            return easedX;
        }

        public static float Lerp(float start, float target, float x, EasingCurve curve = EasingCurve.EaseLinear)
        {
            float easedX = Ease(x, curve);
            return Mathf.LerpUnclamped(start, target, easedX);
        }

        public static Vector3 Lerp(Vector3 start, Vector3 target, float x, EasingCurve curve = EasingCurve.EaseLinear)
        {
            float easedX = Ease(x, curve);
            return Vector3.LerpUnclamped(start, target, easedX);
        }

        public static Color Lerp(Color start, Color target, float x, EasingCurve curve = EasingCurve.EaseLinear)
        {
            float easedX = Ease(x, curve);
            return Color.LerpUnclamped(start, target, easedX);
        }

        private static void EnsureInitialized()
        {
            if (_easingsDict != null) return;

            _easingsDict = new Dictionary<EasingCurve, EasingFunction>();
            foreach (EasingFunction easing in _easings)
            {
                easing.Initialize();
                _easingsDict[easing.Curve] = easing;
            }
        }
    }
}