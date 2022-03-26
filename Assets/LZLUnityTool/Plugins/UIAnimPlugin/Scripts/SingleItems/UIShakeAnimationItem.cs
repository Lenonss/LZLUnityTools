using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace LZLUnityTool.Plugins.UIAnimPlugin
{
	public class UIShakeAnimationItem : MonoBehaviour
	{
		 [BoxGroup("晃动触发设置")] 
    public bool playOnAwake = false;
    
    [BoxGroup("晃动参数配置"),InfoBox("晃动幅度",InfoMessageType.None)]
    public float shakeFactor = 0.1f;
    [BoxGroup("晃动参数配置"),InfoBox("单元晃动周期（秒）,指走完2PI周期需要的时间",InfoMessageType.None)]
    public float shakeUnitDuration = 0.1f;
    [BoxGroup("晃动参数配置"),InfoBox("总晃动时长（秒）",InfoMessageType.None)]
    public float shakeSumDuration  = 120;

    [BoxGroup("运行时参数状态："),ReadOnly,InfoBox("实时摇晃剩余时间（秒）：",InfoMessageType.None)]
    public float curDuration;

    [BoxGroup("事件"),InfoBox("晃动结束后触发",InfoMessageType.None)]
    public UnityEvent OnShakeOver;

    private Coroutine curCoroutine;
    private Vector3 startLocalEulerAngles;
    
    private void Awake()
    {
        startLocalEulerAngles = transform.localEulerAngles;
        if (playOnAwake)
        {
            PlayShake(shakeSumDuration);
        }
    }

    private void OnDisable()
    {
        transform.localEulerAngles = startLocalEulerAngles;
        if (curCoroutine != null)
        {
            StopCoroutine(curCoroutine);
            curCoroutine = null;
        }
    }

    public bool PlayShake(float shakeDuration,bool forceShake = false,Action shakeOverInvoke = null)
    {
        if (curCoroutine == null)
        {
            curCoroutine = StartCoroutine(ShakeSelf(shakeDuration, shakeOverInvoke));
            return true;
        }
        else
        {
            if (forceShake)
            {
                StopCoroutine(curCoroutine);
                curCoroutine = StartCoroutine(ShakeSelf(shakeDuration, shakeOverInvoke));
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    IEnumerator ShakeSelf(float shakeDuration,Action overInvoke = null)
    {
        curDuration = shakeDuration;
        
        //数值限制
        if (shakeUnitDuration < 0.01f)
        {
            shakeUnitDuration = 0.01f;
        }
        if (shakeSumDuration < shakeUnitDuration)
        {
            shakeSumDuration = shakeUnitDuration;
        }

        while (true)
        {
            float deltaZ = Mathf.Sin(Time.unscaledTime / shakeUnitDuration * Mathf.PI * 2) * Mathf.Abs(shakeFactor);
            transform.localEulerAngles = startLocalEulerAngles + new Vector3(0, 0, deltaZ);
            yield return new WaitForSecondsRealtime(0.01f);
            curDuration -= 0.01f;
            if (curDuration <= 0)
            {
                transform.localEulerAngles = startLocalEulerAngles;
                OnShakeOver?.Invoke();
                overInvoke?.Invoke();
                break;
            }
        }

        if (curCoroutine!=null)
        {
            StopCoroutine(curCoroutine);
            curCoroutine = null;
        }
    }

    public void PrintShakeOver()
    {
        print("shakeOver");
    }

	}
}

