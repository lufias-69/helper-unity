using UnityEngine;

namespace Helper.Tween
{
    public class TweenRunner : MonoBehaviour
    {
        private static TweenRunner _instance;
        public static TweenRunner Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject obj = new GameObject("SimpleTweenRunner");
                    _instance = obj.AddComponent<TweenRunner>();
                    DontDestroyOnLoad(obj);
                }
                return _instance;
            }
        }
    }
}
