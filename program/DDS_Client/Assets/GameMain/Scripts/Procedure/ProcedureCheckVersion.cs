//------------------------------------------------------------
// Copyright © 2017-2020 Chen Hua. All rights reserved.
// Author: 一条猪儿虫
// Email: 1184923569@qq.com
//------------------------------------------------------------

using System;
using UnityGameFramework.Runtime;
using GameFramework;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
using UnityEngine;
using GameFramework.Resource;

/// <summary>
/// 版本检测流程
/// </summary>
public class ProcedureCheckVersion : GameProcedureBase
{
    private bool m_IsUpdateVersionListComplete = false;
    private VersionInfo m_LatestVersionInfo;

    protected override void OnEnter(ProcedureOwner procedureOwner)
    {
        base.OnEnter(procedureOwner);

        m_IsUpdateVersionListComplete = false;
        GameManager.Event.Subscribe(WebRequestSuccessEventArgs.EventId,OnWebRequestSuccess);
        GameManager.Event.Subscribe(WebRequestFailureEventArgs.EventId,OnWebRequestFailure);

        GameManager.WebRequest.AddWebRequest(GameManager.Config.GetString(Const.BuildConfigKey.CheckVersionUrl));
    }


    protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

        if(m_IsUpdateVersionListComplete)
        {
            ChangeState<ProcedureUpdateResource>(procedureOwner);
        }
    }

    protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
    {
        GameManager.Event.Unsubscribe(WebRequestSuccessEventArgs.EventId, OnWebRequestSuccess);
        GameManager.Event.Unsubscribe(WebRequestFailureEventArgs.EventId, OnWebRequestFailure);

        base.OnLeave(procedureOwner, isShutdown);
    }

    private void OnWebRequestFailure(object sender, GameFramework.Event.GameEventArgs e)
    {
        WebRequestFailureEventArgs evt = (WebRequestFailureEventArgs)e;
        Log.Error("Web Request Failure !! Url:{0}",evt.WebRequestUri);
    }


    private void OnWebRequestSuccess(object sender, GameFramework.Event.GameEventArgs e)
    {
        WebRequestSuccessEventArgs evt = (WebRequestSuccessEventArgs)e;

        Log.Info("Web Request Success! ");
         m_LatestVersionInfo = Utility.Json.ToObject<VersionInfo>(evt.GetWebResponseBytes());

        //检测版本号
        CheckVersionListResult versionListResult = GameManager.Resource.CheckVersionList(m_LatestVersionInfo.LatestInternalResourceVersion);
        if(versionListResult == CheckVersionListResult.Updated)
        {
            Log.Info("检查版本结果：已是最新版本，无需更新！");
            Log.Info("最新当前资源适用游戏版本: ApplicableGameVersion : {0}.", GameManager.Resource.ApplicableGameVersion);
            Log.Info("最新内部资源版本号: {0}.", GameManager.Resource.InternalResourceVersion);
            m_IsUpdateVersionListComplete = true;
            return;
        }else if(versionListResult == CheckVersionListResult.NeedUpdate)
        {
            Log.Info("检查版本结果：需要更新！");
            GameManager.Resource.UpdatePrefixUri = Utility.Path.GetCombinePath(m_LatestVersionInfo.ResourceUpdateUrl, GetResourceVersion(), GetOSName());
            GameManager.Resource.UpdateVersionList(m_LatestVersionInfo.ResourceLength, m_LatestVersionInfo.ResourceHashCode, m_LatestVersionInfo.ResourceZipLength, m_LatestVersionInfo.ResourceZipHashCode,new UpdateVersionListCallbacks(OnUpdateVersionListSuccess, OnUpdateVersionListFailure));
        }

    }

    private void OnUpdateVersionListFailure(string downloadUri, string errorMessage)
    {
        Log.Error("version.dat 更新失败！");
    }

    private void OnUpdateVersionListSuccess(string downloadPath, string downloadUri)
    {
        Log.Info("version.dat 更新完成！");
        m_IsUpdateVersionListComplete = true;
    }

    private string GetResourceVersion()
    {
        var versionStr = Application.version.Replace('.', '_');
        return string.Format("{0}_{1}", versionStr, m_LatestVersionInfo.LatestInternalResourceVersion);
    }

    private string GetOSName()
    {
        string name = string.Empty;

        switch(Application.platform)
        {
            case RuntimePlatform.Android:
                name = "android";
                break;
            case RuntimePlatform.IPhonePlayer:
                name = "ios";
                break;
            case RuntimePlatform.WindowsEditor :
            case RuntimePlatform.WindowsPlayer:
                name = "windows";    
                break;
        }

        return name;
    }
}
