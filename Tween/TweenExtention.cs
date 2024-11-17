using UnityEngine;
using UnityEngine.UI;

namespace Helper.Tween
{
    public static class TweenExtension
    {
        #region Transform

        #region Move

        #region Global
        /// <summary>
        /// Move the target to the end position in the given duration
        /// </summary>
        /// <param name="target">The target transform</param>
        /// <param name="endPos">The end position</param>
        /// <param name="duration">The duration of the tween</param>
        /// <returns>The tween object</returns>
        public static Tween DoMove(this Transform target, Vector3 endPos, float duration)
        {
            Tween tween = new Tween(() => target.position, pos => target.position = pos, endPos, duration, TweenType.Vector3);
            tween.Start();
            return tween;
        }

        /// <summary>
        /// Move the target to the end position on the X axis in the given duration
        /// </summary>
        /// <param name="target">The target transform</param>
        /// <param name="endValue">The end position on the X axis</param>
        /// <param name="duration">The duration of the tween</param>
        /// <returns>The tween object</returns>
        public static Tween DoMoveX(this Transform target, float endValue, float duration)
        {
            Vector3 endPos = new Vector3(endValue, target.position.y, target.position.z);
            Tween tween = new Tween(() => target.position, pos => target.position = pos, endPos, duration, TweenType.Vector3);
            tween.Start();
            return tween;
        }

        /// <summary>
        /// Move the target to the end position on the Y axis in the given duration
        /// </summary>
        /// <param name="target">The target transform</param>
        /// <param name="endValue">The end position on the Y axis</param>
        /// <param name="duration">The duration of the tween</param>
        /// <returns>The tween object</returns>
        public static Tween DoMoveY(this Transform target, float endValue, float duration)
        {
            Vector3 endPos = new Vector3(target.position.x, endValue, target.position.z);
            Tween tween = new Tween(() => target.position, pos => target.position = pos, endPos, duration, TweenType.Vector3);
            tween.Start();
            return tween;
        }

        /// <summary>
        /// Move the target to the end position on the Z axis in the given duration
        /// </summary>
        /// <param name="target">The target transform</param>
        /// <param name="endValue">The end position on the Z axis</param>
        /// <param name="duration">The duration of the tween</param>
        /// <returns>The tween object</returns>
        public static Tween DoMoveZ(this Transform target, float endValue, float duration)
        {
            Vector3 endPos = new Vector3(target.position.x, target.position.y, endValue);
            Tween tween = new Tween(() => target.position, pos => target.position = pos, endPos, duration, TweenType.Vector3);
            tween.Start();
            return tween;
        }
        #endregion

        #region Local
        /// <summary>
        /// Move the target to the local end position in the given duration
        /// </summary>
        /// <param name="target">The target transform</param>
        /// <param name="endPos">The end position</param>
        /// <param name="duration">The duration of the tween</param>
        /// <returns>The tween object</returns>
        public static Tween DoLocalMove(this Transform target, Vector3 endPos, float duration)
        {
            Tween tween = new Tween(() => target.localPosition, pos => target.localPosition = pos, endPos, duration, TweenType.Vector3);
            tween.Start();
            return tween;
        }

        /// <summary>
        /// Move the target to the local end position on the X axis in the given duration
        /// </summary>
        /// <param name="target">The target transform</param>
        /// <param name="endValue">The end position on the X axis</param>
        /// <param name="duration">The duration of the tween</param>
        /// <returns>The tween object</returns>
        public static Tween DoLocalMoveX(this Transform target, float endValue, float duration)
        {
            Vector3 endPos = new Vector3(endValue, target.localPosition.y, target.localPosition.z);
            Tween tween = new Tween(() => target.localPosition, pos => target.localPosition = pos, endPos, duration, TweenType.Vector3);
            tween.Start();
            return tween;
        }

        /// <summary>
        /// Move the target to the local end position on the Y axis in the given duration
        /// </summary>
        /// <param name="target">The target transform</param>
        /// <param name="endValue">The end position on the Y axis</param>
        /// <param name="duration">The duration of the tween</param>
        /// <returns>The tween object</returns>
        public static Tween DoLocalMoveY(this Transform target, float endValue, float duration)
        {
            Vector3 endPos = new Vector3(target.localPosition.x, endValue, target.localPosition.z);
            Tween tween = new Tween(() => target.localPosition, pos => target.localPosition = pos, endPos, duration, TweenType.Vector3);
            tween.Start();
            return tween;
        }

