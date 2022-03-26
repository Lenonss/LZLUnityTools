using System;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace LZLUnityTool.Plugins.CommercializePlugin.BaseClass
{
    public class SkinTryPanel_Base : MonoBehaviour
    {
        public bool hasTitle = false;
        [ShowIf("hasTitle",true)]
        public Text TitleText;

        [Title("试用皮肤单元项")]
        public List<GameObject> TrySkinItems;

        [Title("放弃按钮")]
        public Button GiveUpButton;

        public float delayShowDuration = 2.5f;

        
        protected void Awake()
        {
            InitPanelUI();
        }

        protected virtual void InitPanelUI()
        {
            InitBtnFunc();
            
            //show givebutton
            GiveUpButton.gameObject.SetActive(false);
            DOVirtual.DelayedCall(delayShowDuration, (() => GiveUpButton.gameObject.SetActive(true)));
        }

        protected virtual void InitBtnFunc()
        {
            GiveUpButton.onClick.RemoveListener(GiveUpButtonFunc);
            GiveUpButton.onClick.AddListener(GiveUpButtonFunc);
        }

        protected virtual void GiveUpButtonFunc()
        {
            gameObject.SetActive(false);
        }
    }
}