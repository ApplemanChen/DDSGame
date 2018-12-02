//------------------------------------------------------------
// Copyright © 2017-2020 Chen Hua. All rights reserved.
// CreateTime:2018/11/30 14:48:37
// Author: 一条猪儿虫
// Email: 1184923569@qq.com
//------------------------------------------------------------

using GameFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

/// <summary>
/// 计时器管理组件
/// </summary>
[DisallowMultipleComponent]
[AddComponentMenu("Game Framework/Custom/Timer")]
public sealed class TimerComponent : GameFrameworkComponent
{
    private static int s_SerialId = 0;

    private Type m_TimerClassType; 
    private Dictionary<int, TimerBase> m_TimerDict;
    private List<int> m_WillReleaseTimerIdList;
    private List<int> m_AutoReleaseTimerIdList;

    /// <summary>
    /// 计时器类型
    /// </summary>
    [SerializeField]
    private string m_TimerTypeName = "DefaultTimer";

    [SerializeField]
    private int m_TimerCount = 0;

    protected override void Awake()
    {
        base.Awake();

        m_TimerDict = new Dictionary<int, TimerBase>();
        m_AutoReleaseTimerIdList = new List<int>();
        m_WillReleaseTimerIdList = new List<int>();
    }

    private void Start()
    {
        InitTimerType();
    }

    /// <summary>
    /// 轮训
    /// </summary>
    private void Update()
    {
        if (m_TimerDict.Count == 0) return;

        //清理计时器
        ReleaseTimers();

        //轮训计时器
        m_TimerCount = m_TimerDict.Count;
        IDictionaryEnumerator iter = m_TimerDict.GetEnumerator();
        while(iter.MoveNext())
        {
            TimerBase current = (TimerBase)iter.Value;
            //此处暂时用逻辑流逝时间
            current.Update(Time.deltaTime);
        }

    }

    private void ReleaseTimers()
    {
        if (m_AutoReleaseTimerIdList.Count == 0) return;

        //加入待释放列表
        m_WillReleaseTimerIdList.Clear();
        int count = m_AutoReleaseTimerIdList.Count;
        for(int i = 0;i<count;i++)
        {
            int timerId = m_AutoReleaseTimerIdList[i];
            TimerBase timer = GetTimer(timerId);
            if(timer != null && timer.IsCompleted)
            {
                m_WillReleaseTimerIdList.Add(timerId);
            }
        }

        //释放
        int count2 = m_WillReleaseTimerIdList.Count;
        for(int i = 0;i< count2;i++)
        {
            RemoveTimer(m_WillReleaseTimerIdList[i]);
            m_AutoReleaseTimerIdList.Remove(m_WillReleaseTimerIdList[i]);
        }
    }

    private void InitTimerType()
    {
        if (string.IsNullOrEmpty(m_TimerTypeName))
        {
            return;
        }

        m_TimerClassType = Utility.Assembly.GetType(m_TimerTypeName);
        if (m_TimerClassType == null)
        {
            throw new GameFrameworkException(Utility.Text.Format("Can not find timer type '{0}'.", m_TimerTypeName));
        }
    }

    private TimerBase CreateTimer(int timerId,float duration, int times, TimerRoundCompleteCallback roundComplete, TimerAllCompleteCallback allComplete)
    {
        TimerBase timer = (TimerBase)Activator.CreateInstance(m_TimerClassType,new object[] { timerId,duration, times,roundComplete,allComplete });
        if(timer == null)
        {
            throw new GameFrameworkException(Utility.Text.Format("Can not create timer instance '{0}'.", m_TimerTypeName));
        }

        return timer;
    }

    /// <summary>
    /// 添加一次性计时器
    /// </summary>
    /// <param name="duration">时间间隔</param>
    /// <param name="times">次数</param>
    /// <param name="allComplete">完成回调</param>
    /// <param name="isAutoRelease">执行完成是否释放</param>
    /// <returns></returns>
    public TimerBase AddTimer(float duration, int times,TimerAllCompleteCallback allComplete,bool isAutoRelease = true)
    {
        return AddTimer(duration,times,null,allComplete,isAutoRelease);
    }

    /// <summary>
    /// 添加计时器
    /// </summary>
    /// <param name="duration">时间间隔</param>
    /// <param name="times">次数</param>
    /// <param name="roundComplete">单次完成回调</param>
    /// <param name="allComplete">全部完成回调</param>
    /// <param name="isAutoRelease">执行完成是否释放</param>
    public TimerBase AddTimer(float duration,int times, TimerRoundCompleteCallback roundComplete,TimerAllCompleteCallback allComplete,bool isAutoRelease = true)
    {
        int timerId = GenerateTimerId();
        TimerBase timer = CreateTimer(timerId, duration, times, roundComplete,allComplete);

        if(m_TimerDict.ContainsKey(timerId))
        {
            m_TimerDict[timerId] = timer;
        }else
        {
            m_TimerDict.Add(timerId,timer);
        }

        if(isAutoRelease)
        {
            m_AutoReleaseTimerIdList.Add(timerId);
        }

        return timer;
    }

    /// <summary>
    /// 开启计时器
    /// </summary>
    /// <param name="timerId"></param>
    public void StartTimer(int timerId)
    {
        TimerBase timer = GetTimer(timerId);
        timer.Start();
    }

    /// <summary>
    /// 关闭计时器
    /// </summary>
    public void StopTimer(int timerId)
    {
        TimerBase timer = GetTimer(timerId);
        timer.Stop();
    }

    /// <summary>
    /// 移除计时器
    /// </summary>
    /// <param name="timerId"></param>
    public bool RemoveTimer(int timerId)
    {
        if(m_TimerDict.ContainsKey(timerId))
        {
            if(m_TimerDict.Remove(timerId))
            {
                s_SerialId--;
                return true;
            }
        }

        throw new GameFrameworkException(Utility.Text.Format("Can not remove timer timerId is '{0}'.", timerId));
    }

    /// <summary>
    /// 获取Timer
    /// </summary>
    /// <param name="timerId"></param>
    public TimerBase GetTimer(int timerId)
    {
        TimerBase timer = null;
        m_TimerDict.TryGetValue(timerId, out timer);
        return timer;
    }

    /// <summary>
    /// 生成计时器id
    /// </summary>
    /// <returns></returns>
    private int GenerateTimerId()
    {
        return ++s_SerialId;
    }

    /// <summary>
    /// 清理所有计时器
    /// </summary>
    public void Clear()
    {
        m_TimerDict.Clear();
        m_AutoReleaseTimerIdList.Clear();
        m_WillReleaseTimerIdList.Clear();
    }
}
