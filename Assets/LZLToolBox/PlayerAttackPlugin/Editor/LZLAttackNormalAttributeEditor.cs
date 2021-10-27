using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LZLToolBox.PlayerController
{
    [CustomEditor(typeof(LZLAttackNormalAttribute))]
    public class LZLAttackNormalAttributeEditor : Editor
    {
        private SerializedObject obj; //序列化

        private SerializedProperty selfAnimator;

        private SerializedProperty norAtkCanBreakOff,nolAttackCycle,
            nolAtkAnimParameters,useNolPatticle,nolAtkPars;
        private SerializedProperty hitAtkCanBreakOff,hitAttackCycle,
            hitAtkAnimParameters,useHitPatticle,hitAtkPars;

        private SerializedProperty skillNum, skillAnimParam, skillAtkCanBreakOff;

        private SerializedProperty curWeapon,curCharacter;
        

        void OnEnable()
        {
            obj = new SerializedObject(target);
            //获得属性字段
            selfAnimator = obj.FindProperty("_selfAnimator");

            norAtkCanBreakOff = obj.FindProperty("_norAtkCanBreakOff");
            nolAttackCycle = obj.FindProperty("_nolAttackCycles");
            nolAtkAnimParameters = obj.FindProperty("_nolAtkAnimParameters");
            useNolPatticle = obj.FindProperty("_useNolPatticle");
            nolAtkPars = obj.FindProperty("_nolAtkPars");
            
            //获得重击相关字段
            GetHitAtkProperty();
            
            //获得技能相关字段
            GetSkillProperty();
            
            //获得武器信息相关字段
            curWeapon = obj.FindProperty("_curWeapon");
            //获得人物信息相关字段
            curCharacter = obj.FindProperty("_curCharacter");
        }

        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();
            obj.Update();
            //******Middle******
            EditorGUILayout.PropertyField(selfAnimator);
            
            //普攻相关参数
            EditorGUILayout.PropertyField(norAtkCanBreakOff);
            EditorGUILayout.PropertyField(nolAttackCycle);
            nolAtkAnimParameters.arraySize = nolAttackCycle.intValue >= 0 ? nolAttackCycle.intValue : 0;
            EditorGUILayout.PropertyField(nolAtkAnimParameters);
            EditorGUILayout.PropertyField(useNolPatticle);
            if (useNolPatticle.boolValue)
            {
                nolAtkPars.arraySize = nolAtkAnimParameters.arraySize;
                EditorGUILayout.PropertyField(nolAtkPars);
            }
            //重击相关参数显示
            ShowHitAtkProperty();
            //技能相关参数显示
            ShowSkillProperty();
            //武器信息显示
            EditorGUILayout.PropertyField(curWeapon);
            //人物信息显示
            EditorGUILayout.PropertyField(curCharacter);
            //******Middle******
            obj.ApplyModifiedProperties();
        }

        /// <summary>
        /// 获取重击相关字段
        /// </summary>
        private void GetHitAtkProperty()
        {
            hitAtkCanBreakOff = obj.FindProperty("_hitAtkCanBreakOff");
            hitAttackCycle = obj.FindProperty("_hitAttackCycles");
            hitAtkAnimParameters = obj.FindProperty("_hitAtkAnimParameters");
            useHitPatticle = obj.FindProperty("_useHitPatticle");
            hitAtkPars = obj.FindProperty("_hitAtkPars");
        }
        /// <summary>
        /// 获取技能相关字段
        /// </summary>
        private void GetSkillProperty()
        {
            skillNum = obj.FindProperty("_skillNum");
            skillAnimParam = obj.FindProperty("_skillAnimParam");
            skillAtkCanBreakOff = obj.FindProperty("_skillAtkCanBreakOff");
        }

        /// <summary>
        /// 显示重击相关字段到属性面板
        /// </summary>
        private void ShowHitAtkProperty()
        {
            EditorGUILayout.PropertyField(hitAtkCanBreakOff);
            EditorGUILayout.PropertyField(hitAttackCycle);
            hitAtkAnimParameters.arraySize = hitAttackCycle.intValue >= 0 ? hitAttackCycle.intValue : 0;
            EditorGUILayout.PropertyField(hitAtkAnimParameters);
            EditorGUILayout.PropertyField(useHitPatticle);
            if (useHitPatticle.boolValue)
            {
                hitAtkPars.arraySize = hitAtkAnimParameters.arraySize;
                EditorGUILayout.PropertyField(hitAtkPars);
            }
        }
        
        /// <summary>
        /// 显示重击相关字段到属性面板
        /// </summary>
        private void ShowSkillProperty()
        {
            EditorGUILayout.PropertyField(skillNum);
            if (skillNum.intValue > 0)
            {
                skillAnimParam.arraySize = skillNum.intValue;
                EditorGUILayout.PropertyField(skillAnimParam);
                EditorGUILayout.PropertyField(skillAtkCanBreakOff);
            }
        }
    }
}