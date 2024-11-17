using UnityEngine;
using System.Collections;
using System;

namespace Helper.Tween
{
    #region Core
    internal enum TweenType
    {
        Vector3, Vector2, Float, Color, Shake, Punch, Rotation
    }

    public class Tween
    {
        private Coroutine coroutine;
        private bool isPlaying = false;

        private Action<float> setFloatValue;
        private Action<Vector3> setVector3Value;
        private Action<Vector2> setVector2Value;
        private Action<Color> setColorValue;

        private float duration;
        private float delay;
        private EaseType easeType;
        private Action onComplete;
        private Vector3 startVector3;
        private Vector2 startVector2;
        private Color startColor;
        private float startValue;
        private Vector3 endVector3;
        private Vector2 endVector2;
        private Color endColor;
        private float endValue;
        private TweenType type;

        private float power;
        private float speed;
        private Vector3 axis;

        // Constructor for Shake Effect
        internal Tween(Func<Vector3> getValue, Action<Vector3> setValue, Vector3 axisToImpact, float duration, float power, float speed)
        {
            this.setVector3Value = setValue;
            this.startVector3 = getValue();
            this.power = power;
            this.speed = speed;
            this.duration = duration;
            this.axis = axisToImpact;
            this.easeType = EaseType.Linear;
            this.type = TweenType.Shake;
        }

        // Constructor for Punch Effect
        internal Tween(Func<Vector3> getValue, Action<Vector3> setValue, float duration, Vector3 punchAmount)
        {
            this.setVector3Value = setValue;
            this.startVector3 = getValue();
            this.axis = punchAmount;
            this.duration = duration;
            this.easeType = EaseType.Linear;
            this.type = TweenType.Punch;
        }

        // Constructor for float values (like alpha)
        internal Tween(Func<float> getValue, Action<float> setValue, float endValue, float duration)
        {
            this.setFloatValue = setValue;
            this.endValue = endValue;
            this.duration = duration;
            this.startValue = getValue();
            this.type = TweenType.Float;
            this.easeType = EaseType.Linear;
        }

        // Constructor for Vector3 values (like position, rotation, scale)
        internal Tween(Func<Vector3> getValue, Action<Vector3> setValue, Vector3 endValue, float duration, TweenType tweenType)
        {
            this.setVector3Value = setValue;
            this.endVector3 = endValue;
            this.duration = duration;
            this.startVector3 = getValue();
            this.type = tweenType;
            this.easeType = EaseType.Linear;
        }

        // Constructor for Vector2 values (like anchoredPosition)
        internal Tween(Func<Vector2> getValue, Action<Vector2> setValue, Vector2 endValue, float duration)
        {
            this.setVector2Value = setValue;
            this.endVector2 = endValue;
            this.duration = duration;
            this.startVector2 = getValue();
            this.type = TweenType.Vector2;
            this.easeType = EaseType.Linear;
        }

        // Constructor for Color values (like Image color)
        internal Tween(Func<Color> getValue, Action<Color> setValue, Color endValue, float duration)
        {
            this.setColorValue = setValue;
            this.endColor = endValue;
            this.duration = duration;
            this.startColor = getValue();
            this.type = TweenType.Color;
            this.easeType = EaseType.Linear;
        }

        public bool IsPlaying => isPlaying;

        /// <summary>
        /// Sets the delay before the tween starts.
        /// </summary>
        /// <param name="delay">The delay in seconds.</param>
        /// <returns>The current Tween instance.</returns>
        public Tween SetDelay(float delay)
        {
            this.delay = delay;
            return this;
        }

        /// <summary>
        /// Sets the easing function for the tween.
        /// </summary>
        /// <param name="ease">The easing function to use.</param>
        /// <returns>The current Tween instance.</returns>
        public Tween SetEase(EaseType ease)
        {
            this.easeType = ease;
            return this;
        }

        /// <summary>
        /// Sets the action to be called when the tween completes.
        /// </summary>
        /// <param name="onComplete">The action to call on completion.</param>
        /// <returns>The current Tween instance.</returns>
        public Tween OnComplete(Action onComplete)
        {
            this.onComplete = onComplete;
            return this;
        }

