using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace LZLUnityTool.Plugins.UIAnimPlugin
{
    public class UIAnimationItem : MonoBehaviour
    {
        [SerializeField,BoxGroup("功能选择"),Title("旋转动画")]
        private bool _animateRotation = true;
        [SerializeField,BoxGroup("功能选择"),Title("缩放动画")]
        private bool _animateScale = true;
        public bool AnimateScale
        {
            get => _animateScale;
            set
            {
                this._animateScale = value;

                if (!_animateScale && Application.isPlaying) 
                {
                    this.transform.localScale = _startLocalScale;
                }
            }
        }
        [SerializeField,BoxGroup("功能选择"),Title("Y轴周期位移动画")]
        private bool _animateYOffset = true;
        public bool AnimateYOffset
        {
            get { return _animateYOffset;}
            set
            {
                this._animateYOffset = value;

                if (!_animateYOffset && Application.isPlaying) 
                {
                    this.transform.localPosition = _startLocalPosition;
                }
            }
        }

        [EnumToggleButtons]
        public selfTags selectFuncTag;
        
        [BoxGroup("旋转动画"),ShowIf("selectFuncTag",selfTags.旋转动画),Title("速度 degree/s")]
        public Vector3 rotationSpeedsInDegreePerSecond;
        [BoxGroup("旋转动画"),ShowIf("selectFuncTag",selfTags.旋转动画),Title("旋转类型")]
        public RotationType rotationType = RotationType.SelfAxis;
        
        [BoxGroup("缩放动画"),ShowIf("selectFuncTag",selfTags.缩放动画)]
        public float scaleMin = 0.5f, scaleMax = 1.5f, scaleCycleDuration = 5;
        
        [BoxGroup("Y轴周期位移动画"),ShowIf("selectFuncTag",selfTags.Y周期位移动画)]
        public float yOffsetAmplitude = 1, yOffsetCycleDuration = 5;
        
        
        private Vector3 _startLocalPosition;
        private Quaternion _startLocalRotation;
        private Vector3 _startLocalScale;

        private Transform _transform;

        void Awake() 
        {
            _transform = this.GetComponent<Transform>();

            _startLocalPosition = _transform.localPosition;
            _startLocalRotation = _transform.localRotation;
            _startLocalScale = _transform.localScale;
        }

        void Update() 
        {
            if (_animateYOffset) 
            {
                float yOff;
                if (yOffsetCycleDuration != 0) 
                {
                    yOff = Mathf.Sin(Time.time / yOffsetCycleDuration * Mathf.PI * 2) * yOffsetAmplitude;
                } 
                else 
                {
                    yOff = 0;
                }

                this.transform.localPosition = _startLocalPosition + new Vector3(0, yOff, 0);
            }

            if (_animateScale) 
            {
                float scale;
                if (scaleCycleDuration != 0) 
                {
                    float scaleT = Mathf.InverseLerp(-1, 1, Mathf.Sin(Time.time / scaleCycleDuration * Mathf.PI * 2));
                    scale = Mathf.Lerp(scaleMin, scaleMax, scaleT);
                }
                else 
                {
                    scale = 1;
                }

                this.transform.localScale = scale * _startLocalScale;
            }

            if (_animateRotation) 
            {
                if (rotationType == RotationType.WorldAxis) 
                {

                    this.transform.Rotate(rotationSpeedsInDegreePerSecond * Time.deltaTime, Space.World);
                } 
                else 
                {
                    this.transform.Rotate(rotationSpeedsInDegreePerSecond * Time.deltaTime, Space.Self);
                }
            }
        }
        
        
        
        public enum  selfTags
        {
            旋转动画,
            缩放动画,
            Y周期位移动画
        }
        public enum RotationType { SelfAxis, WorldAxis }
    }
}

