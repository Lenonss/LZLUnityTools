using System.Collections.Generic;
using System.Text;
using LZLUnityTool.Plugins.CommonPlugin.ExtructFunction;
using UnityEngine;

namespace LZLUnityTool.Plugins.InvntoryPlugin.Loot.SO
{
    [CreateAssetMenu(fileName = "product info", menuName = "LZLUnityTool/ScriptableObjects/Goods/产出表")]
    public class ProductInformation : ScriptableObject
    {
        [SerializeField]
        private string remark;

        [SerializeField, NonReorderable]
        private List<DropItemInfo> products = new List<DropItemInfo>();
        public List<DropItemInfo> Products
        {
            get
            {
                return products;
            }
        }

        public bool IsValid => products != null && products.Count > 0;

        public List<ItemInfoBase> DoDrop()
        {
            List<ItemInfoBase> lootItems = new List<ItemInfoBase>();
            foreach (DropItemInfo di in Products)
                if (di.IsValid && CommonTool.Probability(di.DropRate))
                    if (!di.OnlyDropForQuest || (di.OnlyDropForQuest && QuestManager.Instance.HasOngoingQuestWithID(di.BindedQuest.ID)))
                        lootItems.Add(new ItemInfo(di.Item, Random.Range(di.MinAmount, di.MaxAmount + 1)));
            return lootItems;
        }

        public string GetDropInfoString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < Products.Count; i++)
            {
                var di = Products[i];
                if(di.IsValid)
                {
                    sb.Append("[");
                    sb.Append(di.DropRate);
                    sb.Append("%]概率掉落");
                    sb.Append('[');
                    sb.Append(di.ItemName);
                    sb.Append("] ");
                    sb.Append(di.MinAmount);
                    if (di.MinAmount < di.MaxAmount)
                    {
                        sb.Append("~");
                        sb.Append(di.MaxAmount);
                    }
                    sb.Append("个");
                }
                if (i != Products.Count - 1) sb.Append("\n");
            }
            return sb.ToString();
        }
    }
}