        /// <summary>
        /// Kills the tween if it is currently playing.
        /// </summary>
        public void Kill()
        {
            if (coroutine != null)
            {
                if (isPlaying)
                {
                    TweenRunner.Instance.StopCoroutine(coroutine);
                    isPlaying = false;
                }
            }
        }

        internal void Start()
        {
            isPlaying = true;
            coroutine = TweenRunner.Instance.StartCoroutine(Execute());
        }

        private IEnumerator Execute()
        {
            yield return new WaitForEndOfFrame();

            if (delay > 0f)
                yield return new WaitForSeconds(delay);

            float time = 0f;
            Func<float, float> easeFunction = EasePresets.GetEaseFunction(easeType);

            float t, easedT;
            float shakeX, shakeY, shakeZ;
            float valT;
            Vector3 offset;

            while (time < duration)
            {
                time += Time.deltaTime;
                t = Mathf.Clamp01(time / duration);
                easedT = easeFunction(t);

                try
                {
                    switch (type)
                    {
                        case TweenType.Float:
                            setFloatValue(Mathf.Lerp(startValue, endValue, easedT));
                            break;

                        case TweenType.Vector3:
                            setVector3Value(Vector3.Lerp(startVector3, endVector3, easedT));
                            break;

                        case TweenType.Vector2:
                            setVector2Value(Vector2.Lerp(startVector2, endVector2, easedT));
                            break;

                        case TweenType.Color:
                            setColorValue(Color.Lerp(startColor, endColor, easedT));
                            break;

                        case TweenType.Shake:
                            valT = Mathf.Sin(time * speed) * power * easedT;
                            shakeX = axis.x == 0 ? startVector3.x : valT;
                            shakeY = axis.y == 0 ? startVector3.y : valT;
                            shakeZ = axis.z == 0 ? startVector3.z : valT;
                            offset = new Vector3(shakeX, shakeY, 0);
                            setVector3Value(startVector3 + offset);
                            break;

                        case TweenType.Punch:
                            valT = Mathf.Sin(t * Mathf.PI) * (1 - t);
                            offset = axis * valT;

                            setVector3Value(startVector3 + offset);
                            break;

                        case TweenType.Rotation:
                            shakeX = Mathf.LerpAngle(startVector3.x, endVector3.x, easedT);
                            shakeY = Mathf.LerpAngle(startVector3.y, endVector3.y, easedT);
                            shakeZ = Mathf.LerpAngle(startVector3.z, endVector3.z, easedT);

                            setVector3Value(Vector3.Lerp(startVector3, endVector3, easedT));
                            break;
                    }
                }
                catch (Exception)
                {
                    yield break;
                }

                yield return null;
            }

            try
            {
                switch (type)
                {
                    case TweenType.Float:
                        setFloatValue(endValue);
                        break;
                    case TweenType.Vector3:
                        setVector3Value(endVector3);
                        break;
                    case TweenType.Vector2:
                        setVector2Value(endVector2);
                        break;
                    case TweenType.Color:
                        setColorValue(endColor);
                        break;
                    case TweenType.Shake:
                        setVector3Value(startVector3);
                        break;
                    case TweenType.Punch:
                        setVector3Value(startVector3);
                        break;
                }
            }
            catch (Exception) { }

            isPlaying = false;
            onComplete?.Invoke();
        }
    }
    #endregion

    #region Virtual
    public static class DoVirtual
    {
        /// <summary>
        /// Creates a virtual tween for float values.
        /// </summary>
        /// <param name="start">The starting value of the tween.</param>
        /// <param name="end">The ending value of the tween.</param>
        /// <param name="duration">The duration of the tween in seconds.</param>
        /// <param name="onUpdate">The action to call on each update with the current tween value.</param>
        /// <returns>The created VirtualTween instance.</returns>
        public static VirtualTween Float(float start, float end, float duration, Action<float> onUpdate)
        {
            VirtualTween tween = new VirtualTween(start, end, duration, onUpdate);
            tween.Start();
            return tween;
        }
    }

    public class VirtualTween
    {
        private float startValue;
        private float endValue;
        private float duration;
        private float delay;
        private Action<float> onUpdate;
        private Action onComplete;

        internal VirtualTween(float start, float end, float duration, Action<float> onUpdate)
        {
            this.startValue = start;
            this.endValue = end;
            this.duration = duration;
            this.onUpdate = onUpdate;
            delay = 0;
        }

