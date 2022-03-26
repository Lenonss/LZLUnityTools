using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LZLToolBox.CommonPlugin.Interfaces;
using LZLToolBox.CommonPlugin.Tag;
using LZLToolBox.PlayerController;
using Unity.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LZLToolBox.PlayerController
{
    [RequireComponent(typeof(LZLAttackNormalAttribute))]
    public class LZLBaseAttackCtrl : MonoBehaviour,ISubscribeEvent
    {
        /// <summary>
        /// 攻击相关的属性
        /// </summary>
        private LZLAttackNormalAttribute _atkAttribute;
        
        /// <summary>
        /// 攻击相关的属性
        /// </summary>
        public LZLAttackNormalAttribute AtkAttribute
        {
            get
            {
                if (_atkAttribute == null)
                {
                    _atkAttribute = this.GetComponent<LZLAttackNormalAttribute>();
                }

                return _atkAttribute;
            }
        }

        #region 运行中用到的参数

        /// <summary>
        /// 当前LZLBaseAtkCtrl的注册id
        /// </summary>
        [ReadOnly] public int id = -1;
        
        /// <summary>
        /// 上次攻击的时间点
        /// </summary>
        protected float lastAtkTime = 0;

        /// <summary>
        /// 当前的武器配置
        /// </summary>
        protected WeaponState_SO curWeapon;
        
        /// <summary>
        /// 当前普攻连击次数
        /// </summary>
        protected int curNolAtkRank = 0;
        
        /// <summary>
        /// 当前普攻样式ID
        /// </summary>
        protected int curNolAtkId = 0;

        /// <summary>
        /// 当前的生命值
        /// </summary>
        [HideInInspector]public float _curHealth = float.MinValue;

        /// <summary>
        /// 当前的法力值
        /// </summary>
        private float _curMana = float.MinValue;
        /// <summary>
        /// 当前的护盾值
        /// </summary>
        private float _curShiled = float.MinValue;
        /// <summary>
        /// 当前的物理攻击力
        /// </summary>
        private float _curPhysicAtk = float.MinValue;
        /// <summary>
        /// 当前的魔法攻击力
        /// </summary>
        private float _curMagicAtk = float.MinValue;
        /// <summary>
        /// 是否处于攻击状态
        /// </summary>
        protected bool _attacking = false;

        /// <summary>
        /// 记录技能游戏体的图表
        /// </summary>
        private Dictionary<int, GameObject> skillObjMap = new Dictionary<int, GameObject>();

        private AttackTargetArg _curAtkData;
        #endregion

        #region 外部显示参数
        [Header("攻击范围"),Tooltip("是否显示可攻击范围")]
        public bool _showAttackRange = false;
        [Tooltip("显示攻击范围的颜色")]
        public Color _atkRangeColor;
        #endregion

        #region 公用参数
        /// <summary>
        /// 当前的生命值
        /// </summary>
        public float CurHealth 
        {
            get
            {
                if (_curHealth == float.MinValue)
                {
                    _curHealth = AtkAttribute._curCharacter.Basic_Health +
                                 AtkAttribute._curWeapon.Additional_Health;
                }

                return _curHealth;
            }
            set
            {
                _curHealth = value;
            }

        }
        /// <summary>
        /// 当前的法力值
        /// </summary>
        public float CurMana
        {
            get
            {
                if (_curMana == float.MinValue)
                {
                    _curMana = AtkAttribute._curCharacter.Mana +
                               AtkAttribute._curWeapon.Additional_Mana;
                }

                return _curMana;
            }
            set
            {
                _curMana = value;
            }

        }
        /// <summary>
        /// 当前的护盾值
        /// </summary>
        public float CurShiled
        {
            get
            {
                if (_curShiled == float.MinValue)
                {
                    _curShiled = AtkAttribute._curCharacter.Shiled;
                }

                return _curShiled;
            }
            set
            {
                _curShiled = value;
            }
        }
        /// <summary>
        /// 当前的物理攻击力
        /// </summary>
        public float CurPhysicAtk
        {
            get
            {
                if (_curPhysicAtk == float.MinValue)
                {
                    _curPhysicAtk = AtkAttribute._curCharacter.Physic_Attack + AtkAttribute._curWeapon.GetTypeAtkValue(true)
                        +_buffPlus_PhysicAtk;
                }
                return _curPhysicAtk;
            }
            set
            {
                _curPhysicAtk = value;
            }
        }
        /// <summary>
        /// 当前的魔法攻击力
        /// </summary>
        public float CurMagicAtk
        {
            get
            {
                if (_curMagicAtk == float.MinValue)
                {
                    _curMagicAtk = AtkAttribute._curCharacter.Magic_Attack 
                                   + AtkAttribute._curWeapon.GetTypeAtkValue(false)
                                   +_buffPlus_MagicAtk;
                }
                return _curMagicAtk;
            }
            set
            {
                _curMagicAtk = value;
            }
        }
        /// <summary>
        /// 增幅的物理攻击力
        /// </summary>
        public float _buffPlus_PhysicAtk = 0;
        /// <summary>
        /// 增幅的魔法攻击力
        /// </summary>
        public float _buffPlus_MagicAtk = 0;
        /// <summary>
        /// 实时攻击数据
        /// </summary>
        public AttackTargetArg CurAtkData
        {
            get
            {
                return _curAtkData;
            }
            set => _curAtkData = value;
        }

        #endregion
        
        /// <summary>
        /// 检测球形范围内的碰撞器来找到攻击对象
        /// </summary>
        /// <param name="center"></param>
        /// <param name="radius"></param>
        /// <param name="tag"></param>
        public List<LZLBaseAttackCtrl> FindAtkTargetByOverlapSphere(Vector3 center,float radius,UnityEnumTag tag)
        {
            var targets = Physics.OverlapSphere(center, radius);
            List<LZLBaseAttackCtrl> result = new List<LZLBaseAttackCtrl>();
            if (targets == null || targets.Length == 0)
            {
                return null;
            }

            foreach (var item in targets)
            {
                if (item.tag.ToString() == tag.ToString() && item.gameObject.GetComponent<LZLBaseAttackCtrl>())
                {
                    result.Add(item.gameObject.GetComponent<LZLBaseAttackCtrl>());
                }
            }
            
            return result;
        }
        
        #region Unity函数

        protected void Awake()
        {
            //注册自己
            var result = LZLAtkCenterManager.Instance.RegisterAtkCtrl(this);
            if (!result.RegistSuccess)
            {
                Debug.LogError(this.name+" LZLBaseAttackCtrl注册失败！");
                return;
            }

            id = result.id;
            //事件订阅
            SubscribeEvent();
        }

        protected void OnCollisionEnter(Collision other)
        {
            Debug.Log("in");
            //技能碰撞体检测,并进行伤害计算
            var colliderDmgUnit_skill = other.transform.GetComponent<LZLColliderDmgUnit_Skill>();
            if (colliderDmgUnit_skill != null)
            {
                var atkCtrl = LZLAtkCenterManager.Instance.GetBaseAttackVtrlById(colliderDmgUnit_skill.AtkId);
                var atker = BornAtkDamageArgByLZLBaseAttackCtrl(atkCtrl);
                var defer = BornAtkDamageArgByLZLBaseAttackCtrl(this);
                var skillDmg = colliderDmgUnit_skill.GetSkillDmgValue();
                atker.mAttack_Physic = skillDmg.physicDamge;
                atker.mDefence_Magic = skillDmg.magicDamage;
                CalculateDamage(atker,defer,skillDmg.dmgType);
            }
        }

        protected void OnTriggerEnter(Collider other)
        {
            //技能碰撞体检测,并进行伤害计算
            var colliderDmgUnit_skill = other.transform.GetComponent<LZLColliderDmgUnit_Skill>();
            if (colliderDmgUnit_skill != null)
            {
                var atkCtrl = LZLAtkCenterManager.Instance.GetBaseAttackVtrlById(colliderDmgUnit_skill.AtkId);
                var atker = BornAtkDamageArgByLZLBaseAttackCtrl(atkCtrl);
                var defer = BornAtkDamageArgByLZLBaseAttackCtrl(this);
                var skillDmg = colliderDmgUnit_skill.GetSkillDmgValue();
                atker.mAttack_Physic = skillDmg.physicDamge;
                atker.mDefence_Magic = skillDmg.magicDamage;
                CalculateDamage(atker,defer,skillDmg.dmgType);
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (_showAttackRange)
            {
                var oriColor = Gizmos.color;
                Gizmos.color = _atkRangeColor;
                //绘制攻击检测范围
                Gizmos.DrawWireSphere(transform.position,AtkAttribute._curWeapon.Attack_Range);
                Gizmos.color = oriColor;
            }
        }

        protected void OnDestroy()
        {
            UnSubscribeEvent();
        }

        #endregion
        
        /// <summary>
        /// 攻击
        /// </summary>
        /// <param name="atkType"></param>
        /// <returns></returns>
        public bool AttackTarget(int skillIndex = 0)
        {
            if (_attacking)
            {
                return false;
            }

            if (CurAtkData == null)
            {
                Debug.LogError("CurAtkData不能为空，请赋值后再调用该函数——AttackTArget");
                return false;
            }
            switch (CurAtkData.mAtkType)
            {
                case AtkType.normalAtk:
                    return nolAtk();
                case  AtkType.smiteAtk:
                    return false;
                case AtkType.SkillAtk:
                    return skillAtk(skillIndex);
                default:
                    return false;
            }
        }

        private bool nolAtk()
        {
            //初始化武器配置字段
            InitCurWeapon();
            //判断能否攻击，目前只考虑冷却时间
            var deltaTime = Time.unscaledTime - lastAtkTime;
            if (deltaTime < curWeapon.Attack_Cool_Time)
            {
                return false;//攻击失败
            }
            //判断是否配置了普攻相关参数
            if (AtkAttribute._nolAtkAnimParameters.Count == 0)
            {
               Debug.LogError("未配置错误：普攻相关内容未进行配置，请在\nLZLAttackNormalAttributes里配置完后再运行。");
               return false;
            }
            //检查普攻动画参数是否正确
            var checkArg1 = CheckAnimatorParaContain(AtkAttribute._selfAnimator,
                AtkAttribute._nolAtkAnimParameters[curNolAtkId]);
            if (!checkArg1.checkResult)
            {
                Debug.LogError(checkArg1.errorMsg);
                return false;
            }
            //检查暴击攻击动画参数是否正确
            var canPlayCriticalAtkAnim = CheckAnimatorParaContain(AtkAttribute._selfAnimator,
                AtkAttribute._criAtkAnimParameters[curNolAtkId]).checkResult;

            //更新最新攻击时间
            lastAtkTime = Time.unscaledTime;
            
            //*******播放攻击动画******
            var atkAnimParam = _atkAttribute._nolAtkAnimParameters[curNolAtkId];
            bool atkCritical = false;
            //检查攻击是否暴击
            if (atkCritical = CheckCritical())
            {
                if (canPlayCriticalAtkAnim)
                {
                    atkAnimParam = AtkAttribute._criAtkAnimParameters[curNolAtkId];//更新动画参数
                }
                else
                {
                    Debug.LogWarning("动画参数警告：当此普攻暴击了，但是没有配置对应的暴击动画参数。默认使用普攻动画。\n"+
                                     "如果需要在暴击时进行不一样的攻击，请在配置脚本中配置。");
                }
            }
            //播放攻击动画,采用触发器方式
            _atkAttribute._selfAnimator.SetTrigger(atkAnimParam);
            UpNolAtkID();
            curNolAtkRank += 1;
            
            //暴击率赋值
            CurAtkData.mCurAtkCritical = atkCritical;
            return true;
        }

        private bool skillAtk(int skillIndex)
        {
            //初始化武器配置字段
            InitCurWeapon();
            //技能号检测
            var checkArg = AtkAttribute.CheckSkillIdIsRight(skillIndex);
            if (!checkArg.checkResult)
            {
                Debug.LogError(checkArg.errorMsg);
                return checkArg.checkResult;
            }

            //获取技能信息类
            var skillAtb = AtkAttribute.GetSkillById(skillIndex);
            if (skillAtb._skillPrefab == null)
            {
                Debug.LogError(transform.name +"的技能"+skillAtb+"的技能预制体未设置");
                return false;
            }
            
            //获取技能游戏体容器，不存在则创建
            GameObject skillContainer = AtkAttribute.GetSkillContainer();
            
            //获得技能游戏体
            GameObject skill = GetSkillObjBySkillId_CanCreate(skillIndex, skillAtb, skillContainer.transform);
            skill.GetComponent<LZLBaseSkillGOBJ>().SkillParticle_ActiveCtrl(false);
            //判断能否释放技能
            var checkArg2 = skill.GetComponent<LZLBaseSkillGOBJ>().CanAttackCheck(this, skillAtb);
            if (!checkArg2.checkResult)
            {
                Debug.LogError(checkArg.errorMsg);
                return checkArg2.checkResult;
            }
            //播放技能动画
            else
            {
                var checkArg3 = CheckAnimatorParaContain(AtkAttribute._selfAnimator, skillAtb._animParam);
                if (checkArg3.checkResult)
                {
                    AtkAttribute._selfAnimator.SetTrigger(skillAtb._animParam);
                }
            }

            return true;
        }

        /// <summary>
        /// 计算伤害值，一般用于普攻
        /// </summary>
        /// <param name="critical"></param>
        /// <param name="arg"></param>
        private void CalculateDamage(AttackTargetArg arg,Action endAct = null)
        {
            var atkChar = arg.mCurAtkArg;
            var defChars = arg.mCurDefArgs;
            float atkValue = 0, defValue = 0;
            //暴击伤害的增值系数
            float dmgExp = arg.mCurAtkCritical
                ? AtkAttribute._curWeapon.Additional_Critical_Damage + AtkAttribute._curCharacter.Critical_Damage
                : 1;
            //对所有受到攻击的目标计算伤害
            foreach (var defChar in defChars)
            {
                atkValue = defValue = 0;
                switch (arg.mAtkDamageType)
                {
                    case AtkDamageType.Physic:
                        atkValue = atkChar.mAttack_Physic;
                        atkValue *= dmgExp;
                        //先对护盾进行消减
                        if (atkValue > defChar.mShiled)
                        {
                            atkValue -= defChar.mShiled;
                            defChar.mShiled = 0;
                        }
                        else
                        {
                            atkValue = 0;
                            defChar.mShiled -= atkValue;
                        }
                        defValue = defChar.mDefence_Physic + defChar.mShiled;
                        break;
                    case AtkDamageType.Magic:
                        atkValue = atkChar.mAttack_Magic;
                        atkValue *= dmgExp;
                        if (atkValue > defChar.mShiled)
                        {
                            atkValue -= defChar.mShiled;
                            defChar.mShiled = 0;
                        }
                        else
                        {
                            atkValue = 0;
                            defChar.mShiled -= atkValue;
                        }
                        defValue = defChar.mDefence_Magic + defChar.mShiled;
                        break;
                    case AtkDamageType.Both:
                        atkValue = atkChar.mAttack_Magic + atkChar.mAttack_Physic;
                        atkValue *= dmgExp;
                        if (atkValue > defChar.mShiled)
                        {
                            atkValue -= defChar.mShiled;
                            defChar.mShiled = 0;
                        }
                        else
                        {
                            atkValue = 0;
                            defChar.mShiled -= atkValue;
                        }
                        defValue = defChar.mDefence_Magic + defChar.mDefence_Physic + defChar.mShiled;
                        break;
                }
                var damageValue = atkValue - defValue > 0 ? atkValue - defValue : 0;
                defChar.mHealth -= damageValue;
            }
            arg.UpdateHealth();
            endAct?.Invoke();
        }

        private void CalculateDamage(AtkDamageArg atker, AtkDamageArg defer,SampleDmgArg atk,Action endAct = null)
        {
            float atkValue = 0, defValue = 0;
            //暴击伤害的增值系数
            float dmgExp = atk.critical
                ? atk.criticalDmg
                : 1;
            switch (atk.dmgType)
            {
                case AtkDamageType.Physic:
                    atkValue = atker.mAttack_Physic;
                    atkValue *= dmgExp;
                    //先对护盾进行消减
                    if (atkValue > defer.mShiled)
                    {
                        atkValue -= defer.mShiled;
                        defer.mShiled = 0;
                    }
                    else
                    {
                        atkValue = 0;
                        defer.mShiled -= atkValue;
                    }
                    defValue = defer.mDefence_Physic + defer.mShiled;
                    break;
                case AtkDamageType.Magic:
                    atkValue = atker.mAttack_Magic;
                    atkValue *= dmgExp;
                    if (atkValue > defer.mShiled)
                    {
                        atkValue -= defer.mShiled;
                    }
                    else
                    {
                        atkValue = 0;
                        defer.mShiled -= atkValue;
                    }
                    defValue = defer.mDefence_Magic + defer.mShiled;
                    break;
                case AtkDamageType.Both:
                    atkValue = atker.mAttack_Magic + atker.mAttack_Physic;
                    atkValue *= dmgExp;
                    if (atkValue > defer.mShiled)
                    {
                        atkValue -= defer.mShiled;
                        defer.mShiled = 0;
                    }
                    else
                    {
                        atkValue = 0;
                        defer.mShiled -= atkValue;
                    }
                    defValue = defer.mDefence_Magic + defer.mDefence_Physic + defer.mShiled;
                    break;
            }
            var damageValue = atkValue - defValue > 0 ? atkValue - defValue : 0;
            defer.mHealth -= damageValue;
            //执行回调函数
            endAct?.Invoke();
        }
        
        /// <summary>
        /// <para>计算伤害值</para>
        /// <para>不涉及暴击</para>
        /// </summary>
        /// <param name="atker">攻击者</param>
        /// <param name="defer">防御者</param>
        /// <param name="endAct">回调</param>
        private void CalculateDamage(AtkDamageArg atker, AtkDamageArg defer,AtkDamageType dmgType)
        {
            float atkValue = 0, defValue = 0;
            switch (dmgType)
            {
                case AtkDamageType.Physic:
                    atkValue = atker.mAttack_Physic;
                    //先对护盾进行消减
                    if (atkValue > defer.mShiled)
                    {
                        atkValue -= defer.mShiled;
                        defer.mShiled = 0;
                    }
                    else
                    {
                        atkValue = 0;
                        defer.mShiled -= atkValue;
                    }
                    defValue = defer.mDefence_Physic + defer.mShiled;
                    break;
                case AtkDamageType.Magic:
                    atkValue = atker.mAttack_Magic;
                    if (atkValue > defer.mShiled)
                    {
                        atkValue -= defer.mShiled;
                    }
                    else
                    {
                        atkValue = 0;
                        defer.mShiled -= atkValue;
                    }
                    defValue = defer.mDefence_Magic + defer.mShiled;
                    break;
                case AtkDamageType.Both:
                    atkValue = atker.mAttack_Magic + atker.mAttack_Physic;
                    if (atkValue > defer.mShiled)
                    {
                        atkValue -= defer.mShiled;
                        defer.mShiled = 0;
                    }
                    else
                    {
                        atkValue = 0;
                        defer.mShiled -= atkValue;
                    }
                    defValue = defer.mDefence_Magic + defer.mDefence_Physic + defer.mShiled;
                    break;
            }
            var damageValue = atkValue - defValue > 0 ? atkValue - defValue : 0;
            defer.mHealth -= damageValue;
            
            //更新数据
            CurHealth = defer.mHealth;
            CurShiled = defer.mShiled;
        }

        /// <summary>
        /// 计算自身受到的伤害
        /// </summary>
        /// <param name="atk"></param>
        /// <param name="defs"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void GetDamage(SampleDmgArg atk, List<int> defs)
        {
            //判断自己是否受到伤害
            if (!defs.Contains(id))
            {
                return;
            }

            var atkBaseAtkCtrl = LZLAtkCenterManager.Instance.GetBaseAttackVtrlById(atk.atkerId);
            var atker = BornAtkDamageArgByLZLBaseAttackCtrl(atkBaseAtkCtrl);
            var defer = BornAtkDamageArgByLZLBaseAttackCtrl(this);
            
            //攻击停顿
            atkBaseAtkCtrl.HitPause();
            //镜头晃动
            atkBaseAtkCtrl.CameraShake();
            //计算伤害
            CalculateDamage(atker,defer, atk,() =>
            {
                //更新生命值
                CurHealth = defer.mHealth;
                //更新护盾值
                CurShiled = defer.mShiled;
            });
        }

        /// <summary>
        /// 计算自身受到的伤害
        /// </summary>
        /// <param name="atk"></param>
        public void GetDamage(SampleDmgArg atk)
        {
            GetDamage(atk,new List<int>(){this.id});
        }

        /// <summary>
        /// 更新下一次的攻击样式ID
        /// </summary>
        private void UpNolAtkID()
        {
            if (curNolAtkId < AtkAttribute._nolAttackCycles)
            {
                curNolAtkId += 1;
            }
            //置普攻样式为0
            if (curNolAtkId >= AtkAttribute._nolAttackCycles)
            {
                curNolAtkId = 0;
            }
        }

        /// <summary>
        /// 初始化curWeapon字段
        /// </summary>
        private void InitCurWeapon()
        {
            if (curWeapon==null)
            {
                curWeapon = Instantiate(AtkAttribute._curWeapon);
            }
        }

        #region AnimUseFuinction

        /// <summary>
        /// 动画里的事件调用
        /// </summary>
        public void CalculateDamageAnim()
        {
            if (CurAtkData == null)
            {
                Debug.LogError("CurAtkData未赋值，请在攻击时赋值！");
            }

            if (CurAtkData.mCurTargets.Count == 0)
            {
                Debug.LogError("攻击目标为0，设置后才能计算伤害。");
            }
            //自身攻击属性赋值
            CurAtkData.InitAtkDmgArg(ref CurAtkData.mCurAtkArg,this);
            //目标攻击属性赋值
            foreach (var item in CurAtkData.mCurTargets)
            {
                AtkDamageArg damageArg = new AtkDamageArg();
                CurAtkData.InitAtkDmgArg(ref damageArg,item);
                CurAtkData.mCurDefArgs.Add(damageArg);
            }
            //伤害计算
            CalculateDamage(CurAtkData, () => CurAtkData = null);
        }

        /// <summary>
        /// 近战动画调用，给予伤害。需要预先设置好CurAtkData的目标
        /// </summary>
        public void GiveDmg_CloseCombat_Anim()
        {
            if (CurAtkData == null)
            {
                Debug.LogError("CurAtkData未赋值，请在攻击时赋值！");
            }

            if (CurAtkData.mCurTargets.Count == 0)
            {
                Debug.LogError("攻击目标为0，设置后才能计算伤害。");
            }
            //自身攻击属性赋值
            SampleDmgArg atk = new SampleDmgArg()
            {
                atkerId = id,
                critical = CurAtkData.mCurAtkCritical,
                criticalDmg = AtkAttribute._curWeapon.Additional_Critical_Damage +
                              AtkAttribute._curCharacter.Critical_Damage,
            };
            //目标集获取
            List<int> defs = new List<int>();
            foreach (var item in CurAtkData.mCurTargets)
            {
                defs.Add(item.id);
            }
            //触发伤害调用管道
            LZLAtkCenterManager.Invoke_GiveDamagePipeLine(atk,defs);
            
            //CurAtkData置空
            CurAtkData = null;
        }

        /// <summary>
        /// 设置攻击状态，所有攻击动画都要设置
        /// </summary>
        /// <param name="attacking"></param>
        public void SetAttackState_Anim(bool attacking)
        {
            this._attacking = attacking;
        }

        /// <summary>
        /// 显示技能特效，动画触发事件
        /// </summary>
        public void ShowSkillParticle_Anim()
        {
            var skillObj = GetSkillObjBySkillId_CanCreate(CurAtkData.mSkillIndex);
            skillObj.GetComponent<LZLBaseSkillGOBJ>().SkillParticle_ActiveCtrl(true);
            skillObj.GetComponent<LZLBaseSkillGOBJ>().StartSkillParticle(this);
        }

        /// <summary>
        /// 开启武器检测器
        /// </summary>
        public void OpenWeaponCheck_Anim()
        {
            var checkMachine = AtkAttribute._weaponCheckMachine.gameObject;
            checkMachine.SendMessage("StartTargetCheck",this);
        }
        /// <summary>
        /// 关闭武器检测器
        /// </summary>
        public void OffWeponCheck_Anim()
        {
            var checkMachine = AtkAttribute._weaponCheckMachine.gameObject;
            checkMachine.SendMessage("EndTargetCheck");
            
            //结束攻击状态
            _attacking = false;
        }

        #endregion
        
        #region 辅助性函数
        /// <summary>
        /// 检查动画状态机是否包含对应的参数名
        /// </summary>
        /// <param name="animator"></param>
        /// <param name="paraName"></param>
        /// <returns></returns>
        protected LZLCheckArg CheckAnimatorParaContain(Animator animator,string paraName)
        {
            LZLCheckArg result = new LZLCheckArg();
            foreach (var item in animator.parameters)
            {
                if (item.name==paraName)
                {
                    result.checkResult = true;
                    result.errorMsg = "";
                    return result;
                }
            }

            result.checkResult = false;
            result.errorMsg = "错误信息:参数不正确：" + animator.name + "的Animator动画参数不正确，错误参数：" +
                              paraName;
            return result;
        }

        /// <summary>
        /// 判断目标于自身的位置关系
        /// </summary>
        /// <param name="self">自身的transform</param>
        /// <param name="target">目标的transform</param>
        /// <param name="angle">角度，[0,180]，自身到目标向量同自身z轴方向的夹角</param>
        /// <returns></returns>
        protected bool CheckTargetPosForSelf(Transform self, Transform target, float angle)
        {
            if (angle<0)
            {
                Debug.LogError("阈值报错：angle: " + angle + "小于0，但它不能小于0");
                return false;
            }
            angle = angle > 360 ? angle % 360 : angle;
            if (angle > 180)
            {
                angle = 360 - angle;
            }
            var tranToTarget = target.position - self.position;
            tranToTarget.Normalize();
            var cosValue = Vector3.Dot(transform.forward, tranToTarget);
            var cosAngle = Mathf.Acos(cosValue);
            return cosAngle < angle;
        }

        /// <summary>
        /// 检测攻击是否暴击
        /// </summary>
        /// <returns></returns>
        private bool CheckCritical()
        {
            //计算暴击相关参数,人物基础的加上武器附带的
            float criticalRate = AtkAttribute._curCharacter.Critical_Hit_Rate +
                                 AtkAttribute._curWeapon.Additional_Critical_Hit_Rate;
            criticalRate = Mathf.Clamp01(criticalRate);
            var randomValue = Random.Range(0f, 1f);
            return randomValue < criticalRate;
        }
        /// <summary>
        /// 根据LZLBaseCtrl来生成对应的AtkDamageArg
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static AtkDamageArg BornAtkDamageArgByLZLBaseAttackCtrl(LZLBaseAttackCtrl src)
        {
            var atkChar = src.AtkAttribute._curCharacter;
            var atkWeapon = src.AtkAttribute._curWeapon;
            AtkDamageArg result = new AtkDamageArg()
            {
                mHealth = src.CurHealth,
                mAttack_Physic = atkChar.Physic_Attack + atkWeapon.Weapon_Attack,
                mDefence_Magic = atkChar.Magic_Denfence + atkWeapon.Magic_Defence,
                mShiled =atkChar.Shiled,
                mAttack_Magic = atkChar.Magic_Attack + atkWeapon.Weapon_Attack,
                mDefence_Physic = atkChar.Physic_Denfence + atkWeapon.Physic_Defence
            };
            return result;
        }

        /// <summary>
        /// 使用技能获得对应的技能游戏体。
        /// 1：检测是否在字典中记录过，有记录就直接获取，没记录就创建新的游戏体。
        /// 2：添加LZLSkillGOBJ给技能游戏体
        /// 3：该函数未检测技能号
        /// 4:只给技能号则不涉及创建，只是获取，会返回空值
        /// </summary>
        /// <param name="skillIndex">技能号</param>
        /// <param name="skillAtb">技能信息</param>
        /// <param name="skillContainer">角色技能容器</param>
        /// <returns>技能游戏体</returns>
        public GameObject GetSkillObjBySkillId_CanCreate(int skillIndex,LZLSkillAtb skillAtb = null,Transform skillContainer = null)
        {
            GameObject result;
            //没生成过，生成技能特效游戏体并记录。
            if (!skillObjMap.ContainsKey(skillIndex))
            {
                if (skillAtb == null || skillContainer == null)
                {
                    return null;
                }
                result = Instantiate(skillAtb._skillPrefab, skillContainer);
                skillObjMap.Add(skillIndex,result);
            }
            //生成过，获取对应游戏体。
            else
            {
                result = skillObjMap[skillIndex];
            }
            
            //判断是否带有对应脚本，没有则添加
            if (result.GetComponent<LZLBaseSkillGOBJ>() == null)
            {
                result.AddComponent<LZLBaseSkillGOBJ>();
            }
            
            return result;
        }

        /// <summary>
        /// 清空当前存储的攻击数据
        /// </summary>
        public void ClearCurAtkData()
        {
            CurAtkData = null;
        }

        /// <summary>
        /// 获得当前攻击状态
        /// </summary>
        /// <returns></returns>
        public bool GetAttackState()
        {
            return _attacking;
        }

        /// <summary>
        /// 攻击停顿
        /// </summary>
        /// <param name="pauseTime"></param>
        public void HitPause()
        {
            StartCoroutine(hitPause(AtkAttribute._pauseTime));
        }

        IEnumerator hitPause(float pauseTime)
        {
            pauseTime = pauseTime / 60;
            Time.timeScale = 0;
            yield return new WaitForSecondsRealtime(pauseTime);
            Time.timeScale = 1;
        }

        /// <summary>
        /// 镜头晃动
        /// </summary>
        public void CameraShake()
        {
            StartCoroutine(cameraShake(AtkAttribute._shakeDuration, AtkAttribute._shakeStrength));
        }

        IEnumerator cameraShake(float duration,float strength)
        {
            var cam = Camera.main.transform;
            var oriPosition = cam.position;
            while (duration>0)
            {
                cam.position = Random.insideUnitSphere * strength + cam.position;
                duration -= Time.deltaTime;
                yield return null;
            }

            cam.position = oriPosition;
        }

        #endregion

        #region 注册函数
        
        public void SubscribeEvent()
        {
            //订阅受到伤害事件
            LZLAtkCenterManager.GiveDamagePipeLine += GetDamage;
        }
        
        public void UnSubscribeEvent()
        {
            LZLAtkCenterManager.GiveDamagePipeLine -= GetDamage;
        }
        
        #endregion

        #region 外部使用函数
        [ContextMenu("输出当前状态")]
        public void ShowCurState()
        {
            string msg = transform.name + "当前状态\n";
            msg += "生命值：" + CurHealth + "\n";
            msg += "护盾值：" + CurShiled + "\n";
            msg += "法力值：" + CurMana + "\n";
            Debug.Log(msg);
        }

        #endregion

    }

    public enum AtkType
    {
        normalAtk,smiteAtk,SkillAtk
    }

    public enum AtkDamageType
    {
        Physic,Magic,Both
    }

    public enum AtkState
    {
        /// <summary>
        /// 未进行任何攻击
        /// </summary>
        Empty,
        /// <summary>
        /// 正在执行一次攻击
        /// </summary>
        Atking,
    }

    /// <summary>
    /// 调用攻击函数时用到的类型结构
    /// </summary>
    public class AttackTargetArg
    {
        /// <summary>
        /// 攻击类型
        /// </summary>
        public AtkType mAtkType;
        /// <summary>
        /// 攻击伤害类型
        /// </summary>  
        public AtkDamageType mAtkDamageType;
        /// <summary>
        /// 攻击是否暴击
        /// </summary>
        public bool mCurAtkCritical;
        /// <summary>
        /// 实时目标集
        /// </summary>
        public List<LZLBaseAttackCtrl> mCurTargets;
        /// <summary>
        /// 攻击方的属性
        /// </summary>
        public AtkDamageArg mCurAtkArg;
        /// <summary>
        /// 防守方的属性
        /// </summary>
        public List<AtkDamageArg> mCurDefArgs;
        /// <summary>
        /// 技能号
        /// </summary>
        public int mSkillIndex;
        public AttackTargetArg()
        {
            mAtkType = AtkType.normalAtk;
            mAtkDamageType = AtkDamageType.Physic;
            mCurAtkCritical = false;
            mCurTargets = new List<LZLBaseAttackCtrl>();
            mCurDefArgs = new List<AtkDamageArg>();
            mSkillIndex = 0;
        }

        public AttackTargetArg(AtkType type, AtkDamageType dmgType, List<LZLBaseAttackCtrl> targets)
        {
            //属性初始化
            mAtkType = type;
            mAtkDamageType = dmgType;
            mCurTargets = targets.ToList<LZLBaseAttackCtrl>();
            mCurDefArgs = new List<AtkDamageArg>();
        }
        public AttackTargetArg(AtkType type, AtkDamageType dmgType)
        {
            //属性初始化
            mAtkType = type;
            mAtkDamageType = dmgType;
            mCurTargets = new List<LZLBaseAttackCtrl>();
            mCurDefArgs = new List<AtkDamageArg>();
        }
        /// <summary>
        /// 初始化攻击数据
        /// </summary>
        /// <param name="arg"></param>
        /// <param name="value"></param>
        public void InitAtkDmgArg(ref AtkDamageArg arg,LZLBaseAttackCtrl value)
        {
            var atkChar = value.AtkAttribute._curCharacter;
            var atkWeapon = value.AtkAttribute._curWeapon;
            //自身属性赋值
            arg = new AtkDamageArg()
            {
                mHealth = value.CurHealth,
                mAttack_Physic = atkChar.Physic_Attack + atkWeapon.Weapon_Attack,
                mDefence_Magic = atkChar.Magic_Denfence + atkWeapon.Magic_Defence,
                mShiled =atkChar.Shiled,
                mAttack_Magic = atkChar.Magic_Attack + atkWeapon.Weapon_Attack,
                mDefence_Physic = atkChar.Physic_Denfence + atkWeapon.Physic_Defence
            };
        }

        public void UpdateHealth()
        {
            int i = 0;
            foreach (var item in mCurDefArgs)
            {
                mCurTargets[i]._curHealth = item.mHealth;
                i++;
            }
        }
    }

    /// <summary>
    /// 攻击伤害计算的通用类型
    /// </summary>
    public class AtkDamageArg : ISHOWCLASS
    {
        public float mHealth;
        public float mDefence_Physic;
        public float mDefence_Magic;
        public float mAttack_Physic;
        public float mAttack_Magic;
        public float mShiled;
        public void ShowSelf()
        {
            Debug.Log("AtkDamageArg ShowSelf");
            Debug.Log("mHealth: " + mHealth);
            Debug.Log("mDefence_Physic: " + mDefence_Physic);
            Debug.Log("mDefence_Magic: " + mDefence_Magic);
            Debug.Log("mAttack_Physic: " + mAttack_Physic);
            Debug.Log("mAttack_Magic: " + mAttack_Magic);
            Debug.Log("mShiled: " + mShiled);
        }
    }

    /// <summary>
    /// 伤害事件通知需要参数
    /// </summary>
    public struct SampleDmgArg
    {
        public int atkerId;
        public AtkDamageType dmgType;
        public bool critical;
        public float criticalDmg;
    }

}

