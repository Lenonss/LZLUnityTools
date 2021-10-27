using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LZLToolBox.PlayerController
{
    [RequireComponent(typeof(Animator))]
    public class LZLAttackNormalAttribute : MonoBehaviour
    {
        [Tooltip("角色的动画状态机")]
        public Animator _selfAnimator;

        [Header("普通攻击相关")]
        [Tooltip("普通攻击是否可以被中断")]
        public bool _norAtkCanBreakOff;
        [Tooltip("普攻以几种样式循环"),Range(0,30)]
        public int _nolAttackCycles;
        [Tooltip("普攻段数对应的动画播放器参数")]
        public List<string> _nolAtkAnimParameters = new List<string>();
        [Tooltip("普攻是否有特效")]
        public bool _useNolPatticle;
        [Tooltip("特效预制体数组")]
        public List<GameObject> _nolAtkPars = new List<GameObject>();
        
        [Header("重击攻击相关")]
        [Tooltip("重击攻击是否可以被中断")]
        public bool _hitAtkCanBreakOff;
        [Tooltip("重击以几种样式循环"),Range(0,30)]
        public int _hitAttackCycles;
        [Tooltip("重击段数对应的动画播放器参数")]
        public List<string> _hitAtkAnimParameters = new List<string>();
        [Tooltip("重击是否有特效")]
        public bool _useHitPatticle;
        [Tooltip("特效预制体数组")]
        public List<GameObject> _hitAtkPars = new List<GameObject>();

        [Header("技能相关")] 
        [Tooltip("技能个数"),Range(0,100)]
        public int _skillNum;
        [Tooltip("技能控制的动画参数")]
        public List<string> _skillAnimParam;
        [Tooltip("技能是否可以中途中断")]
        public bool _skillAtkCanBreakOff;

        [Header("武器相关")] 
        [Tooltip("相应武器的SO")]
        public WeaponState_SO _curWeapon;
        
        [Header("人物相关1")] 
        [Tooltip("相应人物的SO")]
        public CharacterState_SO _curCharacter;


        private void Awake()
        {
            CanRunCheck();
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
    }
    
}

