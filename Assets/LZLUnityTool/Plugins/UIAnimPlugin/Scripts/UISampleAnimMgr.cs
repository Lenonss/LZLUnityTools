using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace LZLUnityTool.Plugins.UIAnimPlugin
{
    public class UISampleAnimMgr : MonoBehaviour
    {
        /// <summary>
        /// 基于二元锚点的UI移动
        /// </summary>
        /// <param name="animObjs">被移到的UI</param>
        /// <param name="isforWard">true:+increment,false:-increment</param>
        /// <param name="endCall">结尾处调用</param>
        /// <typeparam name="T"></typeparam>
        public static void DoAnchorV2Move<T>(List<T> animObjs,bool isforWard,Action endCall = null) where T : UISampleAnimUnit
        {
            if (animObjs.Count==0) return;
            foreach (var item in animObjs)
            {
                //RectTransform获取
                RectTransform itemRectTransform = null;
                if (item.GetComponent<RectTransform>() == null)
                {
                    Debug.LogError("UISampleAnimMgrMethod : " +
                        item.name.ToString()+" has not RectTramsform Component");
                    continue;
                }
                else
                {
                    itemRectTransform = item.GetComponent<RectTransform>();
                }
                
                if (item.UIAnimType == UISampleAnimType.AnchorV2Move || 
                    item.UIAnimType == UISampleAnimType.AV2MaSV3 ||
                    item.UIAnimType == UISampleAnimType.AV2MaLRV3 ||
                    item.UIAnimType == UISampleAnimType.All)
                {
                    //获得动画时间
                    var duration = item._anchorV2.useContinueDuration ? item.duration : item._anchorV2.duration;
                    //获得目标位置
                    var targetAnchorPos = itemRectTransform.anchoredPosition
                                          + (isforWard ? item._anchorV2.increment : -item._anchorV2.increment);
                    //开始移动
                    itemRectTransform.DOAnchorPos(targetAnchorPos, duration,item._isSnaping);
                }
            }
            
            //调用函数
            endCall?.Invoke();
        }
        /// <summary>
        /// 基于本地欧拉角的UI旋转
        /// </summary>
        /// <param name="animObjs">被操作的UI</param>
        /// <param name="AddOrSubstract">true:+increment,false:-increment</param>
        /// <param name="endCall">结尾处调用</param>
        /// <typeparam name="T"></typeparam>
        public static void DoLocalRotateV3<T>(List<T> animObjs, bool AddOrSubstract, Action endCall = null)
            where T : UISampleAnimUnit
        {
            if (animObjs.Count==0) return;
            foreach (var item in animObjs)
            {
                //RectTransform获取
                RectTransform itemRectTransform = null;
                if (item.GetComponent<RectTransform>() == null)
                {
                    Debug.LogError("UISampleAnimMgrMethod : " +
                                   item.name.ToString()+" has not RectTramsform Component");
                    continue;
                }
                else
                {
                    itemRectTransform = item.GetComponent<RectTransform>();
                }
                
                if (item.UIAnimType == UISampleAnimType.LocalRotateV3 || 
                    item.UIAnimType == UISampleAnimType.LRV3aSV3 ||
                    item.UIAnimType == UISampleAnimType.AV2MaLRV3 ||
                    item.UIAnimType == UISampleAnimType.All)
                {
                    //获得动画时间
                    var duration = item._anchorV2.useContinueDuration ? item.duration : item._anchorV2.duration;
                    //获得目标位置
                    var targetAnchorPos = itemRectTransform.localEulerAngles
                                          + (AddOrSubstract ? item._localRotateV3.increment : -item._localRotateV3.increment);
                    //开始移动
                    itemRectTransform.DOLocalRotate(targetAnchorPos, duration, item._rotateMode);
                }
            }
            
            //调用函数
            endCall?.Invoke();
        }
        /// <summary>
        /// 基于Scale的缩放
        /// </summary>
        /// <param name="animObjs">被操作的UI</param>
        /// <param name="AddOrSubstract">true:+increment,false:-increment</param>
        /// <param name="endCall">结尾处调用</param>
        /// <typeparam name="T"></typeparam>
        public static void DoSizeV3<T>(List<T> animObjs, bool AddOrSubstract, Action endCall = null)
            where T : UISampleAnimUnit
        {
            if (animObjs.Count==0) return;
            foreach (var item in animObjs)
            {
                //RectTransform获取
                RectTransform itemRectTransform = null;
                if (item.GetComponent<RectTransform>() == null)
                {
                    Debug.LogError("UISampleAnimMgrMethod : " +
                                   item.name.ToString()+" has not RectTramsform Component");
                    continue;
                }
                else
                {
                    itemRectTransform = item.GetComponent<RectTransform>();
                }
                
                if (item.UIAnimType == UISampleAnimType.SizeV3 || 
                    item.UIAnimType == UISampleAnimType.AV2MaSV3 ||
                    item.UIAnimType == UISampleAnimType.LRV3aSV3 ||
                    item.UIAnimType == UISampleAnimType.All)
                {
                    //获得动画时间
                    var duration = item._anchorV2.useContinueDuration ? item.duration : item._sizeV3.duration;
                    //获得目标位置
                    var targetAnchorPos = itemRectTransform.localScale
                                          + (AddOrSubstract ? item._sizeV3.increment : -item._sizeV3.increment);
                    //开始移动
                    itemRectTransform.DOScale(targetAnchorPos, duration);
                }
            }
            
            //调用函数
            endCall?.Invoke();
        }
    }

}
