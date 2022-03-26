using System;
using System.Collections;
using System.Collections.Generic;
using LZLToolBox.PlayerController;
using UnityEngine;

public class LZLBaseSkillGOBJ : MonoBehaviour
{
    public ParticleSystem _skillPar;
    [Tooltip("多久后反激活粒子特效")]
    public float unActiveTime;
    [Tooltip("激活于反激活的游戏体集合。未赋值则默认该游戏体下所有子物体")]
    public List<GameObject> _actAndUnActGobjs;
    /// <summary>
    /// 技能信息
    /// </summary>
    protected LZLSkillAtb _skillAtb;
    /// <summary>
    /// 自身的攻击控制器
    /// </summary>
    internal LZLBaseAttackCtrl _selfAtkCtrl;
    [HideInInspector]
    public bool coolOver = true;

    /// <summary>
    /// 是否进行了技能攻击前检测
    /// </summary>
    private bool _atkChecked = false;
    /// <summary>
    /// 实时的冷却时间
    /// </summary>
    private float currentCoolTime;



    #region 生命周期函数

    protected void Awake()
    {
        
        //默认初始化受控制的游戏体集合
        if (_actAndUnActGobjs.Count == 0) 
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                _actAndUnActGobjs.Add(transform.GetChild(i).gameObject);
            }
        }
    }

    #endregion
    
    
    /// <summary>
    /// 检测是否能够释放技能，更新技能信息
    /// </summary>
    /// <param name="attacker">施法者</param>
    /// <param name="skillAtb">技能信息类</param>
    /// <returns>检测结果</returns>
    public LZLCheckArg CanAttackCheck(LZLBaseAttackCtrl attacker,LZLSkillAtb skillAtb)
    {
        LZLCheckArg result = new LZLCheckArg();
        
        //atb赋值
        if (_skillAtb == null)
        {
            _skillAtb = skillAtb.GetSelfCopy();
        }
        //atkCtrl赋值
        if (_selfAtkCtrl == null)
        {
            _selfAtkCtrl = attacker;
        }
        //判断法力值是否足够
        if (attacker.CurMana < skillAtb._needMana)
        {
            result.checkResult = false;
            result.errorMsg = "法力值不够，当前：" + attacker.CurMana + "  需要：" + skillAtb._needMana;
            return result;
        }
        //判断是否冷却完
        if (!coolOver)
        {
            result.checkResult = false;
            result.errorMsg = "错误信息：技能未冷却完。剩余冷却时间" + GetCurrentCoolTime() + "s";
            return result;
        }

        result.checkResult = true;
        result.errorMsg = "";
        
        _atkChecked = true;
        return result;
    }

    /// <summary>
    /// 开始技能释放
    /// </summary>
    /// <param name="attacker">施法者的LZLBaseAttackCtrl</param>
    /// <param name="skillAtb">技能属性</param>
    /// <param name="skillAnimAct">释放技能的动画</param>
    /// <returns></returns>
    public bool StartSkillParticle(LZLBaseAttackCtrl attacker)
    {
        if (!_atkChecked)
        {
            Debug.LogError("错误信息：技能释放前检测未通过或者未进行技能释放前检测。\n相关函数LZLSkillGOBJ.CanAttackCheck");
            return false;
        }
        //判断是否绑定了粒子特效
        if (_skillPar==null)
        {
            Debug.LogError(transform.name+"的_skillPar未赋值");
            return false;
        }
        //开始了一次特效释放，检测置false
        _atkChecked = false;
        //显示粒子特效
        StartCoroutine(UnActiveSkillPar());
        //冷却开始
        StartCoroutine(StartCool());
        //消耗法力值
        ManaCost(attacker);
        return true;
    }

    /// <summary>
    /// 反激活技能特效
    /// </summary>
    /// <returns></returns>
    IEnumerator UnActiveSkillPar()
    {
        //激活特效
        SkillParticle_ActiveCtrl(true);
        //等待
        yield return new WaitForSecondsRealtime(unActiveTime);
        SkillParticle_ActiveCtrl(false);
    }

    /// <summary>
    /// 开始冷却倒计时
    /// </summary>
    /// <returns></returns>
    IEnumerator StartCool()
    {
        coolOver = false;
        currentCoolTime = _skillAtb._coolTime;
        while (true)
        {
            yield return new WaitForSecondsRealtime(0.01f);
            currentCoolTime -= 0.01f;
            if (currentCoolTime <= 0)
            {
                currentCoolTime = 0;
                break;
            }
        }

        coolOver = true;
    }

    /// <summary>
    /// 获取实时的冷却时间
    /// </summary>
    /// <returns></returns>
    public float GetCurrentCoolTime()
    {
        currentCoolTime = currentCoolTime <= 0 ? 0 : currentCoolTime;
        return currentCoolTime;
    }

    /// <summary>
    /// 消耗法力值
    /// </summary>
    /// <param name="attacker">施法者</param>
    public void ManaCost(LZLBaseAttackCtrl attacker)
    {
        attacker.CurMana = (attacker.CurMana - _skillAtb._needMana) > 0 ? (attacker.CurMana - _skillAtb._needMana) : 0;
    }

    #region 重写

    /// <summary>
    /// 获得技能的伤害值
    /// </summary>
    public virtual LZLSkillDamageArg GetDamageValue(float magicRatio, float physicRatio, AtkDamageType dmgType)
    {
        return null;
    }

    #endregion

    
    
    #region 工具函数

    /// <summary>
    /// <para>控制actAndUnActGobjs记录的游戏体集合的激活状态</para>
    /// <para>如果为手动给游戏体集合赋值，默认所有子物体</para>
    /// </summary>
    /// <param name="act">激活状态</param>
    public void SkillParticle_ActiveCtrl(bool act)
    {
        foreach (var item in _actAndUnActGobjs)
        {
            item.SetActive(act);
        }
    }

    #endregion
    public class LZLSkillDamageArg
    {
        public AtkDamageType dmgType;
        public float physicDamge;
        public float magicDamage;
    }
}
