using System;
using System.Collections;
using System.Collections.Generic;
using LZLUnityTool.Plugins.CommonPlugin.ExtructFunction;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace LZLUnityTool.Plugins.CommercializePlugin.BaseClass
{
    public abstract class UpAttributeItem_Base : MonoBehaviour
    {
        public enum AttributeType
        {
            Type1,Type2,Type3,Type4
        }
        [Title("类型")]
        public AttributeType itemType;

        [BoxGroup("视频广告相关"),InfoBox("是否只支持视频获取",InfoMessageType.None)]
        public bool videoOnly;
        [BoxGroup("视频广告相关"),InfoBox("视频广告图标",InfoMessageType.None)]
        public Image videoIcon;
        [BoxGroup("视频广告相关"),InfoBox("是否根据数量更新广告图标",InfoMessageType.None),
         ShowIf("videoOnly",true)]
        public bool updateVideoIconActive;
        
        
        [BoxGroup("按钮相关"),InfoBox("是否自动获取按钮",InfoMessageType.None)] 
        public bool autoGetButton = true;
        [BoxGroup("按钮相关"),HideIf("autoGetButton",true)]
        public Button tarButton;
        
        [BoxGroup("实时更新相关")]
        [InfoBox("是否循环调用",InfoMessageType.None),Tooltip("只有组件能够正确获取时才会有效果")]
        public bool update = false;
        [BoxGroup("实时更新相关")]
        [InfoBox("循环监听间隔时间",InfoMessageType.None),ShowIf("update",true)]
        public float intervalSeconds = 0.1f;
    
        private float counter = 0;
        private bool canUseGold = true;

        protected virtual void GetButton()
        {
            if (autoGetButton)
            {
                if (GetComponent<Button>() == null)
                {
                    tarButton = gameObject.AddComponent<Button>();
                }
                else
                {
                    tarButton = GetComponent<Button>();
                }
            }
        }

        protected virtual void Awake()
        {
            GetButton();
        }

        private void OnEnable()
        {
            UpdateCanUseGold();
            UpdateItemUI();
            UpdateButtonFunc();
        }

        private void Update()
        {
            if (update)
            {
                counter += Time.deltaTime;
                if (counter > intervalSeconds)
                {
                    counter = 0;
                    UpdateCanUseGold();
                    UpdateItemUI();
                    UpdateButtonFunc();
                }
            }

        }

        /// <summary>
        /// 更新是否有钱购买状态参数
        /// </summary>
        protected virtual void UpdateCanUseGold()
        {
            canUseGold = MoneyIsEnougth(itemType);
        }

        /// <summary>
        /// 更新单元项的UI显示
        /// </summary>
        protected virtual void UpdateItemUI()
        {
            CommonTool.SetActive(videoIcon.gameObject,false);
            if (videoOnly)
            {
                if (updateVideoIconActive)
                {
                    bool showstate = GetAttributeNum(itemType) == 0 ? true : false;
                    CommonTool.SetActive(videoIcon.gameObject,showstate);
                }
                else
                {
                    CommonTool.SetActive(videoIcon.gameObject,true);
                }
            }
            else
            {
                if (canUseGold)
                {
                    CommonTool.SetActive(videoIcon.gameObject,false);
                }
                else
                {
                    CommonTool.SetActive(videoIcon.gameObject,true);
                }
            }
        }

        /// <summary>
        /// 更新按钮功能
        /// </summary>
        protected virtual void UpdateButtonFunc()
        {
            if (tarButton == null)
            {
                return;
            }
            
            //tarButton.onClick.RemoveAllListeners();
            RemoveButtonAllListener();
            if (videoOnly)
            {
                tarButton.onClick.AddListener(VideoOnlyButtonFunc);
            }
            else
            {
                if (canUseGold)
                {
                    tarButton.onClick.AddListener(UpAttribute);
                }
                else
                {
                    tarButton.onClick.AddListener(VideoToUp);
                }
            }
        }

        private void UpAttribute()
        {
            if (canUseGold)
            {
                if (UseGold(itemType))
                {
                    UpAttribute(itemType);
                            
                    UpdateCanUseGold();
                    UpdateItemUI();
                    UpdateButtonFunc();
                }
            }
            //必定是通过VideoToUp进来的
            else
            {
                        
                UpAttribute(itemType);
                            
                UpdateCanUseGold();
                UpdateItemUI();
                UpdateButtonFunc();
            }
        }

        protected virtual void RemoveButtonAllListener()
        {
            tarButton.onClick.RemoveListener(VideoOnlyButtonFunc);
            tarButton.onClick.RemoveListener(UpAttribute);
            tarButton.onClick.RemoveListener(VideoToUp);
        }

        protected virtual void VideoToUp()
        {
            ShowAdsVideo(UpAttribute);
        }

        /// <summary>
        /// 使用金钱
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected virtual bool UseGold(AttributeType type)
        {
            if (!MoneyIsEnougth(type))
            {
                return false;
            }
            else
            {
                ReduceMoney(type);
                return true;
            }
        }

        /// <summary>
        /// 金钱是否足够支持某个属性的购买或者升级
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected abstract bool MoneyIsEnougth(AttributeType type);

        /// <summary>
        /// 消耗货币，用于升级或购买物品
        /// </summary>
        /// <param name="type"></param>
        protected abstract void ReduceMoney(AttributeType type);
        /// <summary>
        /// 仅仅只能观看视频时按钮的功能
        /// </summary>
        protected abstract void VideoOnlyButtonFunc();

        /// <summary>
        /// 实现对应属性的增加
        /// </summary>
        /// <param name="type"></param>
        protected abstract void UpAttribute(AttributeType type);

        /// <summary>
        /// 显示视频广告
        /// </summary>
        /// <param name="upAtb">升级或购买属性的函数，父类已经添加了</param>
        protected abstract void ShowAdsVideo(Action upAtb = null);

        /// <summary>
        /// 获得属性数量
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected abstract int GetAttributeNum(AttributeType type);
        
    }
}

