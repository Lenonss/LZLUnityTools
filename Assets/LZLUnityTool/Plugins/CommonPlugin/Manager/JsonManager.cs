using System.IO;
using UnityEngine;

namespace LZLUnityTool.Plugins.CommonPlugin.Manager
{
    public class JsonManager
    {
        private static JsonManager instance;

        public static JsonManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new JsonManager();
                }

                return instance;
            }
        }

        // public static T LoadJsonData<T>(string dataPath)
        // {
        //     T result = default;
        //     if (!File.Exists(dataPath))
        //     {
        //         
        //     }
        // }
    }
}