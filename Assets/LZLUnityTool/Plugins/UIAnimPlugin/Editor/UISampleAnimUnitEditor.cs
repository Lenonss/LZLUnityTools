using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace LZLUnityTool.Plugins.UIAnimPlugin
{
    [CustomEditor(typeof(UISampleAnimUnit))]
    public class UISampleAnimUnitEditor : Editor
    {
        private SerializedObject obj; //序列化
        
        /// <summary>
        /// AnimType选择相关
        /// </summary>
        private SerializedProperty anchorV2,localRV3,sizeV3,animType,rotateMode
            ,isSnaping;
        /// <summary>
        /// 通用属性字段
        /// </summary>
        private SerializedProperty duration;
        void OnEnable()
        {
            obj = new SerializedObject(target);
            //获得UIAnim相关属性字段
            GetAnimTypeGUIProptery();
            duration = obj.FindProperty("duration");
        }

        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();

            obj.Update();
            DrawAnimTypeGUI();
            EditorGUILayout.PropertyField(duration);
            obj.ApplyModifiedProperties();
         
        }
        /// <summary>
        /// 获取AnimType相关的属性字段
        /// </summary>
        private void GetAnimTypeGUIProptery()
        {
            //用户的选择
            animType = obj.FindProperty("_animType");
            
            anchorV2 = obj.FindProperty("_anchorV2");
            localRV3 = obj.FindProperty("_localRotateV3");
            sizeV3 = obj.FindProperty("_sizeV3");
            rotateMode = obj.FindProperty("_rotateMode");
            isSnaping = obj.FindProperty("_isSnaping");
        }

        /// <summary>
        /// 绘制AnimType相关字段，接口
        /// </summary>
        private void DrawAnimTypeGUI()
        {
            AnimTypeArg arg;
            //空两格
            DrawGUISpace(2);
            //绘制枚举选项
            EditorGUILayout.PropertyField(animType);
            //根据选项绘制属性
            switch ((UISampleAnimType)animType.enumValueIndex)
            {
                case UISampleAnimType.AnchorV2Move:
                    arg = new AnimTypeArg()
                    {
                        showAnchorV2 = true, showLocalRV3 = false, showSizeV3 = false,
                        needSpace = true,spaceNum = 2
                    };
                    DrawAnimTypeGUI(arg);
                    break;
                case UISampleAnimType.LocalRotateV3:
                    arg = new AnimTypeArg()
                    {
                        showAnchorV2 = false, showLocalRV3 = true, showSizeV3 = false,
                        needSpace = true,spaceNum = 2
                    };
                    DrawAnimTypeGUI(arg);
                    break;
                case UISampleAnimType.SizeV3:
                    arg = new AnimTypeArg()
                    {
                        showAnchorV2 = false, showLocalRV3 = false, showSizeV3 = true,
                        needSpace = true,spaceNum = 2
                    };
                    DrawAnimTypeGUI(arg);
                    break;
                case UISampleAnimType.AV2MaSV3:
                    arg = new AnimTypeArg()
                    {
                        showAnchorV2 = true, showLocalRV3 = false, showSizeV3 = true,
                        needSpace = true,spaceNum = 2
                    };
                    DrawAnimTypeGUI(arg);
                    break;
                case UISampleAnimType.LRV3aSV3:
                    arg = new AnimTypeArg()
                    {
                        showAnchorV2 = false, showLocalRV3 = true, showSizeV3 = true,
                        needSpace = true,spaceNum = 2
                    };
                    DrawAnimTypeGUI(arg);
                    break;
                case UISampleAnimType.AV2MaLRV3:
                    arg = new AnimTypeArg()
                    {
                        showAnchorV2 = true, showLocalRV3 = true, showSizeV3 = false,
                        needSpace = true,spaceNum = 2
                    };
                    DrawAnimTypeGUI(arg);
                    break;
                case UISampleAnimType.All:
                    arg = new AnimTypeArg()
                    {
                        showAnchorV2 = true, showLocalRV3 = true, showSizeV3 = true,
                        needSpace = true,spaceNum = 2
                    };
                    DrawAnimTypeGUI(arg);
                    break;
            }
        }
        /// <summary>
        /// 绘制AnimType相关字段，附庸函数
        /// </summary>
        /// <param name="arg"></param>
        private void DrawAnimTypeGUI(AnimTypeArg arg)
        {
            if (arg.showAnchorV2)
            {
                EditorGUILayout.PropertyField(isSnaping);
                EditorGUILayout.PropertyField(anchorV2);
                //绘制空格
                if (arg.needSpace)
                    DrawGUISpace(arg.spaceNum);
            }

            if (arg.showLocalRV3)
            {
                EditorGUILayout.PropertyField(localRV3);
                EditorGUILayout.PropertyField(rotateMode);
                //绘制空格
                if (arg.needSpace)
                    DrawGUISpace(arg.spaceNum);
            }

            if (arg.showSizeV3)
            {
                EditorGUILayout.PropertyField(sizeV3);
                //绘制空格
                if (arg.needSpace)
                    DrawGUISpace(arg.spaceNum);
            }
        }

        #region 通用工具函数

        /// <summary>
        /// 绘制空行
        /// </summary>
        /// <param name="num"></param>
        private void DrawGUISpace(int num)
        {
            for (int i = 0; i < num; i++)
            {
                EditorGUILayout.Space();
            }
        }

        #endregion


        #region 工具class

        private class AnimTypeArg
        {
            /// <summary>
            /// 是否显示anchorV2属性
            /// </summary>
            public bool showAnchorV2 = false;
            /// <summary>
            /// 是否显示localRV3属性
            /// </summary>
            public bool showLocalRV3 = false;
            /// <summary>
            /// 是否显示sizeV3属性
            /// </summary>
            public bool showSizeV3 = false;
            
            /// <summary>
            ///是否需要空行 
            /// </summary>
            public bool needSpace = false;
            /// <summary>
            /// 空几行
            /// </summary>
            public int spaceNum = 0;
        }

        #endregion
    }
    
}