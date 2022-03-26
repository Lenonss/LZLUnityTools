using System;
using UnityEditor;
using UnityEngine;
using UE_EditorGUILayout = UnityEditor.EditorGUILayout;

namespace LZLToolBox.PlayerController
{
    [CustomEditor(typeof(LZLColliderDmgUnit_Skill))]
    public class LZLColliderDmgUnit_Skill_Editor : Editor
    {
        private SerializedObject obj; //序列化
        
        private SerializedProperty damageType,magicRatio,physicRatio;
        private void OnEnable()
        {
            obj = new SerializedObject(target);
            
            damageType = obj.FindProperty("_damageType");
            magicRatio = obj.FindProperty("_magicRatio");
            physicRatio = obj.FindProperty("_physicRatio");
        }

        public override void OnInspectorGUI()
        {
            obj.Update();
            UE_EditorGUILayout.PropertyField(damageType);
            switch ((AtkDamageType)damageType.enumValueIndex)
            {
                case AtkDamageType.Both:
                    DrawRatio(true,true);
                    break;
                case AtkDamageType.Magic:
                    DrawRatio(true,false);
                    break;
                case AtkDamageType.Physic:
                    DrawRatio(false,true);
                    break;
            }
            
            obj.ApplyModifiedProperties();
        }

        private void DrawRatio(bool drawMagic, bool drawPhysic)
        {
            if (drawMagic)
            {
                UE_EditorGUILayout.PropertyField(magicRatio);
            }
            UE_EditorGUILayout.Space();
            if (drawPhysic)
            {
                UE_EditorGUILayout.PropertyField(physicRatio);
            }
        }
    }
}