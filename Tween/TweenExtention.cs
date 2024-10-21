using UnityEngine;
using UnityEngine.UI;

namespace Helper.Tween
{
    public static class TweenExtension
    {
        #region Transform

        #region Move
        public static Tween DoMove(this Transform target, Vector3 endPos, float duration)
        {
            Tween tween = new Tween(() => target.localPosition, pos => target.localPosition = pos, endPos, duration);
            tween.Start();
            return tween;
        }

        public static Tween DoMoveX(this Transform target, float endValue, float duration)
        {
            Vector3 endPos = new Vector3(endValue, target.localPosition.y, target.localPosition.z);
            Tween tween = new Tween(() => target.localPosition, pos => target.localPosition = pos, endPos, duration);
            tween.Start();
            return tween;
        }

        public static Tween DoMoveY(this Transform target, float endValue, float duration)
        {
            Vector3 endPos = new Vector3(target.localPosition.x, endValue, target.localPosition.z);
            Tween tween = new Tween(() => target.localPosition, pos => target.localPosition = pos, endPos, duration);
            tween.Start();
            return tween;
        }

        public static Tween DoMoveZ(this Transform target, float endValue, float duration)
        {
            Vector3 endPos = new Vector3(target.localPosition.x, target.localPosition.y, endValue);
            Tween tween = new Tween(() => target.localPosition, pos => target.localPosition = pos, endPos, duration);
            tween.Start();
            return tween;
        }
        #endregion

        #region Rotate
        public static Tween DoRotate(this Transform target, Vector3 endValue, float duration)
        {
            Tween tween = new Tween(() => target.localEulerAngles, rot => target.localEulerAngles = rot, endValue, duration);
            tween.Start();
            return tween;
        }

        public static Tween DoRotateX(this Transform target, float endValue, float duration)
        {
            Vector3 _endValue = new Vector3(endValue, target.localEulerAngles.y, target.localEulerAngles.z);
            Tween tween = new Tween(() => target.localEulerAngles, rot => target.localEulerAngles = rot, _endValue, duration);
            tween.Start();
            return tween;
        }

        public static Tween DoRotateY(this Transform target, float endValue, float duration)
        {
            Vector3 _endValue = new Vector3(target.localEulerAngles.x, endValue, target.localEulerAngles.z);
            Tween tween = new Tween(() => target.localEulerAngles, rot => target.localEulerAngles = rot, _endValue, duration);
            tween.Start();
            return tween;
        }

        public static Tween DoRotateZ(this Transform target, float endValue, float duration)
        {
            Vector3 _endValue = new Vector3(target.localEulerAngles.x, target.localEulerAngles.y, endValue);
            Tween tween = new Tween(() => target.localEulerAngles, rot => target.localEulerAngles = rot, _endValue, duration);
            tween.Start();
            return tween;
        }
        #endregion

        #region Scale
        public static Tween DoScale(this Transform target, Vector3 endValue, float duration)
        {
            Tween tween = new Tween(() => target.localScale, scale => target.localScale = scale, endValue, duration);
            tween.Start();
            return tween;
        }
        #endregion

        #endregion

        #region UI
        public static Tween DoAlpha(this CanvasRenderer target, float endValue, float duration)
        {
            Tween tween = new Tween(() => target.GetAlpha(), target.SetAlpha, endValue, duration);
            tween.Start();
            return tween;
        }

        public static Tween DoColor(this Image target, Color endColor, float duration)
        {
            Tween tween = new Tween(() => target.color, color => target.color = color, endColor, duration);
            tween.Start();
            return tween;
        }

        #region Rect Transform
        public static Tween DoAnchorPos(this RectTransform target, Vector2 endValue, float duration)
        {
            Tween tween = new Tween(() => target.anchoredPosition, value => target.anchoredPosition = value, endValue, duration);
            tween.Start();
            return tween;
        }

        public static Tween HideUp(this RectTransform target, float duration)
        {
            Vector2 offScreenPos = new Vector2(target.anchoredPosition.x, target.sizeDelta.y);
            Tween tween = new Tween(() => target.anchoredPosition, pos => target.anchoredPosition = pos, offScreenPos, duration);
            tween.Start();
            return tween;
        }

        public static Tween HideDown(this RectTransform target, float duration)
        {
            Vector2 offScreenPos = new Vector2(target.anchoredPosition.x, -target.sizeDelta.y);
            Tween tween = new Tween(() => target.anchoredPosition, pos => target.anchoredPosition = pos, offScreenPos, duration);
            tween.Start();
            return tween;
        }

        public static Tween HideLeft(this RectTransform target, float duration)
        {
            Vector2 offScreenPos = new Vector2(-target.sizeDelta.x, target.anchoredPosition.y);
            Tween tween = new Tween(() => target.anchoredPosition, pos => target.anchoredPosition = pos, offScreenPos, duration);
            tween.Start();
            return tween;
        }

        public static Tween HideRight(this RectTransform target, float duration)
        {
            Vector2 offScreenPos = new Vector2(target.sizeDelta.x, target.anchoredPosition.y);
            Tween tween = new Tween(() => target.anchoredPosition, pos => target.anchoredPosition = pos, offScreenPos, duration);
            tween.Start();
            return tween;
        }

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
        public static Tween ShakePosition(this Transform target, float duration, float intensity, int speed = 3)
        {
            int _speed = speed * 10;
            Tween tween = new Tween(() => target.localPosition, pos => target.localPosition = pos, Vector3.one, duration, intensity, _speed);
            tween.Start();
            return tween;
        }

        public static Tween ShakePosition(this Transform target, float duration, float intensity, Vector3 axisToImpact, int speed = 3)
        {
            int _speed = speed * 10;
            Tween tween = new Tween(() => target.localPosition, pos => target.localPosition = pos, axisToImpact, duration, intensity, _speed);
            tween.Start();
            return tween;
        }
        #endregion

        #region Punch
        public static Tween DoPunch(this Transform target, float duration, Vector3 punchAmount)
        {
            Tween tween = new Tween(() => target.localScale, size => target.localScale = size, duration, punchAmount);
            tween.Start();
            return tween;
        }
        #endregion
    }

}
