using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace LZLUnityTool.Plugins.CommonPlugin.Tools.Editor
{
    [CustomEditor(typeof(UnityLifeCycleEvents), true)]
    public class UnityLifeCycleEventsEditor : UnityEditor.Editor
    {
        SerializedProperty m_showedEvents;
        
        SerializedProperty m_OnAwakeEvent;
        SerializedProperty m_OnEnableEvent;
        SerializedProperty m_OnStartEvent;
        SerializedProperty m_OnUpdatedEvent;
        SerializedProperty m_OnFixedUpdateEvent;
        SerializedProperty m_OnLateUpdateEventEvent;
        SerializedProperty m_OnDisableEventEvent;
        SerializedProperty m_OnDestroyEventEvent;
        SerializedProperty m_OnApplicationQuitEventEvent;
        
        GUIContent m_IconToolbarMinus;
        GUIContent m_EventIDName;
        GUIContent[] m_EventTypes;
        GUIContent m_AddButonContent;

        protected virtual void OnEnable()
        {
            m_showedEvents = serializedObject.FindProperty("showedEvents");
            
            m_OnAwakeEvent = serializedObject.FindProperty("OnAwakeEvent");
            m_OnEnableEvent = serializedObject.FindProperty("OnEnableEvent");
            m_OnStartEvent = serializedObject.FindProperty("OnStartEvent");
            m_OnUpdatedEvent = serializedObject.FindProperty("OnUpdatedEvent");
            m_OnFixedUpdateEvent = serializedObject.FindProperty("OnFixedUpdateEvent");
            m_OnLateUpdateEventEvent = serializedObject.FindProperty("OnLateUpdateEvent");
            m_OnDisableEventEvent = serializedObject.FindProperty("OnDisableEvent");
            m_OnDestroyEventEvent = serializedObject.FindProperty("OnDestroyEvent");
            m_OnApplicationQuitEventEvent = serializedObject.FindProperty("OnApplicationQuitEvent");
            
            m_AddButonContent = EditorGUIUtility.TrTextContent("添加新事件");
            
            // Have to create a copy since otherwise the tooltip will be overwritten.
            m_IconToolbarMinus = new GUIContent(EditorGUIUtility.IconContent("Toolbar Minus"));
            // m_IconToolbarMinus.tooltip = "Remove all events in this list.";
            
            m_EventIDName = new GUIContent("");
            
            string[] eventNames = Enum.GetNames(typeof(UnityLifeCycleType));
            m_EventTypes = new GUIContent[eventNames.Length];
            for (int i = 0; i < eventNames.Length; ++i)
            {
                m_EventTypes[i] = new GUIContent(eventNames[i]);
            }
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            int toBeRemovedEntry = -1;

            EditorGUILayout.Space();

            Vector2 removeButtonSize = GUIStyle.none.CalcSize(m_IconToolbarMinus);

            for (int i = 0; i < m_showedEvents.arraySize; ++i)
            {
                SerializedProperty eventType = m_showedEvents.GetArrayElementAtIndex(i);
                m_EventIDName.text = eventType.enumDisplayNames[eventType.enumValueIndex];

                ShowTargetParam(eventType.enumValueIndex);
                //显示对应Type的参数
                // EditorGUILayout.PropertyField(callbacksProperty, m_EventIDName);
                Rect callbackRect = GUILayoutUtility.GetLastRect();

                Rect removeButtonPos = new Rect
                    (callbackRect.xMax - removeButtonSize.x - 8, callbackRect.y + 1, removeButtonSize.x, removeButtonSize.y);
                if (GUI.Button(removeButtonPos, m_IconToolbarMinus, GUIStyle.none))
                {
                    toBeRemovedEntry = i;
                }

                EditorGUILayout.Space();
            }

            if (toBeRemovedEntry > -1)
            {
                RemoveEntry(toBeRemovedEntry);
            }
            
            Rect btPosition = GUILayoutUtility.GetRect(m_AddButonContent, GUI.skin.button);
            const float addButonWidth = 200f;
            btPosition.x = btPosition.x + (btPosition.width - addButonWidth) / 2;
            btPosition.width = addButonWidth;
            if (GUI.Button(btPosition, m_AddButonContent))
            {
                ShowAddTriggermenu();
            }
            serializedObject.ApplyModifiedProperties();
        }

        private void ShowTargetParam(int type)
        {
            switch ((UnityLifeCycleType)(type))
            {
                case UnityLifeCycleType.Awake:
                    EditorGUILayout.PropertyField(m_OnAwakeEvent);
                    break;
                case UnityLifeCycleType.OnEnable:
                    EditorGUILayout.PropertyField(m_OnEnableEvent);
                    break;
                case UnityLifeCycleType.Start:
                    EditorGUILayout.PropertyField(m_OnStartEvent);
                    break;
                case UnityLifeCycleType.Update:
                    EditorGUILayout.PropertyField(m_OnUpdatedEvent);
                    break;
                case UnityLifeCycleType.FixedUpdate:
                    EditorGUILayout.PropertyField(m_OnFixedUpdateEvent);
                    break;
                case UnityLifeCycleType.LateUpdate:
                    EditorGUILayout.PropertyField(m_OnLateUpdateEventEvent);
                    break;
                case UnityLifeCycleType.OnDisable:
                    EditorGUILayout.PropertyField(m_OnDisableEventEvent);
                    break;
                case UnityLifeCycleType.OnDestroy:
                    EditorGUILayout.PropertyField(m_OnDestroyEventEvent);
                    break;
                case UnityLifeCycleType.OnApplicationQuit:
                    EditorGUILayout.PropertyField(m_OnApplicationQuitEventEvent);
                    break;
            }
        }

        private void RemoveEntry(int toBeRemovedEntry)
        {
            m_showedEvents.DeleteArrayElementAtIndex(toBeRemovedEntry);
        }
        
        void ShowAddTriggermenu()
        {
            // Now create the menu, add items and show it
            GenericMenu menu = new GenericMenu();
            for (int i = 0; i < m_EventTypes.Length; ++i)
            {
                bool active = true;

                // Check if we already have a Entry for the current eventType, if so, disable it
                for (int p = 0; p < m_showedEvents.arraySize; ++p)
                {
                    SerializedProperty itemType = m_showedEvents.GetArrayElementAtIndex(p);
                    if (itemType.enumValueIndex == i)
                    {
                        active = false;
                    }
                }
                if (active)
                    menu.AddItem(m_EventTypes[i], false, OnAddNewSelected, i);
                else
                    menu.AddDisabledItem(m_EventTypes[i]);
            }
            menu.ShowAsContext();
            Event.current.Use();
        }
        
        private void OnAddNewSelected(object index)
        {
            int selected = (int)index;

            m_showedEvents.arraySize += 1;
            SerializedProperty itemType = m_showedEvents.GetArrayElementAtIndex(m_showedEvents.arraySize - 1);
            itemType.enumValueIndex = selected;
            serializedObject.ApplyModifiedProperties();
        }
    }
}