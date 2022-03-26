using UnityEngine;

namespace LZLUnityTool.Plugins.InventoryPlugin.System.TaskSystem.Scripts.SO
{
    [CreateAssetMenu(fileName = "quest group", menuName = "Zetan Studio/任务/任务组", order = 2)]
    public class QuestGroup : ScriptableObject
    {
        [SerializeField]
        private string _ID;
        public string ID
        {
            get
            {
                return _ID;
            }
        }

        [SerializeField]
        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
        }
    }
}