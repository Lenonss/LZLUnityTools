using System.Collections.Generic;
using UnityEditor;

namespace LZLToolBox.PlayerController
{
    [CustomEditor(typeof(LZLAttackNormalAttributeEditor))]
    public class LZLAttackNormalAttributeEditor : Editor
    {
        private SerializedObject obj; //序列化

        private SerializedProperty selfAnimator;

        private SerializedProperty nolAttackCycle, nolAtkAnimParameters;
        
        void OnEnable()
        {
            obj = new SerializedObject(target);
            //获得属性字段
            selfAnimator = obj.FindProperty("_selfAnimator");
            nolAttackCycle = obj.FindProperty("_nolAttackCycles");
            nolAtkAnimParameters = obj.FindProperty("_nolAtkAnimParameters");
            
        }

        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();
            obj.Update();
            EditorGUILayout.PropertyField(selfAnimator);
            EditorGUILayout.PropertyField(nolAttackCycle);
            nolAtkAnimParameters.arraySize = 2; //nolAttackCycle.intValue;
        }
    }
}