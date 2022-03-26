using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace LZLUnityTool.Plugins.InventoryPlugin.System.CurrencySystem
{
    public  class UICurrencyPickController : MonoBehaviour
    {
        [InfoBox("该单元项货币数目")]
        public int CurrencyAmount = 1;

        [BoxGroup("配置参数"),InfoBox("延迟多久启用飞行：",InfoMessageType.None)]
        public float FlyToUIYieldTime = 0;
        [BoxGroup("配置参数"),InfoBox("延迟飞行是否忽略timeScale的影响：",InfoMessageType.None)]
        public bool IgnoreTimeScale = true;
        [BoxGroup("配置参数"),InfoBox("飞行总时长：",InfoMessageType.None)]
        public float FlyToUITime = 0.5f;
        [BoxGroup("配置参数"),InfoBox("飞行速度曲线：",InfoMessageType.None)]
        public AnimationCurve FlyToUICurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
        [BoxGroup("配置参数"),InfoBox("目标UI：",InfoMessageType.None)]
        public Transform targetUI;
        
        [BoxGroup("Runtime State")] 
        [ReadOnly,InfoBox("飞行状态：",InfoMessageType.None)]
        public bool IsFlyingToUI;
        [BoxGroup("Runtime State"),ReadOnly,InfoBox("飞行实时时间：",InfoMessageType.None)]
        public float FlyToUICurrentTime;
        [BoxGroup("Runtime State"),ReadOnly,InfoBox("飞行起始位置：",InfoMessageType.None)]
        public Vector3 FlyToUIStartPosition;

        public Action<int> GetCurency;

        protected virtual void Start()
        {
            StartCoroutine(StartFly(FlyToUIYieldTime, IgnoreTimeScale));
        }

        protected IEnumerator StartFly(float yieldTime, bool ignoreTimeScale)
        {
            if (ignoreTimeScale)
            {
                yield return new WaitForSecondsRealtime(yieldTime);
                IsFlyingToUI = true;
                FlyToUIStartPosition = transform.position;
            }
            else
            {
                yield return new WaitForSeconds(yieldTime);
                IsFlyingToUI = true;
                FlyToUIStartPosition = transform.position;
            }
        }

        // Update is called once per frame
       protected  virtual void Update()
        {
            if (IsFlyingToUI)
            {
                FlyToUICurrentTime += Time.deltaTime;
                float t = FlyToUICurve.Evaluate(FlyToUICurrentTime / FlyToUITime);
                t = Mathf.Clamp01(t);

                transform.position = Vector3.Lerp(FlyToUIStartPosition, targetUI.position, t);

                if (t >= 1f)
                {
                    GetCurency?.Invoke(CurrencyAmount);
                    Destroy(gameObject);
                }
            }
        }
       
    }
}

