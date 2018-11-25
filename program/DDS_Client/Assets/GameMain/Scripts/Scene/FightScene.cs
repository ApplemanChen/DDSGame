//------------------------------------------------------------
// Copyright © 2017-2020 Chen Hua. All rights reserved.
// Author: 一条猪儿虫
// Email: 1184923569@qq.com
//------------------------------------------------------------

using System;
using GameFramework;
using UnityGameFramework.Runtime;
using UnityEngine;

/// <summary>
/// 主城场景
/// </summary>
public class FightScene : SceneBase
{
    public FightScene(int sceneId):base(sceneId)
    {

    }

    protected override void OnInit()
    {
        Log.Info("SceneName:"+SceneName);

        HideSceneCamera();
    }

    protected override void OnEnter()
    {
        //GameManager.Entity.ShowPlayer(new PlayerData(GameManager.Entity.GenerateSerialId(), 10001));
    }

    protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
    {

    }

    protected override void OnExit()
    {

    }

    private void HideSceneCamera()
    {
        //场景摄像机禁用
        Camera sceneCamera = GameObject.Find("Camera").GetComponent<Camera>();
        if (sceneCamera != null)
        {
            sceneCamera.gameObject.SetActive(false);
        }
    }
}
