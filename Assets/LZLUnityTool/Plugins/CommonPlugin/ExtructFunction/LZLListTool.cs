using System.Collections.Generic;
using UnityEngine;

namespace LZLUnityTool.Plugins.CommonPlugin.ExtructFunction
{
    public static class LZLListTool
    {
        /// <summary>
        /// 获得数组的随机项
        /// </summary>
        /// <param name="srcList"></param>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        public static TValue GetRandomItem<TValue>(this List<TValue> srcList)
        {
            if (srcList.Count == 0)
            {
                return default;
            }
            int randomTemp = Random.Range(0, srcList.Count);
            return  srcList[randomTemp];
        }
    }
}