using LZLUnityTool.Plugins.CommonPlugin;
using Sirenix.OdinInspector;
using UnityEngine;

namespace LZLUnityTool.Plugins.CommercializePlugin.Manager
{
    [DisallowMultipleComponent]
    public class ChannelManager : SingletonMono<ChannelManager>
    {
        [EnumToggleButtons]
        public ChannelTye appChannelType;
        protected void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// 是否是小米渠道
        /// </summary>
        /// <returns></returns>
        public bool IsXiaoMiChannel()
        {
            return appChannelType == ChannelTye.XiaoMi;
        }
        /// <summary>
        /// 是否是233渠道
        /// </summary>
        /// <returns></returns>
        public bool Is233Channel()
        {
            return appChannelType == ChannelTye.twoThreeThree;
        }
    }

    public enum ChannelTye
    {
        XiaoMi,
        /// <summary>
        /// 233
        /// </summary>
        twoThreeThree,
    }
}