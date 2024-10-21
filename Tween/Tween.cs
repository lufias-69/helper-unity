using UnityEngine;
using System.Collections;
using System;

namespace Helper.Tween
{
    #region Core
    public enum TweenType
    {
        Vector3, Vector2, Float, Color, Shake, Punch
    }

    public class Tween
    {
        private Coroutine coroutine;
        private bool isPlaying = false;

        private Func<float> getFloatValue;
        private Action<float> setFloatValue;
        private Func<Vector3> getVector3Value;
        private Action<Vector3> setVector3Value;
        private Func<Vector2> getVector2Value;
        private Action<Vector2> setVector2Value;
        private Func<Color> getColorValue;
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
        public Tween(Func<Vector3> getValue, Action<Vector3> setValue, Vector3 axisToImpact, float duration, float power, float speed)
        {
            this.getVector3Value = getValue;
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
        public Tween(Func<Vector3> getValue, Action<Vector3> setValue, float duration, Vector3 punchAmount)
        {
            this.getVector3Value = getValue;
            this.setVector3Value = setValue;
            this.startVector3 = getValue();
            this.axis = punchAmount;
            this.duration = duration;
            this.easeType = EaseType.Linear;
            this.type = TweenType.Punch;
        }

        // Constructor for float values (like alpha)
        public Tween(Func<float> getValue, Action<float> setValue, float endValue, float duration)
        {
            this.getFloatValue = getValue;
            this.setFloatValue = setValue;
            this.endValue = endValue;
            this.duration = duration;
            this.startValue = getValue();
            this.type = TweenType.Float;
            this.easeType = EaseType.Linear;
        }

        // Constructor for Vector3 values (like position, rotation, scale)
        public Tween(Func<Vector3> getValue, Action<Vector3> setValue, Vector3 endValue, float duration, TweenType tweenType)
        {
            this.getVector3Value = getValue;
            this.setVector3Value = setValue;
            this.endVector3 = endValue;
            this.duration = duration;
            this.startVector3 = getValue();
            this.type = tweenType;
            this.easeType = EaseType.Linear;
        }

        // Constructor for Vector2 values (like anchoredPosition)
        public Tween(Func<Vector2> getValue, Action<Vector2> setValue, Vector2 endValue, float duration)
        {
            this.getVector2Value = getValue;
            this.setVector2Value = setValue;
            this.endVector2 = endValue;
            this.duration = duration;
            this.startVector2 = getValue();
            this.type = TweenType.Vector2;
            this.easeType = EaseType.Linear;
        }

        // Constructor for Color values (like Image color)
        public Tween(Func<Color> getValue, Action<Color> setValue, Color endValue, float duration)
        {
            this.getColorValue = getValue;
            this.setColorValue = setValue;
            this.endColor = endValue;
            this.duration = duration;
            this.startColor = getValue();
            this.type = TweenType.Color;
            this.easeType = EaseType.Linear;
        }

        public bool IsPlaying => isPlaying;

        public Tween SetDelay(float delay)
        {
            this.delay = delay;
            return this;
        }

        public Tween SetEase(EaseType ease)
        {
            this.easeType = ease;
            return this;
        }

        public Tween OnComplete(Action onComplete)
        {
            this.onComplete = onComplete;
            return this;
        }

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

        public void Start()
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
                }

                yield return null;
            }

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

            isPlaying = false;
            onComplete?.Invoke();
        }
    }
    #endregion

    #region Virtual
    public static class DoVirtual
    {
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
        private Action<float> onUpdate;
        private Action onComplete;

        public VirtualTween(float start, float end, float duration, Action<float> onUpdate)
        {
            this.startValue = start;
            this.endValue = end;
            this.duration = duration;
            this.onUpdate = onUpdate;
        }

        public VirtualTween OnComplete(Action onComplete)
        {
            this.onComplete = onComplete;
            return this;
        }

        public void Start()
        {
            TweenRunner.Instance.StartCoroutine(Execute());
        }

        private IEnumerator Execute()
        {
            float time = 0f;

            while (time < duration)
            {
                time += Time.deltaTime;
                float t = Mathf.Clamp01(time / duration);
                float value = Mathf.Lerp(startValue, endValue, t);

                try
                {
                    onUpdate?.Invoke(value);
                }
                catch (Exception) { yield break; }

                yield return null;
            }

            try
            {
                onUpdate?.Invoke(endValue);
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

        public ButtonTween(Func<Vector2> getValue, Action<Vector2> setValue, float duration, float size)
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

        public void Start()
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

                setValue(startValue + offset);

                yield return null;
            }

            setValue(startValue);
            isPlaying = false;
            onComplete?.Invoke();
        }
    }

    public class TweenEffects
    {
        private Coroutine coroutine;
        private bool isPlaying = false;

        private Func<Vector3> getVector3Value;
        private Action<Vector3> setVector3Value;
        public float duration;
        public float intensity;
        public float speed;
        private float delay;
        private EaseType easeType;
        private Action onComplete;
        private Vector3 startVector3;

        public TweenEffects(Func<Vector3> getValue, Action<Vector3> setValue, Vector3 originalValue, float duration, float intensity, float speed)
        {
            this.getVector3Value = getValue;
            this.setVector3Value = setValue;
            this.duration = duration;
            this.speed = speed;
            this.intensity = intensity;
            this.startVector3 = originalValue;
            this.easeType = EaseType.Linear;
        }

        public bool IsPlaying => isPlaying;

        public void Start()
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

            while (time < duration)
            {
                float t = time / duration;
                float easedT = easeFunction(t);

                // Use simple sine/cosine for shake effect
                float shakeX = Mathf.Sin(time * speed) * intensity * easedT;
                float shakeY = Mathf.Cos(time * speed) * intensity * easedT;

                Vector3 offset = new Vector3(shakeX, shakeY, 0);
                setVector3Value(startVector3 + offset);

                time += Time.deltaTime;
                yield return null;
            }

            // Reset position back to original value
            setVector3Value(startVector3);
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

    public static class EasePresets
    {
        public static Func<float, float> GetEaseFunction(EaseType easeType)
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
