using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace LZLUnityTool.Plugins.CommonPlugin.Tools
{
    public class HookPrefs : MonoBehaviour 
    { 
        public enum MyType 
        { 
            Toggle, 
            Slider, 
            Text, 
            GameObject ,
            Image
        }
        public enum PpType
        {
            Integer,
            Floating,    
            String
        }
        
        [Title("组件类型")]
        public MyType myType;

        [Title("图片功能介绍"),ShowIf("myType",MyType.Toggle),ReadOnly,Multiline]
        public string ToggleInfo = "Toggle:根据读取的数据切换isOn状态";
        [Title("图片功能介绍"),ShowIf("myType",MyType.Slider),ReadOnly,Multiline]
        public string SliderInfo = "Slider:根据读取的数据设置滑动条进度";
        [Title("图片功能介绍"),ShowIf("myType",MyType.Text),ReadOnly,Multiline]
        public string TextInfo = "Text:根据读取的数据设置文本内容";
        [Title("图片功能介绍"),ShowIf("myType",MyType.GameObject),ReadOnly,Multiline]
        public string GameObjectInfo = "GameObject:根据读取的数据设置游戏体激活状态";
        [Title("图片功能介绍"),ShowIf("myType",MyType.Image),ReadOnly,Multiline]
        public string ImgInfo = "Image:根据读取数据更新fillAmount";
        
        [Title("PlayerPreds存储数据的键值")]
        public string playerPrefsName;

  
        
        [Title("PlayerPrefs存储数据的类型")]
        public PpType playerPrefsType;
        [ShowIf("playerPrefsType",PpType.String)]
        public string playerPrefsStringCompare; 
        [ShowIf("playerPrefsType",PpType.Floating)]
        public float playerPrefsFloatCompare;
        [ShowIf("playerPrefsType",PpType.Integer)]
        public int playerPrefsIntegerCompare;
        [Title("判断时是否取相反")]
        public bool invertComparison = false;
        
        
        [Title("是否循环调用"),Tooltip("只有组件能够正确获取时才会有效果")]
        public bool update = false;
        [Title("是否强制开启循环调用"),Tooltip("忽略当前游戏体未挂载对应组件的影响"),ShowIf("update",true)]
        public bool forceUpdate = false;
        [Title("循环监听间隔时间")]
        public float intervalSeconds = 1;
    
        private float counter = 0;
        
        int cacheInt = 0;
        float cacheFloat = 0;
        string cacheString = "";
        
        void Awake()
        {
            verify();
            UpdateHook();
        } 
        
        void Update () 
        {
            if (update)
            {
                counter += Time.deltaTime;
                if (counter > intervalSeconds)
                {
                    counter = 0;
                    if (forceUpdate ||  verify())
                    {
                        UpdateHook();
                    }
                }
            }
        }
        //检测选择的组件类型是否挂载
        bool verify()
        {
            if (playerPrefsType == PpType.Floating)
            {
                var c = PlayerPrefs.GetFloat(playerPrefsName) != cacheFloat;
                cacheFloat = PlayerPrefs.GetFloat(playerPrefsName);
                return c;
            }
            else if (playerPrefsType == PpType.Integer)
            {
                var c = PlayerPrefs.GetInt(playerPrefsName) != cacheInt;
                cacheInt = PlayerPrefs.GetInt(playerPrefsName);
                return c;
            }
            else if (playerPrefsType == PpType.String)
            {
                var c = PlayerPrefs.GetString(playerPrefsName) != cacheString;
                cacheString = PlayerPrefs.GetString(playerPrefsName);
                return c;
            }
            return false;
        }
        
        void UpdateHook() 
        {
            switch (myType)
            {
                case MyType.Toggle:
                    ToggleHookPref();
                    break;
                case MyType.GameObject:
                    GameObjectHookPrefs();
                    break;
                case MyType.Text:
                    TextHookPrefs();
                    break;
                case MyType.Slider:
                    SliderHookPrefs();
                    break;
                case MyType.Image:
                    ImageHookPrefs();
                    break;
            }
        }

        private void ImageHookPrefs()
        {
            var comp = GetComponent<Image>();
            if (comp && comp.sprite != null)
            {
                if (playerPrefsType == PpType.Floating)
                {
                    comp.type = Image.Type.Filled;
                    comp.fillAmount = PlayerPrefs.GetFloat(playerPrefsName);
                }
                else if (playerPrefsType == PpType.Integer)
                {
                    comp.type = Image.Type.Filled;
                    comp.fillAmount = PlayerPrefs.GetInt(playerPrefsName);
                }
                else if (playerPrefsType == PpType.String)
                {
                    comp.type = Image.Type.Filled;
                    comp.fillAmount = float.Parse(PlayerPrefs.GetString(playerPrefsName));
                }

                comp.fillAmount = Mathf.Clamp01(comp.fillAmount);
            }
        }

        private void SliderHookPrefs()
        {
            var comp = GetComponent<Slider>();
            if (comp)
            {
                if (playerPrefsType == PpType.Floating)
                {
                    comp.value = PlayerPrefs.GetFloat(playerPrefsName);
                }
                else if (playerPrefsType == PpType.Integer)
                {
                    comp.value = PlayerPrefs.GetInt(playerPrefsName);
                }
                else if (playerPrefsType == PpType.String)
                {
                    comp.value = float.Parse(PlayerPrefs.GetString(playerPrefsName));
                }
            }
        }

        private void TextHookPrefs()
        {
            var comp = GetComponent<Text>();
            if (comp)
            {
                if (playerPrefsType == PpType.Floating)
                {
                    comp.text = "" + PlayerPrefs.GetFloat(playerPrefsName);
                }
                else if (playerPrefsType == PpType.Integer)
                {
                    comp.text = "" + PlayerPrefs.GetInt(playerPrefsName);
                }
                else if (playerPrefsType == PpType.String)
                {
                    comp.text = "" + PlayerPrefs.GetString(playerPrefsName);
                }
            }
        }

        private void GameObjectHookPrefs()
        {
            bool enable = gameObject.activeSelf;
            if (playerPrefsType == PpType.Floating)
            {
                enable = (playerPrefsFloatCompare == PlayerPrefs.GetFloat(playerPrefsName)) == !invertComparison;
            }
            else if (playerPrefsType == PpType.Integer)
            {
                enable = (playerPrefsIntegerCompare == PlayerPrefs.GetInt(playerPrefsName)) == !invertComparison;
            }
            else if (playerPrefsType == PpType.String)
            {
                enable = (playerPrefsStringCompare == PlayerPrefs.GetString(playerPrefsName)) == !invertComparison;
            }

            gameObject.SetActive(enable);
        }

        private void ToggleHookPref()
        {
            var comp = GetComponent<Toggle>();
            if (comp)
            {
                if (playerPrefsType == PpType.Floating)
                {
                    comp.isOn = (playerPrefsFloatCompare == PlayerPrefs.GetFloat(playerPrefsName)) == !invertComparison;
                }
                else if (playerPrefsType == PpType.Integer)
                {
                    comp.isOn = (playerPrefsIntegerCompare == PlayerPrefs.GetInt(playerPrefsName)) == !invertComparison;
                }
                else if (playerPrefsType == PpType.String)
                {
                    comp.isOn = (playerPrefsStringCompare == PlayerPrefs.GetString(playerPrefsName)) == !invertComparison;
                }
            }
        }
        


}
        
}
