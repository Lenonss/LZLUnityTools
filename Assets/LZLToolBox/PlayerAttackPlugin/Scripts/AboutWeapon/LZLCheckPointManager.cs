using System;
using System.Collections;
using System.Collections.Generic;
using LZLToolBox.PlayerController;
using Sirenix.OdinInspector;
using UnityEngine;

public class LZLCheckPointManager : MonoBehaviour
{
    #region 字段

    [ChildGameObjectsOnly]
    public List<LZLWeaponCheckPoint> _pointList;

    private List<int> _targets;

    public List<int> Targets
    {
        get
        {
            if (_targets == null)
            {
                _targets = new List<int>();
            }

            return _targets;
        }
    }

    /// <summary>
    /// 当前武器所属角色的控制器
    /// </summary>
    private LZLBaseAttackCtrl _selfAtkCtrl;
    #endregion


    private void Awake()
    {
        SetPointsManagerAtb();
    }

    /// <summary>
    /// <para>开启检测点的攻击检测</para>
    /// <para>由普攻动画所带脚本调用，方式为sendMessage</para>
    /// </summary>
    public void StartTargetCheck(LZLBaseAttackCtrl atker)
    {
        //赋值
        _selfAtkCtrl = atker;
        //清空
        Targets.Clear();
        
        //开启检测点的检测
        foreach (var point in _pointList)
        {
            point.SetCanCheckState(true);
        }
    }

    /// <summary>
    /// <para>关闭检测点的攻击检测</para>
    /// <para>由普攻动画所带脚本调用，方式为sendMessage</para>
    /// <para>进行了伤害计算函数的调用</para>
    /// </summary>
    public void EndTargetCheck()
    {
        //关闭检测点
        foreach (var point in _pointList)
        {
            point.SetCanCheckState(false);
        }
        
        //清空目标数组
        Targets.Clear();
        
        //CurData置空
        _selfAtkCtrl.ClearCurAtkData();
        // //伤害给予
        // atker.CurAtkData.mCurTargets.Clear();
        // try
        // {
        //     if (atker.CurAtkData.mCurTargets==null)
        //     {
        //         atker.CurAtkData.mCurTargets = new List<LZLBaseAttackCtrl>();
        //     }
        //     foreach (var id in Targets)
        //     {
        //         atker.CurAtkData.mCurTargets.Add(LZLAtkCenterManager.Instance.GetBaseAttackVtrlById(id));
        //     }
        //     atker.GiveDmg_CloseCombat_Anim();
        // }
        // catch (Exception e)
        // {
        //     Debug.LogError(e);
        // }
        // finally
        // {
        //     //关闭检测点
        //     foreach (var point in _pointList)
        //     {
        //         point.SetCanCheckState(false);
        //     }
        //
        //     //清空目标数组
        //     Targets.Clear();
        // }
    }

    /// <summary>
    /// 检测对应id的目标是否已被记录
    /// </summary>
    /// <param name="id">目标id号</param>
    /// <returns></returns>
    private bool CheckTargetContain(int id)
    {
        if (Targets.Contains(id))
        {
            return true;
        }

        return false;
    }

    public LZLCheckArg GiveDamage(int id)
    {
        LZLCheckArg result = new LZLCheckArg();
        if (CheckTargetContain(id))
        {
            result.checkResult = false;
            return result;
        }
        //给予伤害
        var def = LZLAtkCenterManager.Instance.GetBaseAttackVtrlById(id);
        SampleDmgArg atk = new SampleDmgArg()
        {
            atkerId = _selfAtkCtrl.id,
            critical = _selfAtkCtrl.CurAtkData.mCurAtkCritical,
            criticalDmg = _selfAtkCtrl.AtkAttribute._curWeapon.Additional_Critical_Damage +
                          _selfAtkCtrl.AtkAttribute._curCharacter.Critical_Damage,
        };
        def.GetDamage(atk);
        //记录目标
        _targets.Add(id);

        result.checkResult = true;
        return result;
    }

    /// <summary>
    /// 设置检测点的管理器属性
    /// </summary>
    private void SetPointsManagerAtb()
    {
        foreach (var point in _pointList)
        {
            point.SetPointManager(this);
        }
    }
}
