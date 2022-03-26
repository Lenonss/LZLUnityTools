using System;
using LZLUnityTool.Plugins.CommercializePlugin.Manager;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace LZLUnityTool.Plugins.CommercializePlugin.BaseClass
{
    public abstract class UIToggleDoubleCash_Base : MonoBehaviour
    {
        [Title("组件")]
        public Button _targetBtn;
        public Text _targetBtnText;
        protected Toggle _videoToggle;

        [Title("语言版本相关")]
        public ChinesizeManager.ChinesizeStringGroup isOnText;
        public ChinesizeManager.ChinesizeStringGroup unIsOnText;
        
        protected virtual void Awake()
        {
            _videoToggle = GetComponent<Toggle>();
            _targetBtn.onClick.AddListener(TargetBtnFunc);
        }

        protected virtual void Update()
        {
            updateBtnText();
        }

        /// <summary>
        /// 目标按钮函数功能实现
        /// </summary>
        protected abstract void TargetBtnFunc();
        
        
        
        private void updateBtnText()
        {
            if (_targetBtn == null)
            {
                return;
            }
            
            if (_videoToggle.isOn)
            {
                ChinesizeManager.Instance.SetTextSrcByLanguageType(ref _targetBtnText,isOnText.chinesizeStr,isOnText.englishStr);
            }
            else
            {
                ChinesizeManager.Instance.SetTextSrcByLanguageType(ref _targetBtnText,unIsOnText.chinesizeStr,unIsOnText.englishStr);
            }
        }
    }
}