        /// <summary>
        /// Move the target to the local end position on the Z axis in the given duration
        /// </summary>
        /// <param name="target">The target transform</param>
        /// <param name="endValue">The end position on the Z axis</param>
        /// <param name="duration">The duration of the tween</param>
        /// <returns>The tween object</returns>
        public static Tween DoLocalMoveZ(this Transform target, float endValue, float duration)
        {
            Vector3 endPos = new Vector3(target.localPosition.x, target.localPosition.y, endValue);
            Tween tween = new Tween(() => target.localPosition, pos => target.localPosition = pos, endPos, duration, TweenType.Vector3);
            tween.Start();
            return tween;
        }
        #endregion

        #endregion

        #region Rotate

        #region Global
        /// <summary>
        /// Rotate the target to the end rotation in the given duration
        /// </summary>
        /// <param name="target">The target transform</param>
        /// <param name="endValue">The end rotation</param>
        /// <param name="duration">The duration of the tween</param>
        /// <returns>The tween object</returns>
        public static Tween DoRotate(this Transform target, Vector3 endValue, float duration)
        {
            Tween tween = new Tween(() => target.eulerAngles, rot => target.eulerAngles = rot, endValue, duration, TweenType.Vector3);
            tween.Start();
            return tween;
        }

        /// <summary>
        /// Rotate the target to the end rotation on the X axis in the given duration
        /// </summary>
        /// <param name="target">The target transform</param>
        /// <param name="endValue">The end rotation on the X axis</param>
        /// <param name="duration">The duration of the tween</param>
        /// <returns>The tween object</returns>
        public static Tween DoRotateX(this Transform target, float endValue, float duration)
        {
            Vector3 _endValue = new Vector3(endValue, target.eulerAngles.y, target.eulerAngles.z);
            Tween tween = new Tween(() => target.eulerAngles, rot => target.eulerAngles = rot, _endValue, duration, TweenType.Vector3);
            tween.Start();
            return tween;
        }

        /// <summary>
        /// Rotate the target to the end rotation on the Y axis in the given duration
        /// </summary>
        /// <param name="target">The target transform</param>
        /// <param name="endValue">The end rotation on the Y axis</param>
        /// <param name="duration">The duration of the tween</param>
        /// <returns>The tween object</returns>
        public static Tween DoRotateY(this Transform target, float endValue, float duration)
        {
            Vector3 _endValue = new Vector3(target.eulerAngles.x, endValue, target.eulerAngles.z);
            Tween tween = new Tween(() => target.eulerAngles, rot => target.eulerAngles = rot, _endValue, duration, TweenType.Vector3);
            tween.Start();
            return tween;
        }

        /// <summary>
        /// Rotate the target to the end rotation on the Z axis in the given duration
        /// </summary>
        /// <param name="target">The target transform</param>
        /// <param name="endValue">The end rotation on the Z axis</param>
        /// <param name="duration">The duration of the tween</param>
        /// <returns>The tween object</returns>
        public static Tween DoRotateZ(this Transform target, float endValue, float duration)
        {
            Vector3 _endValue = new Vector3(target.eulerAngles.x, target.eulerAngles.y, endValue);
            Tween tween = new Tween(() => target.eulerAngles, rot => target.eulerAngles = rot, _endValue, duration, TweenType.Vector3);
            tween.Start();
            return tween;
        }
        #endregion

        #region Local
        /// <summary>
        /// Rotate the target to the local end rotation in the given duration
        /// </summary>
        /// <param name="target">The target transform</param>
        /// <param name="endValue">The end rotation</param>
        /// <param name="duration">The duration of the tween</param>
        /// <returns>The tween object</returns>
        public static Tween DoLocalRotate(this Transform target, Vector3 endValue, float duration)
        {
            Tween tween = new Tween(() => target.localEulerAngles, rot => target.localEulerAngles = rot, endValue, duration, TweenType.Vector3);
            tween.Start();
            return tween;
        }

