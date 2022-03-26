using System;
using System.Collections;
using System.Collections.Generic;
using LZLUnityTool.Plugins.CommonPlugin;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace LZLUnityTool.Plugins.CommercializePlugin.Manager
{
    [DisallowMultipleComponent]
    public class ChinesizeManager : SingletonMono<ChinesizeManager>
    {
        public enum LanguageType
        {
            English,Chinese
        }

        [Title("语言类别"),EnumToggleButtons]
        public LanguageType type;

        /// <summary>
        /// 是否是中文版
        /// </summary>
        public bool isChinese
        {
            get
            {
                if (type == LanguageType.Chinese)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        protected void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// 设置图片的图片为汉化版还是英文版的图片，判据为ChinesizeManager.isChinese
        /// </summary>
        /// <param name="targetImg"></param>
        /// <param name="spriteData"></param>
        public void SetImgSrcByLanguageType(ref Image targetImg, ChinesizeSpriteGroup spriteData)
        {
            if (targetImg == null)
            {
                return;
            }

            targetImg.sprite = isChinese ? spriteData.chinesizeSprite : spriteData.englishSprite;
        }

        /// <summary>
        /// 设置文本内容为汉化版还是英文版的内容，判据为ChinesizeManager.isChinese
        /// </summary>
        /// <param name="targetText"></param>
        /// <param name="strData"></param>
        public void SetTextSrcByLanguageType(ref Text targetText, ChinesizeStringGroup strData)
        {
            if (targetText == null)
            {
                return;
            }

            targetText.text = isChinese ? strData.chinesizeStr : strData.englishStr;
        }

        /// <summary>
        /// 设置文本内容为汉化版还是英文版的内容，判据为ChinesizeManager.isChinese
        /// </summary>
        public void SetTextSrcByLanguageType(ref Text targetText, string chinStr, string englishStr)
        {
            if (targetText == null)
            {
                return;
            }

            targetText.text = isChinese ? chinStr : englishStr;
        }

        /// <summary>
        /// 设置文本颜色为汉化版还是英文版的内容，判据为ChinesizeManager.isChinese
        /// </summary>
        /// <param name="targetText"></param>
        /// <param name="strData"></param>
        public void SetTextColorByLanguageType(ref Text targetText, ChinesizeColorGroup strData)
        {
            if (targetText == null)
            {
                return;
            }

            targetText.color = isChinese ? strData.chinesizeColor : strData.englishColor;
        }
        
        /// <summary>
        /// 汉化Sprite组
        /// </summary>
        [Serializable]
        public class ChinesizeSpriteGroup
        {
            public Sprite chinesizeSprite;
            public Sprite englishSprite;
        }

        /// <summary>
        /// 汉化字符内容组
        /// </summary>
        [Serializable]
        public class ChinesizeStringGroup
        {
            public string chinesizeStr;
            public string englishStr;
        }
        
        /// <summary>
        /// 汉化尺寸
        /// </summary>
        [Serializable]
        public class ChinesizeSizeGroup
        {
            public Vector2 chansizeSize;
            public Vector2 englishSize;
        }
        
        /// <summary>
        /// 汉化颜色组
        /// </summary>
        [Serializable]
        public class ChinesizeColorGroup
        {
            public Color chinesizeColor;
            public Color englishColor;
        }
    }
}

