using System.Collections.Generic;

namespace LZLUnityTool.Plugins.InvntoryPlugin.System.TaskSystem.Scripts.Base
{
    public enum QuestState
{
    NotAccept,
    InProgress,
    Complete,
    Finished
}

public class QuestData
{
    public Quest Info { get; }

    public List<ObjectiveData> ObjectiveInstances { get; } = new List<ObjectiveData>();
    

    public int latestHandleDays;

    public bool InProgress { get; set; }//任务是否正在执行，在运行时用到

    /// <summary>
    /// 所以目标是否都已达成
    /// </summary>
    public bool IsComplete
    {
        get
        {
            if (ObjectiveInstances.Exists(x => !x.IsComplete))
                return false;
            return true;
        }
    }

    /// <summary>
    /// 是否已经提交完成
    /// </summary>
    public bool IsFinished
    {
        get
        {
            return IsComplete && !InProgress;
        }
    }

    public QuestData(Quest quest)
    {
        Info = quest;
        foreach (Objective objective in Info.Objectives)
        {
            if (objective is CollectObjective co)
            { if (co.IsValid) ObjectiveInstances.Add(new CollectObjectiveData(co)); }
            else if (objective is TriggerObjective tgo)
            { if (tgo.IsValid) ObjectiveInstances.Add(new TriggerObjectiveData(tgo)); }
        }
        ObjectiveInstances.Sort((x, y) =>
        {
            if (x.Info.OrderIndex > y.Info.OrderIndex) return 1;
            else if (x.Info.OrderIndex < y.Info.OrderIndex) return -1;
            else return 0;
        });
        if (this.Info.CmpltObjctvInOrder)
            for (int i = 1; i < ObjectiveInstances.Count; i++)
            {
                if (ObjectiveInstances[i].Info.OrderIndex >= ObjectiveInstances[i - 1].Info.OrderIndex)
                {
                    ObjectiveInstances[i].prevObjective = ObjectiveInstances[i - 1];
                    ObjectiveInstances[i - 1].nextObjective = ObjectiveInstances[i];
                }
            }
        int i1, i2, i3, i4, i5, i6;
        i1 = i2 = i3 = i4 = i5 = i6 = 0;
        foreach (ObjectiveData o in ObjectiveInstances)
        {
            if (o.Info is CollectObjective)
            {
                o.entityID = quest.ID + "_CO" + i1;
                i1++;
            }
            if (o.Info is TriggerObjective)
            {
                o.entityID = quest.ID + "_CUO" + i6;
                i6++;
            }
            o.runtimeParent = this;
        }
    }

    public static implicit operator bool(QuestData self)
    {
        return self != null;
    }
}
}

