using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LZLUnityTool.Plugins.CommonPlugin.Interface
{
    /// <summary>
    /// 订阅接口
    /// </summary>
    public interface ISubscribe
    {
        /// <summary>
        /// 集中订阅事件
        /// </summary>
        public void SubscribeEvent();
        /// <summary>
        /// 集中取消订阅事件
        /// </summary>
        public void UnSubscribeEvent();
    }
}