        /// <summary>
        /// Sets the action to be called when the virtual tween completes.
        /// </summary>
        /// <param name="onComplete">The action to call on completion.</param>
        /// <returns>The current VirtualTween instance.</returns>
        public VirtualTween OnComplete(Action onComplete)
        {
            this.onComplete = onComplete;
            return this;
        }

        internal void Start()
        {
            TweenRunner.Instance.StartCoroutine(Execute());
        }

        /// <summary>
        /// Sets the delay before the tween starts.
        /// </summary>
        /// <param name="delay">The delay in seconds.</param>
        /// <returns>The current Tween instance.</returns>
        public VirtualTween SetDelay(float delay)
        {
            this.delay = delay;
            return this;
        }

        private IEnumerator Execute()
        {
            yield return new WaitForEndOfFrame();

            if (delay > 0f) yield return new WaitForSeconds(delay);


            float time = 0f;
            while (time < duration)
            {
                time += Time.deltaTime;
                float t = Mathf.Clamp01(time / duration);
                float value = Mathf.Lerp(startValue, endValue, t);

                try
                {
                    onUpdate(value);
                }
                catch (Exception) { yield break; }

                yield return null;
            }

            try
            {
                onUpdate(endValue);
                onComplete?.Invoke();
            }
            catch (Exception) { }

        }
    }
    #endregion

    #region Other
    public class ButtonTween
    {
        private Coroutine coroutine;
        private bool isPlaying = false;
        private Func<Vector2> getValue;
        private Action<Vector2> setValue;
        private float duration;
        private float size;
        private Vector2 startValue;

        private Action onComplete;

        internal ButtonTween(Func<Vector2> getValue, Action<Vector2> setValue, float duration, float size)
        {
            this.startValue = getValue();
            this.getValue = getValue;
            this.setValue = setValue;
            this.duration = duration;
            this.size = size;
        }

        public ButtonTween OnComplete(Action onComplete)
        {
            this.onComplete = onComplete;
            return this;
        }

        internal void Start()
        {
            if (isPlaying) return;

            isPlaying = true;
            coroutine = TweenRunner.Instance.StartCoroutine(Execute());
        }

        private IEnumerator Execute()
        {
            float time = 0f;
            Func<float, float> easeFunction = EasePresets.GetEaseFunction(EaseType.Linear);

            float t, easedT;
            Vector2 offset;

            while (time < duration)
            {
                time += Time.deltaTime;
                t = Mathf.Clamp01(time / duration);
                easedT = easeFunction(t);

                offset = (1 - t) * Mathf.Sin(t * Mathf.PI) * size * Vector2.one;

                try
                {
                    setValue(startValue + offset);
                }
                catch (Exception) { }
                
                yield return null;
            }

            try
            {
                setValue(startValue);
            }
            catch (Exception) { }

            isPlaying = false;
            onComplete?.Invoke();
        }
    }

    
    #endregion

    #region Ease
    public enum EaseType
    {
        Linear,
        Sine,
        SineIn,
        SineOut,
        Bounce
    }

    internal static class EasePresets
    {
        internal static Func<float, float> GetEaseFunction(EaseType easeType)
        {
            switch (easeType)
            {
                case EaseType.Linear:
                    return t => t;
                case EaseType.Sine:
                    return t => 0.5f - 0.5f * Mathf.Cos(Mathf.PI * t);
                case EaseType.SineIn:
                    return t => 1f - Mathf.Cos(t * Mathf.PI * 0.5f);
                case EaseType.SineOut:
                    return t => Mathf.Sin(t * Mathf.PI * 0.5f);
                case EaseType.Bounce:
                    return t => BounceOut(t);
                default:
                    return t => t;
            }
        }

        private static float BounceOut(float t)
        {
            if (t < (1 / 2.75f))
            {
                return 7.5625f * t * t;
            }
            else if (t < (2 / 2.75f))
            {
                t -= (1.5f / 2.75f);
                return 7.5625f * t * t + 0.75f;
            }
            else if (t < (2.5 / 2.75))
            {
                t -= (2.25f / 2.75f);
                return 7.5625f * t * t + 0.9375f;
            }
            else
            {
                t -= (2.625f / 2.75f);
                return 7.5625f * t * t + 0.984375f;
            }
        }
    }
    #endregion
}
