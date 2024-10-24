using UnityEngine;

namespace Helper.Tween
{
    internal class TweenRunner : MonoBehaviour
    {
        private static TweenRunner _instance;
        internal static TweenRunner Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject obj = new GameObject("TweenRunner");
                    _instance = obj.AddComponent<TweenRunner>();
                    DontDestroyOnLoad(obj);
                }
                return _instance;
            }
        }
    }
}
