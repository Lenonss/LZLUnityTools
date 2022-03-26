using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace LZLUnityTool.Plugins.UIAnimPlugin
{
    public enum UISampleAnimType
    {
        /// <summary>
        /// 锚点二元量
        /// </summary>
        AnchorV2Move,
        /// <summary>
        /// 本地欧拉角
        /// </summary>
        LocalRotateV3,
        /// <summary>
        /// 尺寸
        /// </summary>
        SizeV3,
        /// <summary>
        /// 锚点二元量+本地欧拉角
        /// </summary>
        AV2MaLRV3,
        /// <summary>
        /// 锚点二元量+尺寸
        /// </summary>
        AV2MaSV3,
        /// <summary>
        /// 本地欧拉角+尺寸
        /// </summary>
        LRV3aSV3, 
        /// <summary>
        /// 本地欧拉角+锚点二元量+尺寸
        /// </summary>
        All,
    }
    
    [Serializable]
    public struct V2AnimChange
    {
        [Tooltip("使用通用动画时间")] 
        public bool useContinueDuration;

        [Tooltip("增量")]
        public Vector2 increment;
        [Tooltip("动画时间")]
        public float duration;
    }
    [Serializable]
    public struct V3AnimChange
    {
        [Tooltip("使用通用动画时间")] 
        public bool useContinueDuration;

        [Tooltip("增量")]
        public Vector3 increment;
        [Tooltip("动画时间")]
        public float duration;
    }
    
    public class UISampleAnimUnit : MonoBehaviour
    {
        [HideInInspector] [SerializeField, Tooltip("动画类型")]
        private UISampleAnimType _animType = UISampleAnimType.All;
        [Tooltip("锚点二元移动")]
        public V2AnimChange _anchorV2;
        [Tooltip("自身旋转")]
        public V3AnimChange _localRotateV3;
        [Tooltip("缩放增量")]
        public V3AnimChange _sizeV3;

        [Tooltip("通用动画时长")]
        public float duration;
        [Tooltip("旋转动画模式")]
        public RotateMode _rotateMode = RotateMode.Fast;
        [Tooltip("整数移动")]
        public bool _isSnaping;
        public UISampleAnimType UIAnimType
        {
            get => _animType;
        }

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }

}
