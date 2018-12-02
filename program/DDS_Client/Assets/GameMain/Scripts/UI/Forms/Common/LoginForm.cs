//------------------------------------------------------------
// Copyright © 2017-2020 Chen Hua. All rights reserved.
// CreateTime:2018/11/24 9:38:39
// Author: 一条猪儿虫
// Email: 1184923569@qq.com
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

/// <summary>
/// 登录界面
/// </summary>
public class LoginForm : NGuiForm
{
    private UIButton m_Btn;

    private ProcedureLogin m_Procedure;

    private int m_TimerId;

    protected internal override void OnInit(object userData)
    {
        base.OnInit(userData);
        m_Procedure = (ProcedureLogin)userData;

        m_Btn = CachedTransform.Find("Panel/BtnStart").GetComponent<UIButton>();
        UIEventListener.Get(m_Btn.gameObject).onClick = OnBtnClick;

        GameManager.Timer.AddTimer(1f, 10, OnRoundComplete, OnAllComplete).Start();
        GameManager.Timer.AddTimer(1f, 3, OnRoundComplete2, OnAllComplete2).Start();
    }

    private void OnRoundComplete2(int timerId)
    {
        Log.Info("2 LoginForm: => {0}", GameManager.Timer.GetTimer(timerId).PassTimes);
    }

    private void OnAllComplete2(int timerId)
    {
        Log.Info("2 全部完成！");
    }

    private void OnAllComplete(int timerId)
    {
        Log.Info("1 全部完成！");
    }

    private void OnRoundComplete(int timerId)
    {
        Log.Info("1 LoginForm: => {0}", GameManager.Timer.GetTimer(timerId).PassTimes);
    }

    private void OnBtnClick(GameObject go)
    {
        //进入主菜单流程
        m_Procedure.ProcedureOwner.SetData<VarInt>(Const.ProcedureDataKey.NextSceneId, (int)SceneId.FightScene);
        m_Procedure.IsEnterScene = true;
    }
}
