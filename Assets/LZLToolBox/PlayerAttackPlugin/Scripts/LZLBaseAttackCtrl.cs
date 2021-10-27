using System.Collections;
using System.Collections.Generic;
using LZLToolBox.CommonPlugin.Tag;
using LZLToolBox.PlayerController;
using UnityEngine;

namespace LZLToolBox.PlayerController
{
    [RequireComponent(typeof(LZLAttackNormalAttribute))]
    public class LZLBaseAttackCtrl : MonoBehaviour
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
                    _atkAttribute.GetComponent<LZLAttackNormalAttribute>();
                }

                return _atkAttribute;
            }
        }

        /// <summary>
        /// 检测球形范围内的碰撞器来找到攻击对象
        /// </summary>
        /// <param name="center"></param>
        /// <param name="radius"></param>
        /// <param name="tag"></param>
        public List<Collider> FindAtkTargetByOverlapSphere(Vector3 center,float radius,UnityEnumTag tag)
        {
            var targets = Physics.OverlapSphere(center, radius);
            List<Collider> result = new List<Collider>();
            if (targets == null || targets.Length == 0)
            {
                return null;
            }

            foreach (var item in targets)
            {
                if (item.tag.ToString() != tag.ToString())
                {
                    result.Add(item);
                }
            }

            return result;
        }

        #region 运行中用到的参数

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
        #endregion
        
        /// <summary>
        /// 攻击
        /// </summary>
        /// <param name="atkType"></param>
        /// <returns></returns>
        public bool AttackTarget(AtkType atkType)
        {
            switch (atkType)
            {
                case AtkType.normalAtk:
                    return nolAtk();
                case  AtkType.smiteAtk:
                    return false;
                case AtkType.SkillAtk:
                    return false;
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
            //更新最新攻击时间
            lastAtkTime = Time.unscaledTime;
            //播放攻击动画,采用触发器方式
            _atkAttribute._selfAnimator.SetTrigger(_atkAttribute._nolAtkAnimParameters[curNolAtkId]);
            UpNolAtkID();
            curNolAtkRank += 1;
            return true;
        }

        /// <summary>
        /// 更新下一次的攻击样式ID
        /// </summary>
        private void UpNolAtkID()
        {
            if (curNolAtkId < _atkAttribute._nolAttackCycles)
            {
                curNolAtkId += 1;
            }
            //置普攻样式为0
            if (curNolAtkId >= _atkAttribute._nolAttackCycles)
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
                curWeapon = Instantiate(_atkAttribute._curWeapon);
            }
        }
    }

    public enum AtkType
    {
        normalAtk,smiteAtk,SkillAtk
    }
}

