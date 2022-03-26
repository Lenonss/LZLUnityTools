using UnityEditor;
using UnityEngine;

namespace LZLUnityTool.Plugins.CommonPlugin.Tools
{
    public class PlayerPrefsEditorManager
    {
        [MenuItem("LZLUnityTool/Data/PlayerPrefs/Delete All")]
        static void DeletePlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
            Debug.Log("All PlayerPrefs deleted");
        }

        [MenuItem("LZLUnityTool/Data/PlayerPrefs/Save All")]
        static void SavePlayerPrefs()
        {
            PlayerPrefs.Save();
            Debug.Log("PlayerPrefs saved");
        }
    }
}