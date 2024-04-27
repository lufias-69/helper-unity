using System.Collections.Generic;
using UnityEngine;

namespace Helper.Extension
{
    public static class Extension
    {
        #region Transform
        public static void ResetTransform(this Transform transform)
        {
            transform.localScale = Vector3.one;
            transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        }

        #region Child
        public static void KillAllChild(this Transform transform)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if(Application.isPlaying) Object.Destroy(transform.GetChild(i).gameObject);
                else Object.DestroyImmediate(transform.GetChild(i).gameObject);
            }
        }

        public static List<Transform> GetAllChild(this Transform transform)
        {
            List<Transform> list = new List<Transform>(transform.childCount);
            for (int i = 0; i < transform.childCount; i++)
            {
                list.Add(transform.GetChild(i));
            }

            return list;
        }
        #endregion

        #region Transform Position

        public static void SetPosX(this Transform transform, float x)
        {
            transform.localPosition = new Vector3(x, transform.localPosition.y, transform.localPosition.z);
        }

        public static void SetPosY(this Transform transform, float y)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, y, transform.localPosition.z);
        }

        public static void SetPosZ(this Transform transform, float z)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, z);
        }

        //public static void SetPosXY(this Transform transform, float x, float y)
        //{
        //    transform.position = new Vector3(x,y, transform.position.z);
        //}

        //public static void SetPosXZ(this Transform transform, float x, float z)
        //{
        //    transform.position = new Vector3(x, transform.position.y, z);
        //}

        //public static void SetPosYZ(this Transform transform, float y, float z)
        //{
        //    transform.position = new Vector3(transform.position.x, y, z);
        //}

        public static void ResetPos(this Transform transform)
        {
            transform.localPosition = Vector3.zero;
        }

        #endregion

        #region Transform Rotation
        public static void SetRotX(this Transform transform, float x)
        {
            Quaternion q = Quaternion.Euler(x, transform.localRotation.eulerAngles.y,
                transform.localRotation.eulerAngles.z);

            transform.localRotation = q;
        }

        public static void SetRotY(this Transform transform, float y)
        {
            Quaternion q = Quaternion.Euler(transform.localRotation.eulerAngles.x, y,
                transform.localRotation.eulerAngles.z);

            transform.localRotation = q;
        }

        public static void SetRotZ(this Transform transform, float z)
        {
            Quaternion q = Quaternion.Euler(transform.localRotation.eulerAngles.x,
                transform.localRotation.eulerAngles.y, z);

            transform.localRotation = q;
        }

        public static void ResetRot(this Transform transform)
        {
            transform.localRotation = Quaternion.identity;
        }

        #endregion

        #region Transform Scale

        public static void SetScaleX(this Transform transform, float x)
        {
            transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
        }

        public static void SetScaleY(this Transform transform, float y)
        {
            transform.localScale = new Vector3(transform.localScale.x, y, transform.localScale.z);
        }

        public static void SetScaleZ(this Transform transform, float z)
        {
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, z);
        }

        public static void ResetScale(this Transform transform)
        {
            transform.localPosition = Vector3.one;
        }

        #endregion

        #endregion
    }
}
