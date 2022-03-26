using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace LZLToolBox.PlayerController
{
    [RequireComponent(typeof(Animator))]
    public class LZLAttackNormalAttribute : MonoBehaviour
    {
        [EnumToggleButtons]
        public SelfInpectorEnum _select;
        
        [Title("角色的动画状态机"),ShowIf("_select",SelfInpectorEnum.默认)]
        public Animator _selfAnimator;

        [Title("角色是否拥有武器"),ShowIf("_select",SelfInpectorEnum.默认)]
        public bool _hasWeapon;
        
        [Title("武器上的射线检测器"),ShowIf("_select",SelfInpectorEnum.默认)]
        [InfoBox("该字段不能为空",InfoMessageType.Error,"CheckMachineIsEmpty")]
        [ShowIf("_hasWeapon")]
        public LZLCheckPointManager _weaponCheckMachine;
        
        [BoxGroup("打击感")]
        [Title("命中停顿帧数"),ShowIf("_select",SelfInpectorEnum.默认)]
        [Range(0.01f,10f)]
        public float _pauseTime;
       
        [BoxGroup("打击感")]
        [Title("命中镜头晃动时长"),ShowIf("_select",SelfInpectorEnum.默认)]
        [Range(0.01f,10f)]
        public float _shakeDuration;
        
        [BoxGroup("打击感")]
        [Title("命中镜头晃动强度"),ShowIf("_select",SelfInpectorEnum.默认)]
        [Range(0.01f,10f)]
        public float _shakeStrength;
        
        [BoxGroup("通常普攻")]
        [Title("普通攻击是否可以被中断"),ShowIf("_select",SelfInpectorEnum.普攻相关)]
        public bool _norAtkCanBreakOff;
        
        [BoxGroup("通常普攻")]
        [Title("普攻以几种样式循环"),ReadOnly,ShowIf("_select",SelfInpectorEnum.普攻相关)]
        public int _nolAttackCycles;
        
        [BoxGroup("通常普攻")]
        [Title("普攻段数对应的动画播放器参数"),ShowIf("_select",SelfInpectorEnum.普攻相关)]
        [OnValueChanged("OnParasSizeChange")]
        public List<string> _nolAtkAnimParameters = new List<string>();
        
        [BoxGroup("通常普攻")]
        [Title("普攻是否有特效"),ShowIf("_select",SelfInpectorEnum.普攻相关)]
        public bool _useNolPatticle;
        
        [BoxGroup("通常普攻")]
        [Title("特效预制体数组"),ShowIf("_select",SelfInpectorEnum.普攻相关),ShowIf("_useNolPatticle",true)]
        public List<GameObject> _nolAtkPars = new List<GameObject>();

        [BoxGroup("暴击普攻")]
        [Title("暴击段数对应的动画播放器参数"),ShowIf("_select",SelfInpectorEnum.普攻相关)]
        public List<string> _criAtkAnimParameters = new List<string>();
        
        [BoxGroup("暴击普攻")]
        [Title("暴击是否有特效"),ShowIf("_select",SelfInpectorEnum.普攻相关)]
        public bool _useCriPatticle;
        
        [BoxGroup("暴击普攻")]
        [Title("特效预制体数组"),ShowIf("_select",SelfInpectorEnum.普攻相关)]
        [ShowIf("_useCriPatticle")]
        public List<GameObject> _criAtkPars = new List<GameObject>();

        
        [BoxGroup("技能相关")] 
        [Title("技能个数"),ReadOnly,ShowIf("_select",SelfInpectorEnum.技能相关)]
        public int _skillNum;
        
        [BoxGroup("技能相关")]
        [Title("技能属性"),ShowIf("_select",SelfInpectorEnum.技能相关)]
        [OnValueChanged("OnSkillAtbNumChange")]
        public List<LZLSkillAtb> _skillAtb;
        
        [BoxGroup("技能相关")]
        [Title("技能是否可以中途中断"),ShowIf("_select",SelfInpectorEnum.技能相关)]
        public bool _skillAtkCanBreakOff;
        
        [BoxGroup("技能相关")]
        [Title("存放技能特效的游戏体名称"),ShowIf("_select",SelfInpectorEnum.技能相关)]
        public string _skillobjParentName = "Skills";
        
        
        [BoxGroup("武器相关")] 
        [Title("相应武器的SO"),ShowIf("_select",SelfInpectorEnum.SO设置)]
        public WeaponState_SO _curWeapon;
        
        [BoxGroup("人物相关1")] 
        [Title("相应人物的SO"),ShowIf("_select",SelfInpectorEnum.SO设置)]
        public CharacterState_SO _curCharacter;

        
        private void Awake()
        {
            CanRunCheck();
            Init();
        }

        private void CanRunCheck()
        {
            string errorMsg = "无法启用战斗系统，请修复以下问题后再次启动：\n";
            bool canRun = true;
            if (_curWeapon == null)
            {
                canRun = false;
                errorMsg += "_curWeapon 未赋值，战斗系统无法启用，请修复后再运行。\n";
            }

            if (_curCharacter == null)
            {
                canRun = false;
                errorMsg += "_curCharacter 未赋值，战斗系统无法启用，请修复后再运行。\n";
            }

            if (transform.Find(_skillobjParentName) == null)
            {
                canRun = false;
                errorMsg += transform.name + "的技能仓库游戏体为创建，战斗系统无法启用，请按照skillobjParentName设置的名称创建游戏体后再运行。\n";
            }

            if (_hasWeapon && _weaponCheckMachine == null)
            {
                canRun = false;
                errorMsg += "武器检测器字段_weaponCheckMachine未赋值，请修复后再运行\n";
            }
            
            if (canRun == false)
            {
                Debug.LogError(errorMsg);
                GetComponent<LZLBaseAttackCtrl>().enabled = false;
            }
            else
            {
                GetComponent<LZLBaseAttackCtrl>().enabled = true;
            }
        }

        public void Init()
        {
            //清空技能容器的子物体
            var parTrans = transform.Find(_skillobjParentName);
            for (int i = 0; i < parTrans.childCount; i++)
            {
                DestroyImmediate(parTrans.GetChild(0));
            }
        }

        #region 工具函数

        /// <summary>
        /// 根据技能再数组中的id号获取技能信息
        /// </summary>
        /// <param name="skillIndex">技能号</param>
        /// <returns>技能信息类</returns>
        public LZLSkillAtb GetSkillById(int skillIndex)
        {
            //条件检测
            var checkArg = CheckSkillIdIsRight(skillIndex);
            if (!checkArg.checkResult)
            {
                return null;
            }

            return _skillAtb[skillIndex];
        }

        /// <summary>
        /// 检测对应的技能号是否正确
        /// </summary>
        /// <param name="skillIndex">被检查的技能id</param>
        /// <returns>检测结果</returns>
        public LZLCheckArg CheckSkillIdIsRight(int skillIndex)
        {
            LZLCheckArg result = new LZLCheckArg();
            //为负判断
            if (skillIndex < 0)
            {
                result.checkResult = false;
                result.errorMsg = "技能id为：" + skillIndex + "小于0。";
                return result;
            }
            //数组界限判断2 0,1
            if (_skillAtb.Count < skillIndex + 1)
            {
                result.checkResult = false;
                result.errorMsg = "错误信息：" + "技能数组个数为" + _skillAtb.Count + "。技能号" + skillIndex + "超出索引范围";
            }
            //未超过索引范围
            else
            {
                result.checkResult = true;
                result.errorMsg = "";
            }
            return result;
        }

        /// <summary>
        /// 获得当前角色技能存放容器的游戏体
        /// </summary>
        /// <returns>技能容器游戏体</returns>
        public GameObject GetSkillContainer()
        {
            GameObject skillParent = transform.Find(this._skillobjParentName) == null
                ? Instantiate(new GameObject(this._skillobjParentName),transform)
                : transform.Find(this._skillobjParentName).gameObject;
            return skillParent;
        }

        #endregion

        #region Inspector相关函数

        private void OnParasSizeChange()
        {
            _nolAttackCycles = _nolAtkAnimParameters.Count;
        }

        private void OnSkillAtbNumChange()
        {
            _skillNum = _skillAtb.Count;
        }

        private bool CheckMachineIsEmpty()
        {
            return _weaponCheckMachine == null;
        }

        #endregion
        
        public enum SelfInpectorEnum
        {
            默认,普攻相关,技能相关,SO设置
        }
    }

    [Serializable]
    /// <summary>
    /// 技能属性
    /// </summary>
    public class LZLSkillAtb
    {
        [Tooltip("动画触发参数名称")]
        public string _animParam;
        [Tooltip("法力值消耗")]
        public float _needMana;
        [Tooltip("技能冷却时间")]
        public float _coolTime;
        [Tooltip("技能特效预制体")]
        public GameObject _skillPrefab;

        /// <summary>
        /// 获得备份
        /// </summary>
        /// <returns></returns>
        public LZLSkillAtb GetSelfCopy()
        {
            LZLSkillAtb result = new LZLSkillAtb();
            result._animParam = _animParam;
            result._needMana = _needMana;
            result._coolTime = _coolTime;
            result._skillPrefab = _skillPrefab;
            return result;
        }
        /// <summary>
         /// 获得备份
         /// </summary>
         /// <param name="atb"></param>
        public void GetSelfCopy(ref LZLSkillAtb atb)
        {
            atb._animParam = _animParam;
            atb._needMana = _needMana;
            atb._coolTime = _coolTime;
            atb._skillPrefab = _skillPrefab;
        }
    }

    public class LZLCheckArg
    {
        /// <summary>
        /// 检测结果
        /// </summary>
        public bool checkResult;
        /// <summary>
        /// 错误信息
        /// </summary>
        public string errorMsg;
    }
}