        /// <summary>
        /// Rotate the target to the local end rotation on the X axis in the given duration
        /// </summary>
        /// <param name="target">The target transform</param>
        /// <param name="endValue">The end rotation on the X axis</param>
        /// <param name="duration">The duration of the tween</param>
        /// <returns>The tween object</returns>
        public static Tween DoLocalRotateX(this Transform target, float endValue, float duration)
        {
            Vector3 _endValue = new Vector3(endValue, target.localEulerAngles.y, target.localEulerAngles.z);
            Tween tween = new Tween(() => target.localEulerAngles, rot => target.localEulerAngles = rot, _endValue, duration, TweenType.Vector3);
            tween.Start();
            return tween;
        }

        /// <summary>
        /// Rotate the target to the local end rotation on the Y axis in the given duration
        /// </summary>
        /// <param name="target">The target transform</param>
        /// <param name="endValue">The end rotation on the Y axis</param>
        /// <param name="duration">The duration of the tween</param>
        /// <returns>The tween object</returns>
        public static Tween DoLocalRotateY(this Transform target, float endValue, float duration)
        {
            Vector3 _endValue = new Vector3(target.localEulerAngles.x, endValue, target.localEulerAngles.z);
            Tween tween = new Tween(() => target.localEulerAngles, rot => target.localEulerAngles = rot, _endValue, duration, TweenType.Vector3);
            tween.Start();
            return tween;
        }

        /// <summary>
        /// Rotate the target to the local end rotation on the Z axis in the given duration
        /// </summary>
        /// <param name="target">The target transform</param>
        /// <param name="endValue">The end rotation on the Z axis</param>
        /// <param name="duration">The duration of the tween</param>
        /// <returns>The tween object</returns>
        public static Tween DoLocalRotateZ(this Transform target, float endValue, float duration)
        {
            Vector3 _endValue = new Vector3(target.localEulerAngles.x, target.localEulerAngles.y, endValue);
            Tween tween = new Tween(() => target.localEulerAngles, rot => target.localEulerAngles = rot, _endValue, duration, TweenType.Vector3);
            tween.Start();
            return tween;
        }
        #endregion

        #endregion

        #region Scale
        /// <summary>
        /// Scale the target to the end value in the given duration
        /// </summary>
        /// <param name="target">The target transform</param>
        /// <param name="endValue">The end scale value</param>
        /// <param name="duration">The duration of the tween</param>
        /// <returns>The tween object</returns>
        public static Tween DoScale(this Transform target, Vector3 endValue, float duration)
        {
            Tween tween = new Tween(() => target.localScale, scale => target.localScale = scale, endValue, duration, TweenType.Vector3);
            tween.Start();
            return tween;
        }
        #endregion

        #endregion

        #region UI
        #region Other
        /// <summary>
        /// Tween the alpha value of a CanvasGroup to the end value over the given duration.
        /// </summary>
        /// <param name="target">The target CanvasGroup</param>
        /// <param name="endValue">The end alpha value</param>
        /// <param name="duration">The duration of the tween</param>
        /// <returns>The tween object</returns>
        public static Tween DoAlpha(this CanvasGroup target, float endValue, float duration)
        {
            Tween tween = new Tween(() => target.alpha, alpha => target.alpha = alpha, endValue, duration);
            tween.Start();
            return tween;
        }

        /// <summary>
        /// Tween the color of an Image to the end color over the given duration.
        /// </summary>
        /// <param name="target">The target Image</param>
        /// <param name="endColor">The end color value</param>
        /// <param name="duration">The duration of the tween</param>
        /// <returns>The tween object</returns>
        public static Tween DoColor(this Image target, Color endColor, float duration)
        {
            Tween tween = new Tween(() => target.color, color => target.color = color, endColor, duration);
            tween.Start();
            return tween;
        }

        /// <summary>
        /// Tween the fill amount of an Image to the end value over the given duration.
        /// </summary>
        /// <param name="target">The target Image</param>
        /// <param name="endValue">The end fill amount value</param>
        /// <param name="duration">The duration of the tween</param>
        /// <returns>The tween object</returns>
        public static Tween DoFillAmount(this Image target, float endValue, float duration)
        {
            Tween tween = new Tween(() => target.fillAmount, value => target.fillAmount = value, endValue, duration);
            tween.Start();
            return tween;
        }

