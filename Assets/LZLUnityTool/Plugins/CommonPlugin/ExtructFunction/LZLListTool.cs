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
        
        /// <summary>
        /// 打乱数组顺序
        /// </summary>
        /// <param name="srcList"></param>
        /// <typeparam name="T"></typeparam>
        public static void RandomSelf<T>(this List<T> srcList)
        {
            for (int j = 0; j < srcList.Count; j++)
            {
                int x, y; T t;
                x = Random.Range(0, srcList.Count);
                do
                {
                    y = Random.Range(0, srcList.Count);
                } while (y == x);
 
                t = srcList[x];
                srcList[x] = srcList[y];
                srcList[y] = t;
            }
        }
    }
}