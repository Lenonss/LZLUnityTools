using System.ComponentModel;
using UnityEngine;

namespace LZLUnityTool.Plugins.InventoryPlugin.System.TaskSystem.Scripts.SO.Charactor
{
    [CreateAssetMenu(fileName = "enemy race", menuName = "LZLUnityTool/ScriptableObjects/敌人/敌人种族", order = 1)]
    public class EnemyRace : ScriptableObject
    {
        [SerializeField]
#if UNITY_EDITOR
        [Header("识别码")]
#endif
        private string _ID;
        public string ID
        {
            get
            {
                return _ID;
            }
        }

        [SerializeField]
#if UNITY_EDITOR
        [Header("种群名")]
#endif
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