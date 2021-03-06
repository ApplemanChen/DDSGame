﻿//------------------------------------------------------------
// Copyright © 2017-2020 Chen Hua. All rights reserved.
// Author: 一条猪儿虫
// Email: 1184923569@qq.com
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Resource;
using GameFramework.Localization;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
using UnityEngine;
using GameFramework.Event;
using System.Net;
using network;

/// <summary>
/// 启动流程
/// 打开LaunchForm -> 初始化配置
/// </summary>
public class ProcedureLaunch:GameProcedureBase
{
    private bool _isLanguageInitComplete = false;
    private bool _isQualityInitComplete = false;
    private bool _isSoundInitComplete = false;
    private bool _isNetworkInitComplete = false;

    protected override void OnEnter(ProcedureOwner procedureOwner)
    {
        base.OnEnter(procedureOwner);

        _isLanguageInitComplete = false;
        _isQualityInitComplete = false;
        _isSoundInitComplete = false;
        _isNetworkInitComplete = false;

        SubscribeEvents();

        //单机模式下（非编辑器模式）需要初始化资源
        if(!GameManager.Base.EditorResourceMode && GameManager.Resource.ResourceMode == ResourceMode.Package)
        {
            GameManager.Resource.InitResources(OpenLaunchForm);
        }else
        {
            OpenLaunchForm();
        }
    }

    protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

