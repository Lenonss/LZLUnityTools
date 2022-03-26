using System.Collections;
using System.Collections.Generic;
using LZLUnityTool.Plugins.InvntoryPlugin.Serialize;
using UnityEngine;

namespace LZLUnityTool.Plugins.InvntoryPlugin
{
    [CreateAssetMenu(menuName = "LZLUnityTool/ScriptableObjects/Inventory/StoreInventory",fileName = "new storeInventorySo")]
    public class StoreInventroy_SO : ScriptableObject
    {
        [Header("上架商品"),SerializeField] private StringBaseGoodsSODic putAwayGoods = new StringBaseGoodsSODic();
        
        [Header("下架商品"),SerializeField] private StringBaseGoodsSODic soldOutGoods = new StringBaseGoodsSODic();
        
        
        /// <summary>
        /// 根据商品id获取商品_上架的
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BaseGoodsSO GetGoodsByID_PutAway(string id)
        {
            BaseGoodsSO result = null;
            if (putAwayGoods.ContainsKey(id))
            {
                result = putAwayGoods[id];
            }

            return result;
        }

        /// <summary>
        /// 获得所有上架商品
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, BaseGoodsSO> GetPutAwayGoods_Dic()
        {
            Dictionary<string, BaseGoodsSO> result = new Dictionary<string, BaseGoodsSO>();
            foreach (var item in putAwayGoods)
            {
                result.Add(item.Key,item.Value);
            }

            return result;
        }
        
        /// <summary>
        /// 获得所有上架商品
        /// </summary>
        /// <returns></returns>
        public List<BaseGoodsSO> GetPutAwayGoods_List()
        {
            List<BaseGoodsSO> result = new List<BaseGoodsSO>();
            foreach (var item in putAwayGoods)
            {
                result.Add(item.Value);
            }

            return result;
        }
        
        /// <summary>
        /// 获得上架商品中所有锁住状态的物品
        /// </summary>
        /// <returns></returns>
        public List<BaseGoodsSO> GetPutAwayGoods_Locked_List()
        {
            List<BaseGoodsSO> result = new List<BaseGoodsSO>();
            foreach (var item in putAwayGoods)
            {
                if (!item.Value.unLockState)
                {
                    result.Add(item.Value);
                }
            }

            return result;
        }
        
        /// <summary>
        /// 解锁物品
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        public bool UnLockAnyGoods(string goodsId)
        {
            if (!putAwayGoods.ContainsKey(goodsId))
            {
                return false;
            }

            return putAwayGoods[goodsId].UnLockGoods();
        }
    }
}

