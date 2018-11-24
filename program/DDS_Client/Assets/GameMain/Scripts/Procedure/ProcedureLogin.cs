//------------------------------------------------------------
// Copyright © 2017-2020 Chen Hua. All rights reserved.
// CreateTime:2018/11/24 9:50:25
// Author: 一条猪儿虫
// Email: 1184923569@qq.com
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using GameFramework.Fsm;
using GameFramework.Procedure;
using UnityEngine;
using UnityGameFramework.Runtime;

/// <summary>
/// 登录流程
/// </summary>
public class ProcedureLogin:GameProcedureBase
{
    /// <summary>
    /// 是否切换场景
    /// </summary>
    public bool IsEnterScene
    {
        get;
        set;
    }

    protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
    {
        base.OnEnter(procedureOwner);

        IsEnterScene = false;
        GameManager.UI.OpenUIForm(UIFormId.LoginForm,this);
    }

    protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

        if(IsEnterScene)
        {
            GameManager.UI.CloseUIForm(UIFormId.LoginForm);

            ChangeState<ProcedureChangeScene>(procedureOwner);
        }
    }

    protected override void OnLeave(IFsm<IProcedureManager> procedureOwner, bool isShutdown)
    {
        base.OnLeave(procedureOwner, isShutdown);
    }
}
