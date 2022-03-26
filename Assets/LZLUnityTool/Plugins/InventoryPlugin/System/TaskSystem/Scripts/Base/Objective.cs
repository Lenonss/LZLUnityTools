using System;
using LZLUnityTool.Plugins.InventoryPlugin.System.TaskSystem.Scripts.SO;
using LZLUnityTool.Plugins.InventoryPlugin.System.TaskSystem.Scripts.SO.Charactor;
using UnityEngine;

namespace LZLUnityTool.Plugins.InvntoryPlugin.System.TaskSystem.Scripts.Base
{
/// <summary>
/// 任务目标
/// </summary>
[Serializable]
public abstract class Objective
{
    [SerializeField]
    protected string displayName = string.Empty;
    public string DisplayName => displayName;

    [SerializeField]
    protected bool display = true;
    public bool Display
    {
        get
        {
            return display;
        }
    }

    [SerializeField]
    protected bool canNavigate = true;
    public bool CanNavigate => this is CollectObjective || this is TriggerObjective ? false : canNavigate;

    [SerializeField]
    protected bool showMapIcon = true;
    public bool ShowMapIcon => (this is CollectObjective || this is TriggerObjective) ? false : showMapIcon;

    [SerializeField]
    protected DestinationInformation auxiliaryPos;
    /// <summary>
    /// 辅助位置，用于地图图标、导航等
    /// </summary>
    public DestinationInformation AuxiliaryPos
    {
        get
        {
            return auxiliaryPos;
        }
    }

    [SerializeField]
    protected int amount = 1;
    public int Amount => amount;

    [SerializeField]
    protected bool inOrder;
    public bool InOrder => inOrder;

    [SerializeField]
    protected int orderIndex = 1;
    public int OrderIndex => orderIndex;

    public virtual bool IsValid
    {
        get
        {
            return amount > 0;
        }
    }

    public static implicit operator bool(Objective self)
    {
        return self != null;
    }
}
/// <summary>
/// 收集类目标
/// </summary>
[Serializable]
public class CollectObjective : Objective
{
    [SerializeField]
    private BaseGoodsSO itemToCollect;
    public BaseGoodsSO ItemToCollect => itemToCollect;

    [SerializeField]
    private bool checkBagAtStart = true;
    /// <summary>
    /// 是否在目标开始执行时检查背包道具看是否满足目标，否则目标重头开始计数
    /// </summary>
    public bool CheckBagAtStart => checkBagAtStart;

    [SerializeField]
    private bool loseItemAtSbmt = true;
    /// <summary>
    /// 是否在提交任务时失去相应道具
    /// </summary>
    public bool LoseItemAtSbmt => loseItemAtSbmt;

    public override bool IsValid
    {
        get
        {
            return base.IsValid && itemToCollect && (!showMapIcon || showMapIcon && auxiliaryPos);
        }
    }
}

/// <summary>
/// 触发器目标
/// </summary>
[Serializable]
public class TriggerObjective : Objective
{
    [SerializeField]
    private string triggerName;
    public string TriggerName
    {
        get
        {
            return triggerName;
        }
    }

    [SerializeField]
    private bool stateToCheck;
    public bool StateToCheck => stateToCheck;

    [SerializeField]
    private bool checkStateAtAcpt = true;//用于标识是否在接取任务时检触发器状态看是否满足目标，否则目标重头开始等待触发
    public bool CheckStateAtAcpt
    {
        get
        {
            return checkStateAtAcpt;
        }
    }

    public override bool IsValid
    {
        get
        {
            return base.IsValid && !string.IsNullOrEmpty(triggerName);
        }
    }
}
}