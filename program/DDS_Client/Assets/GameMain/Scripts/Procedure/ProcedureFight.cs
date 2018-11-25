//------------------------------------------------------------
// Copyright © 2017-2020 Chen Hua. All rights reserved.
// CreateTime:2018/11/24 10:00:36
// Author: 一条猪儿虫
// Email: 1184923569@qq.com
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using GameFramework.Fsm;
using GameFramework.Procedure;
using UnityGameFramework.Runtime;

/// <summary>
/// 战斗流程
/// </summary>
public class ProcedureFight : GameProcedureBase
{
    FightScene m_Scene;

    protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
    {
        base.OnEnter(procedureOwner);

        int sceneId = procedureOwner.GetData<VarInt>(Const.ProcedureDataKey.NextSceneId).Value;
        m_Scene = new FightScene(sceneId);
        m_Scene.Enter();
    }

    protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

        if (m_Scene != null)
        {
            m_Scene.Update(elapseSeconds, realElapseSeconds);
        }
    }

    protected override void OnLeave(IFsm<IProcedureManager> procedureOwner, bool isShutdown)
    {
        if(m_Scene != null)
        {
            m_Scene.Exit();
        }

        base.OnLeave(procedureOwner, isShutdown);
    }
}
