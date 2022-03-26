using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LZLToolBox.PlayerController
{
    public enum CharStateType 
    { 
        Player,Enemy, 
    } 
    [CreateAssetMenu(menuName = "LZLToolBox/PlayerController/CharacterState_SO", fileName = "NewCharacterState_SO")] 
    public class CharacterState_SO : ScriptableObject 
    {
        [Tooltip("当前ScriptObject代表的类型")] 
        public CharStateType CharacterType;
        [Tooltip("基础生命值")]
        public float Basic_Health;// 基础生命值
        [Tooltip("当前生命值")]
        public float Current_Health;//当前生命值
        [Tooltip("当前的法力值")]
        public float Mana;//当前的法力值
        [Tooltip("护盾值，伤害抵消比 1点护盾值抵消一点的伤害值")]
        public float Shiled;//护盾值，伤害抵消比 1点护盾值抵消一点的伤害值
        [Tooltip("物理防御值")]
        public float Physic_Denfence;// 物理防御值
        [Tooltip("魔法防御值")]
        public float Magic_Denfence;//魔法防御值

        [Tooltip("物理攻击力")]
        public float Physic_Attack;//物理攻击力
        [Tooltip("魔法攻击力")]
        public float Magic_Attack; //魔法攻击力
        [Tooltip("暴击伤害")]
        public float Critical_Damage;//暴击伤害
        [Tooltip("暴击率")]
        public float Critical_Hit_Rate;// 暴击率
        public static CharacterState_SO operator +(CharacterState_SO character, WeaponState_SO weapon) 
        {
            CharacterState_SO state_SO = new CharacterState_SO();
            state_SO.Basic_Health = character.Basic_Health + weapon.Additional_Health;
            state_SO.Current_Health = character.Current_Health + weapon.Additional_Health;
            state_SO.Shiled = character.Shiled;
            state_SO.Physic_Denfence = character.Physic_Denfence + weapon.Physic_Defence;
            state_SO.Magic_Denfence = character.Magic_Denfence + weapon.Magic_Defence;
            state_SO.Mana = character.Mana + weapon.Additional_Mana;
            switch (weapon.type) 
            {
                case WeaponType.Physic:
                    state_SO.Physic_Attack = character.Physic_Attack + weapon.Weapon_Attack;
                    state_SO.Magic_Attack = character.Magic_Attack;
                    break;
                case WeaponType.Magic:
                    state_SO.Magic_Attack = character.Magic_Attack + weapon.Weapon_Attack;
                    state_SO.Physic_Attack = character.Physic_Attack;
                    break;
                case WeaponType.None:
                    state_SO.Magic_Attack = character.Magic_Attack;
                    state_SO.Physic_Attack = character.Physic_Attack;
                    break;
            }
            state_SO.Critical_Damage = character.Critical_Damage + weapon.Additional_Critical_Damage;
            state_SO.Critical_Hit_Rate = character.Critical_Hit_Rate + weapon.Additional_Critical_Hit_Rate;

            return state_SO;
        }
    }
}

