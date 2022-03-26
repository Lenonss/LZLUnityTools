using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace LZLUnityTool.Plugins.InvntoryPlugin
{
    [Serializable]
    public abstract class BaseGoodsSO : ScriptableObject
    {
        [SerializeField,BoxGroup("物品识别信息"),InfoBox("名字：",InfoMessageType.None)]
        private string goodsName;
        public string GoodsName
        {
            get { return goodsName; }
        }
        
        [SerializeField,BoxGroup("物品识别信息"),InfoBox("ID：",InfoMessageType.None)]
        private string goodsId;
        public string GoodsId
        {
            get { return goodsId; }
        }

        
        
        [SerializeField,BoxGroup("物品配置信息"),InfoBox("类型：",InfoMessageType.None)]
        private GoodsType goodsType;
        public GoodsType GoodsType
        {
            get { return goodsType; }
        }

        [SerializeField,BoxGroup("物品配置信息"),InfoBox("品质：",InfoMessageType.None)]
        private GoodsLevel goodsLevel;
        public GoodsLevel GoodsLevel
        {
            get { return goodsLevel; }
        }
        
        
        
        [SerializeField,BoxGroup("物品描述信息"),InfoBox("数目：",InfoMessageType.None)]
        private int goodsNum;
        public int GoodsNum
        {
            get { return goodsNum; }
        }
        
        [SerializeField,BoxGroup("物品描述信息"),InfoBox("图标：",InfoMessageType.None)]
        private Sprite goodsIcon;
        public Sprite GoodsIcon
        {
            get { return goodsIcon; }
        }
        
        [SerializeField,BoxGroup("物品描述信息"),InfoBox("图标颜色：",InfoMessageType.None)]
        private Color goodsColor = Color.white;
        public Color GoodsColor
        {
            get { return goodsColor; }
        }
        
        
        [SerializeField, TextArea,BoxGroup("物品描述信息"),InfoBox("描述：",InfoMessageType.None)]
        private string description;
        public string Description
        {
            get
            {
                return description;
            }
        }

        
        [BoxGroup("其它")]
        public bool unLockState;
        
        [BoxGroup("其它"),SerializeField,InfoBox("数量是否无限：",InfoMessageType.None)]
        protected bool inexhaustible;//用之不竭
        /// <summary>
        /// 是否用之不竭
        /// </summary>
        public virtual bool Inexhaustible => inexhaustible;
        
        [BoxGroup("其它"),SerializeField,InfoBox("是否可以堆叠：",InfoMessageType.None)]
        protected bool stackAble = true;
        public virtual bool StackAble => Inexhaustible ? false : stackAble;
        public abstract void GoodsEffect();

        public abstract bool UnLockGoods();
        
    }

    public enum GoodsType
    {
        其他,
        使用品,
        装备
    }

    public enum GoodsLevel
    {
        普通,
        良,
        优秀,
        史诗,
        传说,
        典藏
    }
}