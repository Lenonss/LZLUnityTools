using LZLUnityTool.Plugins.InvntoryPlugin.Loot.SO;
using UnityEngine;

namespace LZLUnityTool.Plugins.InventoryPlugin.System.TaskSystem.Scripts.SO.Charactor
{
    [CreateAssetMenu(fileName = "enemy info", menuName = "LZLUnityTool/ScriptableObjects/敌人/敌人信息", order = 0)]
    public class EnemyInformation : CharacterInformation
    {
        [SerializeField]
        private EnemyRace race;
        public EnemyRace Race => race;

        [SerializeField, NonReorderable]
        private ProductInformation dropItems;
        public ProductInformation DropItems
        {
            get
            {
                return dropItems;
            }
        }

        [SerializeField]
        private GameObject lootPrefab;
        public GameObject LootPrefab
        {
            get
            {
                return lootPrefab;
            }
        }
    }
}