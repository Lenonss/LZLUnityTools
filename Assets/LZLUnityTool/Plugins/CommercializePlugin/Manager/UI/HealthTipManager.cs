using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using LZLUnityTool.Plugins.CommonPlugin;
using LZLUnityTool.Plugins.CommonPlugin.ExtructFunction;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HealthTipManager : SingletonMono<HealthTipManager>
{
    [EnumToggleButtons]
    public enum FuncType
    {
        仅标题,
        页面停留
    }
    [EnumToggleButtons]
    public FuncType myFuncType;
    
    
    [ShowIf("myFuncType",FuncType.仅标题)]
    public Text TitleText;

    [ShowIf("myFuncType",FuncType.页面停留),InfoBox("健康提示面板",InfoMessageType.None)]
    public GameObject heathTipPanel;
    [ShowIf("myFuncType",FuncType.页面停留),InfoBox("面板持续时长(秒)",InfoMessageType.None)]
    public float activeDuration = 2f;

    private void Start()
    {
        switch (myFuncType)
        {
            case FuncType.仅标题:
                CommonTool.SetActive(heathTipPanel, false);
                CommonTool.SetActive(TitleText.gameObject,true);
                break;
            case FuncType.页面停留:
                CommonTool.SetActive(TitleText.gameObject, false);
                CommonTool.SetActive(heathTipPanel,true);
                DOVirtual.DelayedCall(activeDuration, () => CommonTool.SetActive(heathTipPanel, false));
                break;
            default:
                break;
        }
    }
}
