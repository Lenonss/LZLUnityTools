using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace LZLUnityTool.Plugins.UIAnimPlugin
{
    public class UITextValueChangeAnimItem : MonoBehaviour
    {
        [Header("变化方式")]
        public ChangeType changeSpeedType;
        
        [Header("公共配置"),InfoBox("自动获取文本组件",InfoMessageType.None)]
        public bool autoGetText;
        [HideIf("autoGetText",true)]
        public Text text;
        public bool playOnAwake = false;
        [InfoBox("起始值",InfoMessageType.None)]
        public int startValue;
        [InfoBox("目标值",InfoMessageType.None)]
        public int targetValue;
        [InfoBox("是否忽略timeScale影响",InfoMessageType.None)]
        public bool ignoreTimeScale = true;

        [Header("震动"),InfoBox("是否启用变化时发生尺寸震动",InfoMessageType.None)] 
        public bool useChangeShake = false;
        [InfoBox("变化目标尺寸",InfoMessageType.None),Range(0.1f,2f),ShowIf("useChangeShake",true)]
        public float changedScale;
        [InfoBox("尺寸变化持续时间",InfoMessageType.None),ShowIf("useChangeShake",true)]
        public float shakeDuration;
        
        //固定
        [BoxGroup("变化参数配置")]
        [InfoBox("固定变换间隔时间"),ShowIf("changeSpeedType",ChangeType.固定)]
        public float unitDuration;

        //线性
        [BoxGroup("变化参数配置")]
        [InfoBox("变换间隔时间尺寸影响因素",InfoMessageType.None),HideIf("changeSpeedType",ChangeType.固定),
        Range(0,2f)]
        public float scaleFactor = 1;
        
        //曲线
        [BoxGroup("变化参数配置")]
        [InfoBox("变化曲线",InfoMessageType.None),ShowIf("changeSpeedType",ChangeType.曲线)]
        public AnimationCurve changeCruve;

        private Vector3 startScale;
        private Coroutine shakeScaleCoro;
        private void Awake()
        {
            //shakeScale
            startScale = transform.localScale;
            changedScale = Mathf.Clamp(changedScale, 0.1f, 2f);
            
            if (autoGetText)
            {
                text = GetComponent<Text>();
            }
            if (playOnAwake)
            {
                PlayChangeAnim();
            }
        }

        /// <summary>
        /// 全部试用脚本自身配置播放动画
        /// </summary>
        public void PlayChangeAnim()
        {
            PlayChangeAnim(changeSpeedType,startValue,targetValue);
        }

        /// <summary>
        /// 仅仅自己配置开始结束值进行播放动画
        /// </summary>
        /// <param name="type"></param>
        /// <param name="startValueP"></param>
        /// <param name="targetValueP"></param>
        public void PlayChangeAnim(ChangeType type,int startValueP, int targetValueP)
        {
            switch (type)
            {
                case ChangeType.固定:
                    ValueChange_Fixed(startValueP,targetValueP,unitDuration,ignoreTimeScale);
                    break;
                case ChangeType.线性:
                    ValueChange_Linear(startValueP,targetValueP,ignoreTimeScale);
                    break;
                case ChangeType.曲线:
                    ValueChange_Cruve(startValueP,targetValueP,changeCruve,ignoreTimeScale);
                    break;
            }
            
        }

        //**************数值变化
        public void ValueChange_Fixed(int startValueP,int targetValueP,float unitDurationP,bool ignoreScale = true)
        {
            if (CheckTextExist())
            {
                StartCoroutine(IEValueChange_Fixed(startValueP, targetValueP, unitDurationP, ignoreScale));
            }
        }
        private IEnumerator IEValueChange_Fixed(int startValueP, int targetValueP, float unitDurationP,
            bool ignoreScale = true)
        {
            var realTime = new WaitForSecondsRealtime(unitDurationP);
            var time = new WaitForSeconds(unitDurationP);
            int unitChangeValue = GetUnitChangeValue(startValueP, targetValueP);
            text.text = startValueP.ToString();
            while (true)
            {
                startValueP += unitChangeValue;
                text.text = startValueP.ToString();
                
                //shakeScale
                if (useChangeShake)
                {
                    PlayShakeScale();
                }
                
                if (ignoreScale)
                {
                    yield return realTime;
                }
                else
                {
                    yield return time;
                }

                if (startValueP == targetValueP)
                {
                    break;
                }
            }
        }
        
        /// <summary>
        /// 线性变化值
        /// </summary>
        /// <param name="startValueP"></param>
        /// <param name="targetValueP"></param>
        /// <param name="ignoreScale"></param>
        public void ValueChange_Linear(int startValueP,int targetValueP,bool ignoreScale = true)
        {
            if (CheckTextExist())
            {
                StartCoroutine(IEValueChange_Linear(startValueP, targetValueP, ignoreScale));
            }
        }
        private IEnumerator IEValueChange_Linear(int startValueP, int targetValueP, bool ignoreScale = true)
        {
            float unitDuration_p = 0;
            float delta = Mathf.Abs(targetValueP - startValueP);
            int unitChangeValue = GetUnitChangeValue(startValueP, targetValueP);
            text.text = startValueP.ToString();
            while (true)
            {
                startValueP += unitChangeValue;
                text.text = startValueP.ToString();

                //shakeScale
                if (useChangeShake)
                {
                    PlayShakeScale();
                }
                
                unitDuration_p = Mathf.Clamp01(1 - startValueP / delta);
                if (ignoreScale)
                {
                    yield return new WaitForSecondsRealtime(unitDuration_p*scaleFactor);
                }
                else
                {
                    yield return new WaitForSeconds(unitDuration_p*scaleFactor);
                }

                if (startValueP == targetValueP)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// 曲线变化值
        /// </summary>
        /// <param name="startValueP"></param>
        /// <param name="targetValueP"></param>
        /// <param name="ignoreScale"></param>
        public void ValueChange_Cruve(int startValueP,int targetValueP,AnimationCurve cruve,bool ignoreScale = true)
        {
            if (CheckTextExist())
            {
                StartCoroutine(IEValueChange_Cruve(startValueP, targetValueP,cruve, ignoreScale));
            }
        }
        private IEnumerator IEValueChange_Cruve(int startValueP, int targetValueP, AnimationCurve cruve,bool ignoreScale = true)
        {
            float unitDuration_p = 0;
            float delta = Mathf.Abs(targetValueP - startValueP);
            int unitChangeValue = GetUnitChangeValue(startValueP, targetValueP);
            text.text = startValueP.ToString();
            while (true)
            {
                startValueP += unitChangeValue;
                text.text = startValueP.ToString();

                //shakeScale
                if (useChangeShake)
                {
                    PlayShakeScale();
                }
                
                unitDuration_p = Mathf.Clamp01(1 - cruve.Evaluate(startValueP / delta));
                if (ignoreScale)
                {
                    yield return new WaitForSecondsRealtime(unitDuration_p*scaleFactor);
                }
                else
                {
                    yield return new WaitForSeconds(unitDuration_p*scaleFactor);
                }

                if (startValueP == targetValueP)
                {
                    break;
                }
            }
        }
        /// <summary>
        /// 获得单元改变值
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private int GetUnitChangeValue(int s, int e)
        {
            var delta = e - s;
            return delta / Mathf.Abs(delta);
        }


        #region 尺寸震动

        /// <summary>
        /// 尺寸震动效果实现函数
        /// </summary>
        /// <param name="duration"></param>
        /// <returns></returns>
        private IEnumerator ShakeScale(float duration)
        {
            transform.localScale = changedScale * startScale;
            if (ignoreTimeScale)
            {
                yield return new WaitForSecondsRealtime(duration);
            }
            else
            {
                yield return new WaitForSeconds(duration);
            }
            transform.localScale = startScale;
        }

        /// <summary>
        /// 停止尺寸震动
        /// </summary>
        private void StopShakeScale()
        {
            if (shakeScaleCoro != null)
            {
                StopCoroutine(shakeScaleCoro);
                shakeScaleCoro = null;
                transform.localScale = startScale;
            }
        }

        /// <summary>
        /// 开始尺寸震动
        /// </summary>
        private void PlayShakeScale()
        {
            if (shakeScaleCoro == null)
            {
                shakeScaleCoro = StartCoroutine(ShakeScale(shakeDuration));
            }
            else
            {
                StopShakeScale();
            }
        }
        #endregion


        /// <summary>
        /// 检查Text是否为空
        /// </summary>
        /// <returns></returns>
        public bool CheckTextExist()
        {
            if (!text)
            {
                Debug.LogError( gameObject.name+" 的 "+ nameof(UITextValueChangeAnimItem)+"'s text is null");
            }
            return text;
        }

        public enum ChangeType
        {
            线性,曲线,固定
        }
    }
}

