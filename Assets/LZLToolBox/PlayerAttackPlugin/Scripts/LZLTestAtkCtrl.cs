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
                //找到攻击对象
                var targets = FindAtkTargetByOverlapSphere(transform.position, AtkAttribute._curWeapon.Attack_Range,
                    UnityEnumTag.enemy);
                //调用攻击函数进行普通攻击
                Debug.Log("攻击结果：" + AttackTarget(AtkType.normalAtk));
            }
        }
    }
}

