using System;
using LZLUnityTool.Plugins.InventoryPlugin.System.TaskSystem.Scripts.SO;

namespace LZLUnityTool.Plugins.InvntoryPlugin.System.TaskSystem.Scripts.Base
{
    public abstract class ObjectiveData
{
    public Objective Info { get; }

    public ObjectiveData(Objective objective)
    {
        Info = objective;
    }

    private int currentAmount;
    public int CurrentAmount
    {
        get
        {
            return currentAmount;
        }
        set
        {
            bool befCmplt = IsComplete;
            int befAmount = currentAmount;
            if (value < Info.Amount && value >= 0)
                currentAmount = value;
            else if (value < 0)
            {
                currentAmount = 0;
            }
            else currentAmount = Info.Amount;
            if (befAmount != currentAmount) OnStateChangeEvent?.Invoke(this, befCmplt);
        }
    }

    public string AmountString => $"{CurrentAmount}/{Info.Amount}";

    public bool IsComplete
    {
        get
        {
            if (currentAmount >= Info.Amount)
                return true;
            return false;
        }
    }

    public ObjectiveData prevObjective;
    public ObjectiveData nextObjective;

    public string entityID;

    public QuestData runtimeParent;

    public Action<ObjectiveData, bool> OnStateChangeEvent;

    protected virtual void UpdateAmountUp(int amount = 1)
    {
        if (IsComplete) return;
        if (!Info.InOrder) CurrentAmount += amount;
        else if (AllPrevObjCmplt) CurrentAmount += amount;
    }

    public bool AllPrevObjCmplt//判定所有前置目标是否都完成
    {
        get
        {
            ObjectiveData tempObj = prevObjective;
            while (tempObj != null)
            {
                if (!tempObj.IsComplete && tempObj.Info.OrderIndex < Info.OrderIndex)
                {
                    return false;
                }
                tempObj = tempObj.prevObjective;
            }
            return true;
        }
    }
    public bool HasNextObjOngoing//判定是否有后置目标正在进行
    {
        get
        {
            ObjectiveData tempObj = nextObjective;
            while (tempObj != null)
            {
                if (tempObj.CurrentAmount > 0 && tempObj.Info.OrderIndex > Info.OrderIndex)
                {
                    return true;
                }
                tempObj = tempObj.nextObjective;
            }
            return false;
        }
    }

    /// <summary>
    /// 可并行？
    /// </summary>
    public bool Parallel
    {
        get
        {
            if (!Info.InOrder) return true;//不按顺序，说明可以并行执行
            if (prevObjective && prevObjective.Info.OrderIndex == Info.OrderIndex) return true;//有前置目标，而且顺序码与前置目标相同，说明可以并行执行
            if (nextObjective && nextObjective.Info.OrderIndex == Info.OrderIndex) return true;//有后置目标，而且顺序码与后置目标相同，说明可以并行执行
            return false;
        }
    }

    public static implicit operator bool(ObjectiveData self)
    {
        return self != null;
    }
}

public class CollectObjectiveData : ObjectiveData
{
    public new CollectObjective Info
    {
        get
        {
            return base.Info as CollectObjective;
        }
    }

    public CollectObjectiveData(CollectObjective objective) : base(objective) { }

    public int amountWhenStart;

    public void UpdateCollectAmount(string id, int leftAmount)//得道具时用到
    {
        if (id == Info.ItemToCollect.GoodsId)
        {
            if (IsComplete) return;
            if (!Info.InOrder) CurrentAmount = leftAmount - (!Info.CheckBagAtStart ? amountWhenStart : 0);
            else if (AllPrevObjCmplt) CurrentAmount = leftAmount - (!Info.CheckBagAtStart ? amountWhenStart : 0);
        }
    }

    public void UpdateCollectAmountDown(string id, int leftAmount)//丢道具时用到
    {
        if (id == Info.ItemToCollect.GoodsId && AllPrevObjCmplt && !HasNextObjOngoing && Info.LoseItemAtSbmt)
            //前置目标都完成且没有后置目标在进行时，才允许更新；在提交任务时不需要提交相应道具，也不会更新减少值。
            CurrentAmount = leftAmount;
    }
}



public class TriggerObjectiveData : ObjectiveData
{
    public new TriggerObjective Info
    {
        get
        {
            return base.Info as TriggerObjective;
        }
    }

    public TriggerObjectiveData(TriggerObjective objective) : base(objective) { }

    public void UpdateTriggerState(string name, bool state)
    {
        if (name != Info.TriggerName) return;
        if (state) UpdateAmountUp();
        else if (AllPrevObjCmplt && !HasNextObjOngoing) CurrentAmount--;
    }
}
}