        if( _isLanguageInitComplete && _isQualityInitComplete && _isSoundInitComplete && _isNetworkInitComplete)
        {
            ChangeState<ProcedureSplash>(procedureOwner);
        }
    }

    protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
    {
        UnsubscribEvents();

        base.OnLeave(procedureOwner, isShutdown);
    }

    private void SubscribeEvents()
    {
        GameManager.Event.Subscribe(UnityGameFramework.Runtime.OpenUIFormSuccessEventArgs.EventId,OnOpenLaunchFormSuccess);
        GameManager.Event.Subscribe(UnityGameFramework.Runtime.OpenUIFormFailureEventArgs.EventId, OnOpenLaunchFormFailure);
        GameManager.Event.Subscribe(UnityGameFramework.Runtime.NetworkConnectedEventArgs.EventId, OnNetworkConneted);
        GameManager.Event.Subscribe(UnityGameFramework.Runtime.NetworkErrorEventArgs.EventId,OnNetworkError);
    }

    private void UnsubscribEvents()
    {
        GameManager.Event.Unsubscribe(UnityGameFramework.Runtime.OpenUIFormSuccessEventArgs.EventId, OnOpenLaunchFormSuccess);
        GameManager.Event.Unsubscribe(UnityGameFramework.Runtime.OpenUIFormFailureEventArgs.EventId, OnOpenLaunchFormFailure);
        GameManager.Event.Unsubscribe(UnityGameFramework.Runtime.NetworkConnectedEventArgs.EventId, OnNetworkConneted);
        GameManager.Event.Unsubscribe(UnityGameFramework.Runtime.NetworkErrorEventArgs.EventId, OnNetworkError);
    }

    private void OpenLaunchForm()
    {
        //由于启动界面打开时，界面配置还未加载完成，所以必须用这种方法打开，否则会报错！
        string assetName = AssetUtility.GetUIFormAsset(UIFormId.LaunchForm.ToString());
        GameManager.UI.OpenUIForm(assetName, "UI");
    }

    private void OnOpenLaunchFormSuccess(object sender, GameEventArgs e)
    {
        InitAll();
    }

    private void OnOpenLaunchFormFailure(object sender, GameEventArgs e)
    {
        OpenUIFormFailureEventArgs evt = (OpenUIFormFailureEventArgs)e;

        Log.Error(evt.ErrorMessage);
    }

    private void InitAll()
    {
        //基础配置
        InitBaseConfig();
        //语言配置初始化
        InitLanguageSetting();
        //画质配置初始化：
        InitQualitySetting();
        //声音配置初始化
        InitSoundSetting();
        //网络初始化
        //InitNetwork();
        _isNetworkInitComplete = true;
    }

    private void UpdateLaunchTips(string tips)
    {
        GameManager.Event.Fire(this, ReferencePool.Acquire<LaunchFormUpdateTipsEventArgs>().Fill(tips));
    }

    /// <summary>
    /// 初始化基础配置
    /// </summary>
    private void InitBaseConfig()
    {
        UpdateLaunchTips("正在进行基础配置...");
        GameManager.BaseConfig.InitServerConfig();
        Log.Info("Launch => Base config init complete.");
    }


    /// <summary>
    /// 初始化语言配置
    /// </summary>
    private void InitLanguageSetting()
    {
        if(GameManager.Base.EditorResourceMode && GameManager.Base.EditorLanguage != Language.Unspecified)
        {
            //编辑器模式下用已指定的语言
            return;
        }

        Language language = GameManager.Localization.Language;
        string languageSetting = GameManager.Setting.GetString(Const.SettingKey.Language);
        if(!string.IsNullOrEmpty(languageSetting))
        {
            try
            {
                language = (Language)Enum.Parse(typeof(Language), languageSetting);
            }catch
            {
                Log.Error("Localization saved language can't convert to enum value.The string is {0}.",languageSetting);
            }
        }

        if (language != Language.English
            && language != Language.ChineseSimplified)
        {
            // 若是暂不支持的语言，则使用英语
            language = Language.English;

            GameManager.Setting.SetString(Const.SettingKey.Language, language.ToString());
            GameManager.Setting.Save();
        }

        GameManager.Localization.Language = language;

        _isLanguageInitComplete = true;
        UpdateLaunchTips("语言配置完成！");
        Log.Info("Launch => Language init complete.");
    }

    /// <summary>
    /// 初始化画质配置
    /// </summary>
    public void InitQualitySetting()
    {
        //此处可拓展为根据不同机型模型进行画质配置，根据检测到的硬件信息 Assets/Main/Configs/DeviceModelConfig 和用户配置数据，设置即将使用的画质选项。
        QualityLevel defaultQuality = QualityLevel.Good;
        int settingQuality = GameManager.Setting.GetInt(Const.SettingKey.Quality, (int)defaultQuality);
        QualitySettings.SetQualityLevel(settingQuality);

        _isQualityInitComplete = true;
        UpdateLaunchTips("画质配置完成！");
        Log.Info("Launch => Quality init complete.");
    }

    /// <summary>
    /// 初始化声音设置
    /// </summary>
    public void InitSoundSetting()
    {
        GameManager.Sound.AddSoundGroup("Music", 1);
        GameManager.Sound.AddSoundGroup("Sound",10);
        GameManager.Sound.AddSoundGroup("UISound",5);

        GameManager.Sound.SetMute("Music", GameManager.Setting.GetBool(Const.SettingKey.MusicMuted, false));
        GameManager.Sound.SetMute("Sound", GameManager.Setting.GetBool(Const.SettingKey.SoundMuted, false));
        GameManager.Sound.SetMute("UISound", GameManager.Setting.GetBool(Const.SettingKey.UISoundMuted, false));
        GameManager.Sound.SetVolume("Music", GameManager.Setting.GetFloat(Const.SettingKey.MusicVolume,1f));
        GameManager.Sound.SetVolume("Sound", GameManager.Setting.GetFloat(Const.SettingKey.SoundVolume,1f));
        GameManager.Sound.SetVolume("UISound", GameManager.Setting.GetFloat(Const.SettingKey.UISoundVolume,1f));

        _isSoundInitComplete = true;
        //Log.Info("PrecedureLaunch ==> Sound setting init complete!");
        UpdateLaunchTips("声音配置完成！");
    }

    /// <summary>
    /// 初始化网络
    /// </summary>
    private void InitNetwork()
    {
        GameManager.Network.CreateNetworkChannel(Const.ServerConfigKey.GameServerIP);
        GameManager.Network.ConnectGameChannel();
    }

    private void OnNetworkConneted(object sender, GameEventArgs e)
    {
        Log.Info("连接上服务器~~~");
        _isNetworkInitComplete = true;

        cs_login loginInfo = new cs_login();
        loginInfo.account = "1234";
        loginInfo.password = "abc";
        GameManager.Network.Send<cs_login>(loginInfo);
    }

    private void OnNetworkError(object sender, GameEventArgs e)
    {
        Log.Error("网络连接错误！");
        UpdateLaunchTips("网络链接错误！！");
    }
}