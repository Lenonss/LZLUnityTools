using UnityEngine;

namespace LZLUnityTool.Plugins.CommonPlugin
{
    public class SingletonMono<T> : MonoBehaviour where T :SingletonMono<T>
    {
        private static T _instance;
        public static T Instance
        {
            get
            {
                if (!_instance || !_instance.gameObject)
                    _instance = FindObjectOfType<T>();
                return _instance;
            }
        }
        public static bool IsIntialized 
        {
            get { return _instance != null; }
        }
    
    }
}
