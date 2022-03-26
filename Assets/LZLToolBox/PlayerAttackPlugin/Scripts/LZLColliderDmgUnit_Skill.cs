using System;
using System.Collections;
using System.Collections.Generic;
using LZLToolBox.PlayerController;
using UnityEngine;

public class LZLColliderDmgUnit_Skill : MonoBehaviour
{
    /// <summary>
    /// 技能游戏体脚本
    /// </summary>
    protected LZLBaseSkillGOBJ _SelfSkillGobj;

    [Tooltip("攻击的伤害类型")]
    public AtkDamageType _damageType;
    
    [Tooltip("法术加成比例")]
    public float _magicRatio;

    [Tooltip("物理加成比例")]
    public float _physicRatio;

    /// <summary>
    /// 攻击方id
    /// </summary>
    public int AtkId
    {
        get
        {
            if (_SelfSkillGobj == null)
            {
                return -1;
            }
            return _SelfSkillGobj._selfAtkCtrl.id;
        }
    }

    private void Awake()
    {
        CanRunCheck();
    }

    private void CanRunCheck()
    {
        bool result = true;
        string msg = "";
        
        //SkillGOBJ
        if (_SelfSkillGobj == null)
        {
            if (transform.parent.GetComponent<LZLBaseSkillGOBJ>())
            {
                _SelfSkillGobj = transform.parent.GetComponent<LZLBaseSkillGOBJ>();
            }
            else
            {
                result = false;
                msg += "未能获取LZLBaseSkillGOBJ类对象，请将当前游戏体置为带有LZLBaseSkillGOBJ类或者其子类的游戏体的第一级子物体。\n";
            }
        }
        
        //has collider ?
        if (GetComponent<Collider>() == null)
        {
            result = false;
            msg += "当前游戏体未设置碰撞器：" + name;
        }

        if (result = false)
        {
            Debug.LogError(msg);
            this.enabled = false;
        }
    }

    /// <summary>
    /// 获得技能的伤害值
    /// </summary>
    /// <returns>包含伤害类型，对应的伤害值</returns>
    public LZLBaseSkillGOBJ.LZLSkillDamageArg GetSkillDmgValue()
    {
        var result = _SelfSkillGobj.GetDamageValue(_magicRatio, _physicRatio, _damageType);
        return result;
    }
}
