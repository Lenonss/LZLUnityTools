using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public abstract class UISettlePanelCtrl_Base : MonoBehaviour
{
    public enum PanelType
    {
        胜利,
        失败
    }

    [EnumToggleButtons]
    public PanelType thePanelType;
    
    [BoxGroup("翻倍领取"),InfoBox("翻倍倍数",InfoMessageType.None),
     ShowIf("thePanelType",PanelType.胜利)]
    public int mutilplyNum = 3;
    [BoxGroup("翻倍领取"),InfoBox("翻倍按钮",InfoMessageType.None),
     ShowIf("thePanelType",PanelType.胜利)]
    public Button mutiplyButton;
    [BoxGroup("翻倍领取"),InfoBox("基础奖励数",InfoMessageType.None),
     ShowIf("thePanelType",PanelType.胜利)]
    public float defaultReward;
    
    
    [BoxGroup("跳转按钮"),ShowIf("thePanelType",PanelType.胜利)]
    public Button GoAheadButton;
    [BoxGroup("跳转按钮"),ShowIf("thePanelType",PanelType.胜利),InfoBox("多长时间显示",InfoMessageType.None)]
    public float unitilActTime_GoAhead = 3f;
    [BoxGroup("跳转按钮"),ShowIf("thePanelType",PanelType.失败)]
    public Button GiveUpButton;
    [BoxGroup("跳转按钮"),ShowIf("thePanelType",PanelType.失败),InfoBox("多长时间显示",InfoMessageType.None)]
    public float unitilActTime_GiveUp = 3f;

    protected void Awake()
    {
        InitButtonFunc();
    }

    protected virtual void OnEnable()
    {
        InitDataOnEnable();

        if (thePanelType == PanelType.胜利)
        {
            ShowGoAheadButton();
        }
        else if(thePanelType == PanelType.失败)
        {
            ShowGiveUpButton();
        }
    }

    protected virtual void ShowGoAheadButton()
    {
        SetActive(GoAheadButton.gameObject,false);
        StartCoroutine(DelayToCall(unitilActTime_GoAhead, (() =>
        {
            SetActive(GoAheadButton.gameObject,true);
        })));
    }

    protected virtual void ShowGiveUpButton()
    {
        SetActive(GiveUpButton.gameObject,false);
        StartCoroutine(DelayToCall(unitilActTime_GiveUp, (() =>
        {
            SetActive(GiveUpButton.gameObject,true);
        })));
    }

    protected virtual void InitButtonFunc()
    {
        if (thePanelType == PanelType.胜利)
        {
            if (mutiplyButton!=null)
            {
                mutiplyButton.onClick.RemoveListener(MutiplyRecieveButtonFunc);
                mutiplyButton.onClick.AddListener(MutiplyRecieveButtonFunc);
            }

            if (GoAheadButton != null)
            {
                GoAheadButton.onClick.RemoveListener(GoAheadButtonFunc);
                GoAheadButton.onClick.AddListener(GoAheadButtonFunc);
            }
        }
        else if(thePanelType == PanelType.失败)
        {
            if (GiveUpButton != null)
            {
                GiveUpButton.onClick.RemoveListener(GiveUpButtonFunc);
                GiveUpButton.onClick.AddListener(GiveUpButtonFunc);
            }
        }
    }

    /// <summary>
    /// 让文本播放数值增加的动画
    /// </summary>
    /// <param name="text">文本组件</param>
    /// <param name="startValue">起始值</param>
    /// <param name="targetValue">目标值</param>
    /// <param name="unitDuration">间隔时间</param>
    /// <param name="ignoreScale">是否忽略timeScale的影响</param>
    protected virtual void CurrencyAnimUp(ref Text text,int startValue,int targetValue,float unitDuration,bool ignoreScale = true)
    {
        StartCoroutine(IECurrencyAnimUp(text, startValue, targetValue, unitDuration, ignoreScale));
    }

    private IEnumerator IECurrencyAnimUp(Text text, int startValue, int targetValue, float unitDuration,bool ignoreScale = true)
    {
        var realTime = new WaitForSecondsRealtime(unitDuration);
        var time = new WaitForSeconds(unitDuration);
        text.text = startValue.ToString();
        while (true)
        {
            startValue += 1;
            text.text = startValue.ToString();
            if (ignoreScale)
            {
                yield return realTime;
            }
            else
            {
                yield return time;
            }
            
            if (startValue == targetValue)
            {
                break;
            }
        }
        
    }

    /// <summary>
    /// OnEnable初始化数据
    /// </summary>
    protected abstract void InitDataOnEnable();
    
    /// <summary>
    /// 继续按钮
    /// </summary>
    protected abstract void GoAheadButtonFunc();

    /// <summary>
    /// 翻倍领取
    /// </summary>
    protected abstract void MutiplyRecieveButtonFunc();

    /// <summary>
    /// 放弃功能
    /// </summary>
    protected abstract void GiveUpButtonFunc();

    private IEnumerator DelayToCall(float delayTime,Action call)
    {
        yield return new WaitForSecondsRealtime(delayTime);
        call?.Invoke();
    }

    /// 设置游戏体激活状态
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="value"></param>
    public static void SetActive(GameObject gameObject, bool value)
    {
        if (!gameObject) return;
        if (gameObject.activeSelf != value) gameObject.SetActive(value);
    }
}
