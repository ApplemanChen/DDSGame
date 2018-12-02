//------------------------------------------------------------
// Copyright © 2017-2020 Chen Hua. All rights reserved.
// CreateTime:2018/11/30 15:50:43
// Author: 一条猪儿虫
// Email: 1184923569@qq.com
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;

/// <summary>
/// 默认计时器
/// </summary>
public class DefaultTimer:TimerBase
{
    private int m_TimerId;
    private float m_Duration;
    private float m_PassedTime;
    private int m_LoopTimes;
    private int m_PassedLoopTimes;
    private bool m_IsRunning;
    private bool m_IsCompleted;
    //单次间隔回调
    private TimerRoundCompleteCallback m_RoundCompleteAction;
    //所有完成回调
    private TimerAllCompleteCallback m_AllCompleteAction;

    /// <summary>
    /// 唯一标识
    /// </summary>
    public override int TimerId
    {
        get
        {
            return m_TimerId;
        }
    }

    /// <summary>
    /// 间隔
    /// </summary>
    public override float Duration
    {
        get
        {
            return m_Duration;
        }
    }

    /// <summary>
    /// 总的循环执行次数
    /// </summary>
    public override int Times
    {
        get
        {
            return m_LoopTimes;
        }
    }

    /// <summary>
    /// 已经经过的循环次数
    /// </summary>
    public override int PassTimes
    {
        get
        {
            return m_PassedLoopTimes;
        }
    }

    /// <summary>
    /// 是否在执行
    /// </summary>
    public override bool IsRunning
    {
        get
        {
            return m_IsRunning;
        }
    }

    /// <summary>
    /// 是否执行完成
    /// </summary>
    public override bool IsCompleted
    {
        get
        {
            return m_IsCompleted;
        }
    }

    /// <summary>
    /// 构造器
    /// </summary>
    /// <param name="timerId">唯一标示</param>
    /// <param name="duration">时间间隔</param>
    /// <param name="loopTimes">循环次数 说明：小于0为无限循环</param>
    /// <param name="roundComplete">单次完成回调</param>
    /// <param name="allComplete">全部完成回调</param>
    public DefaultTimer(int timerId,float duration,int loopTimes, TimerRoundCompleteCallback roundComplete, TimerAllCompleteCallback allComplete):base(timerId,duration,loopTimes,roundComplete,allComplete)
    {
        m_TimerId = timerId;
        m_Duration = duration;
        m_LoopTimes = loopTimes;
        m_RoundCompleteAction = roundComplete;
        m_AllCompleteAction = allComplete;

        //默认时间流速
        TimeScale = 1;
    }

    /// <summary>
    /// 开始
    /// </summary>
    public override void Start()
    {
        m_IsRunning = true;
        m_PassedTime = 0;
        m_PassedLoopTimes = 0;
        m_IsCompleted = false;
    }

    /// <summary>
    /// 停止
    /// </summary>
    public override void Stop()
    {
        m_IsRunning = false;
    }

    /// <summary>
    /// 轮训
    /// </summary>
    /// <param name="deltaTime"></param>
    public override void Update(float deltaTime)
    {
        if (!IsRunning) return;

        m_PassedTime += deltaTime * TimeScale;

        if(m_PassedTime >= Duration)
        {
            if(m_LoopTimes <= 0)
            {
                //无限循环
                UnlimitedRun();
            }
            else if(m_LoopTimes == 1)
            {
                //执行一次
                OnceRun();
            }
            else if(m_LoopTimes > 1)
            {
                //执行多次
                ManyTimesRun();
            }

            m_PassedTime = 0;
        }
    }

    //无限次数执行
    private void UnlimitedRun()
    {
        m_PassedLoopTimes++;

        RoundCallBack();
    }

    //单次执行
    private void OnceRun()
    {
        m_PassedLoopTimes++;

        AllCompleteCallBack();
        m_IsRunning = false;
        m_IsCompleted = true;
    }

    //多次执行
    private void ManyTimesRun()
    {
        m_PassedLoopTimes++;

        if(m_PassedLoopTimes < m_LoopTimes)
        {
            RoundCallBack();
        }else
        {
            AllCompleteCallBack();
            m_IsRunning = false;
            m_IsCompleted = true;
        }
    }

    private void RoundCallBack()
    {
        if (m_RoundCompleteAction != null)
        {
            m_RoundCompleteAction(m_TimerId);
        }
    }

    private void AllCompleteCallBack()
    {
        if (m_AllCompleteAction != null)
        {
            m_AllCompleteAction(m_TimerId);
        }
    }
}
