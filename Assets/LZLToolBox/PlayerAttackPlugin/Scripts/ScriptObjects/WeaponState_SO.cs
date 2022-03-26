using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LZLToolBox.PlayerController
{
    public enum WeaponType
    {
        None, Physic, Magic,
    }
    [CreateAssetMenu(menuName = "LZLToolBox/PlayerController/WeaponState_SO", fileName = "New WeaponState_SO")]
    public class WeaponState_SO : ScriptableObject
    {

        public int Weapon_Id;// 武器Id
        public WeaponType type;// 枚举类型，声明武器类型
        public float Weapon_Attack;// 武器对角色的加值攻击力
        public int Attack_Range;//攻击距离
        public float Attack_Cool_Time;
        public float Physic_Defence;// 物理防御加值
        public float Magic_Defence;// 魔法防御加值
        public float Additional_Health;// 生命值加值
        public float Additional_Mana;//法力值加值
        public float Additional_Critical_Damage;// 暴击伤害加值
        public float Additional_Critical_Hit_Rate;// 暴击率加值

        /// <summary>
        /// <para>获得对应攻击力</para>
        /// <para>参数为真时得到物理攻击力，参数为负时得到魔法攻击力</para>
        /// </summary>
        /// <param name="isPhysic">参数</param>
        /// <returns></returns>
        public float GetTypeAtkValue(bool isPhysic)
        {
            switch (type)
            {
                case WeaponType.None:
                    return 0;
                case WeaponType.Magic:
                    return isPhysic ? 0 : Weapon_Attack;
                case WeaponType.Physic:
                    return isPhysic ? Weapon_Attack : 0;
                default:
                    return 0;
            }
        }
    }
}