        /// <summary>
        /// Tween the alpha value of an Image to the end value over the given duration.
        /// </summary>
        /// <param name="target">The target Image</param>
        /// <param name="endValue">The end alpha value</param>
        /// <param name="duration">The duration of the tween</param>
        /// <returns>The tween object</returns>
        public static Tween DoFade(this Image target, float endValue, float duration)
        {
            Color clr = target.color;
            Tween tween = new Tween(() => target.color.a, alpha => { clr.a = alpha; target.color = clr; }, endValue, duration);
            tween.Start();
            return tween;
        }
        #endregion

        #region Rect Transform
        /// <summary>
        /// Tween the anchored position of a RectTransform to the end value over the given duration.
        /// </summary>
        /// <param name="target">The target RectTransform</param>
        /// <param name="endValue">The end anchored position value</param>
        /// <param name="duration">The duration of the tween</param>
        /// <returns>The tween object</returns>
        public static Tween DoAnchorPos(this RectTransform target, Vector2 endValue, float duration)
        {
            Tween tween = new Tween(() => target.anchoredPosition, value => target.anchoredPosition = value, endValue, duration);
            tween.Start();
            return tween;
        }

        /// <summary>
        /// Tween the anchored position of a RectTransform to be off-screen upwards over the given duration.
        /// </summary>
        /// <param name="target">The target RectTransform</param>
        /// <param name="duration">The duration of the tween</param>
        /// <returns>The tween object</returns>
        public static Tween HideUp(this RectTransform target, float duration)
        {
            Vector2 offScreenPos;
            if ((target.anchorMax == Vector2.one * 0.5f && target.anchorMin == Vector2.one * 0.5f) ||
                (target.anchorMax == Vector2.one && target.anchorMin == Vector2.zero))
            {
                offScreenPos = new Vector2(target.anchoredPosition.x, target.GetComponentInParent<Canvas>().pixelRect.height);
            }
            else offScreenPos = new Vector2(target.anchoredPosition.x, target.sizeDelta.y);

            Tween tween = new Tween(() => target.anchoredPosition, pos => target.anchoredPosition = pos, offScreenPos, duration);
            tween.Start();
            return tween;
        }


        /// <summary>
        /// Tween the anchored position of a RectTransform to be off-screen downwards over the given duration.
        /// </summary>
        /// <param name="target">The target RectTransform</param>
        /// <param name="duration">The duration of the tween</param>
        /// <returns>The tween object</returns>
        public static Tween HideDown(this RectTransform target, float duration)
        {
            Vector2 offScreenPos;
            if ((target.anchorMax == Vector2.one * 0.5f && target.anchorMin == Vector2.one * 0.5f) ||
                (target.anchorMax == Vector2.one && target.anchorMin == Vector2.zero))
            {
                offScreenPos = new Vector2(target.anchoredPosition.x, -target.GetComponentInParent<Canvas>().pixelRect.height);
            }
            else offScreenPos = new Vector2(target.anchoredPosition.x, -target.sizeDelta.y);

            Tween tween = new Tween(() => target.anchoredPosition, pos => target.anchoredPosition = pos, offScreenPos, duration);
            tween.Start();
            return tween;
        }


        /// <summary>
        /// Tween the anchored position of a RectTransform to be off-screen to the left over the given duration.
        /// </summary>
        /// <param name="target">The target RectTransform</param>
        /// <param name="duration">The duration of the tween</param>
        /// <returns>The tween object</returns>
        public static Tween HideLeft(this RectTransform target, float duration)
        {
            Vector2 offScreenPos;
            if ((target.anchorMax == Vector2.one * 0.5f && target.anchorMin == Vector2.one * 0.5f) ||
                (target.anchorMax == Vector2.one && target.anchorMin == Vector2.zero))
            {
                offScreenPos = new Vector2(-target.GetComponentInParent<Canvas>().pixelRect.width, target.anchoredPosition.y);
            }
            else offScreenPos = new Vector2(-target.sizeDelta.x, target.anchoredPosition.y);

            Tween tween = new Tween(() => target.anchoredPosition, pos => target.anchoredPosition = pos, offScreenPos, duration);
            tween.Start();
            return tween;
        }


