//------------------------------------------------------------
// Copyright © 2017-2020 Chen Hua. All rights reserved.
// CreateTime:2018/12/1 10:12:12
// Author: 一条猪儿虫
// Email: 1184923569@qq.com
//------------------------------------------------------------

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityGameFramework.Editor;


[CustomEditor(typeof(TimerComponent))]
sealed class TimerComponentInspector : GameFrameworkInspector
{
    private const string NoneOptionName = "<None>";

    private SerializedProperty m_TimerTypeName = null;
    private string[] m_TimerTypeNames = null;
    private int m_TimerTypeNameIndex = 0;

    private int m_TimerCount;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.Update();

        EditorGUI.BeginDisabledGroup(EditorApplication.isPlayingOrWillChangePlaymode);
        {
            int timerTypeSelectedIndex = EditorGUILayout.Popup("Timer Type", m_TimerTypeNameIndex, m_TimerTypeNames);
            if(timerTypeSelectedIndex != m_TimerTypeNameIndex)
            {
                m_TimerTypeNameIndex = timerTypeSelectedIndex;
                m_TimerTypeName.stringValue = (timerTypeSelectedIndex <= 0 ? null : m_TimerTypeNames[timerTypeSelectedIndex]);
            }

            m_TimerCount = serializedObject.FindProperty("m_TimerCount").intValue;
            EditorGUILayout.IntField("Timer Count", m_TimerCount);
        }

        serializedObject.ApplyModifiedProperties();
    }

    protected override void OnCompileComplete()
    {
        base.OnCompileComplete();

        RefreshTypeNames();
    }

    private void OnEnable()
    {
        m_TimerTypeName = serializedObject.FindProperty("m_TimerTypeName");

        RefreshTypeNames();
    }

    private void RefreshTypeNames()
    {
        List<string> timerTypeNames = new List<string>();
        timerTypeNames.Add(NoneOptionName);
        timerTypeNames.AddRange(Type.GetTypeNames(typeof(TimerBase)));

        m_TimerTypeNames = timerTypeNames.ToArray();
        m_TimerTypeNameIndex = 0;
        if(!string.IsNullOrEmpty(m_TimerTypeName.stringValue))
        {
            m_TimerTypeNameIndex = timerTypeNames.IndexOf(m_TimerTypeName.stringValue);
            if(m_TimerTypeNameIndex < 0)
            {
                m_TimerTypeNameIndex = 0;
                m_TimerTypeName.stringValue = null;
            }
        }
    }
}
