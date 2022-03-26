using System;
using System.Collections;
using System.Collections.Generic;
using LZLToolBox.CommonPlugin.Tag;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace LZLToolBox.PlayerController
{
    public class LZLTestAtkCtrl : LZLBaseAttackCtrl
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                StartNolAtk();
            }

            if (Input.GetKeyDown(KeyCode.B))
            {
                AttackTargetArg arg = new AttackTargetArg();
                arg.mAtkType = AtkType.SkillAtk;
                arg.mSkillIndex = 0;
                CurAtkData = arg;
                //检测魔法值，冷却时间等因素，判断是否播放攻击动画
                
                //播放攻击动画
                //攻击动画调用函数释放特效
                //特效游戏体控制伤害计算
                var result = AttackTarget(skillIndex: 0);
                Debug.Log("技能攻击结果：" + result);
                if (!result)
                {
                    //清空数据
                    ClearCurAtkData();
                }
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                StartNolAtk_LineCheck();
            }
        }

        private void StartNolAtk_LineCheck()
        {
            //攻击对象在武器检测上获取
            //伤害计算在动画状态机各个动画挂载的脚本中调用进行

            if (!AtkAttribute._hasWeapon)
            {
                return;
            }
            
            //生成攻击需要的参数
            AttackTargetArg arg =
                new AttackTargetArg(type: AtkType.normalAtk, AtkDamageType.Physic); 
            //赋值CurAtkData
            CurAtkData = arg;
            //调用攻击动画
            Debug.Log("攻击结果：" + AttackTarget());
        }

        private void StartNolAtk()
        {
            //找到攻击对象
            var targets = FindAtkTargetByOverlapSphere(transform.position, AtkAttribute._curWeapon.Attack_Range,
                UnityEnumTag.enemy);
            Debug.Log("FindTarget? " + targets.Count);
            //不可空A
            if (targets.Count==0||targets==null)
            {
                return;
            }
            //筛选出可攻击到方向中的所有目标
            List<LZLBaseAttackCtrl> InAtkVectorTargets = new List<LZLBaseAttackCtrl>();
            foreach (var item in targets)
            {
                if (CheckTargetPosForSelf(transform,item.transform,30))
                {
                    InAtkVectorTargets.Add(item);
                }
            }
            //攻击目标筛选
            foreach (var item in InAtkVectorTargets)
            {
                //只对Enemy标签的敌人进行伤害计算
                if (item.AtkAttribute._curCharacter.CharacterType != CharStateType.Enemy)
                {
                    InAtkVectorTargets.Remove(item);
                    continue;
                }
            }
            //生成攻击需要的参数
            AttackTargetArg arg =
                new AttackTargetArg(type: AtkType.normalAtk, AtkDamageType.Physic, InAtkVectorTargets);
            //如果还有攻击目标
            if (InAtkVectorTargets.Count>0)
            {
                //赋值CurAtkData
                CurAtkData = arg;
                //调用攻击动画
                AttackTarget();
            }
        }
    }
}