        /// <summary>
        /// Tween the anchored position of a RectTransform to be off-screen to the right over the given duration.
        /// </summary>
        /// <param name="target">The target RectTransform</param>
        /// <param name="duration">The duration of the tween</param>
        /// <returns>The tween object</returns>
        public static Tween HideRight(this RectTransform target, float duration)
        {
            Vector2 offScreenPos;
            if ((target.anchorMax == Vector2.one * 0.5f && target.anchorMin == Vector2.one * 0.5f) ||
                (target.anchorMax == Vector2.one && target.anchorMin == Vector2.zero))
            {
                offScreenPos = new Vector2(target.GetComponentInParent<Canvas>().pixelRect.width, target.anchoredPosition.y);
            }
            else offScreenPos = new Vector2(target.sizeDelta.x, target.anchoredPosition.y);

            Tween tween = new Tween(() => target.anchoredPosition, pos => target.anchoredPosition = pos, offScreenPos, duration);
            tween.Start();
            return tween;
        }


        /// <summary>
        /// Tween the anchored position of a RectTransform to be on-screen over the given duration.
        /// </summary>
        /// <param name="target">The target RectTransform</param>
        /// <param name="duration">The duration of the tween</param>
        /// <param name="offset">Optional offset for the final position</param>
        /// <returns>The tween object</returns>
        public static Tween Show(this RectTransform target, float duration, Vector2? offset = null)
        {
            Vector2 onScreenPos = offset.HasValue ? offset.Value : Vector2.zero;
            Tween tween = new Tween(() => target.anchoredPosition, pos => target.anchoredPosition = pos, onScreenPos, duration);
            tween.Start();
            return tween;
        }
        #endregion

        #endregion

        #region Shake
        /// <summary>
        /// Shake the position of the target transform with the given parameters.
        /// </summary>
        /// <param name="target">The target transform</param>
        /// <param name="duration">The duration of the shake</param>
        /// <param name="intensity">The intensity of the shake</param>
        /// <param name="speed">The speed of the shake</param>
        /// <returns>The tween object</returns>
        public static Tween ShakePosition(this Transform target, float duration, float intensity, int speed = 3)
        {
            int _speed = speed * 10;
            Tween tween = new Tween(() => target.localPosition, pos => target.localPosition = pos, Vector3.one, duration, intensity, _speed);
            tween.Start();
            return tween;
        }

        /// <summary>
        /// Shake the position of the target transform with the given parameters.
        /// </summary>
        /// <param name="target">The target transform</param>
        /// <param name="duration">The duration of the shake</param>
        /// <param name="intensity">The intensity of the shake</param>
        /// <param name="axisToImpact">The axis to impact during the shake</param>
        /// <param name="speed">The speed of the shake</param>
        /// <returns>The tween object</returns>
        public static Tween ShakePosition(this Transform target, float duration, float intensity, Vector3 axisToImpact, int speed = 3)
        {
            int _speed = speed * 10;
            Tween tween = new Tween(() => target.localPosition, pos => target.localPosition = pos, axisToImpact, duration, intensity, _speed);
            tween.Start();
            return tween;
        }
        #endregion

        #region Punch
        /// <summary>
        /// Punch the target's scale with the given parameters.
        /// </summary>
        /// <param name="target">The target transform</param>
        /// <param name="duration">The duration of the punch</param>
        /// <param name="punchAmount">The amount to punch the scale</param>
        /// <returns>The tween object</returns>
        public static Tween DoPunch(this Transform target, float duration, Vector3 punchAmount)
        {
            Tween tween = new Tween(() => target.localScale, size => target.localScale = size, duration, punchAmount);
            tween.Start();
            return tween;
        }
        #endregion

        #region Audio
        /// <summary>
        /// Tween the volume of an AudioSource to the end value over the given duration.
        /// </summary>
        /// <param name="target">The target AudioSource</param>
        /// <param name="volume">The end volume value</param>
        /// <param name="duration">The duration of the tween</param>
        /// <returns>The tween object</returns>
        public static Tween DoFade(this AudioSource target, float volume, float duration)
        {
            Tween tween = new Tween(() => target.volume, vol => target.volume = vol, volume, duration);
            tween.Start();
            return tween;
        }
        #endregion
    }

}
