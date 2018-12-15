//------------------------------------------------------------
// Copyright © 2017-2020 Chen Hua. All rights reserved.
// CreateTime:2018/11/30 15:35:38
// Author: 一条猪儿虫
// Email: 1184923569@qq.com
//------------------------------------------------------------

using System;
using GameFramework;
/// <summary>
/// 计时器基类
/// </summary>
public abstract class TimerBase
{
    /// <summary>
    /// 唯一标示
    /// </summary>
    public abstract int TimerId { get; }

    /// <summary>
    /// 间隔
    /// </summary>
    public abstract float Duration { get; }

    /// <summary>
    /// 总的循环执行次数
    /// </summary>
    public abstract int Times { get; }

    /// <summary>
    /// 已经执行过的次数
    /// </summary>
    public abstract int PassTimes { get; }

    /// <summary>
    /// 时间倍率
    /// </summary>
    public float TimeScale { get; set; }

    /// <summary>
    /// 是否在执行
    /// </summary>
    public abstract bool IsRunning { get; }

    /// <summary>
    /// 是否执行完成
    /// </summary>
    public abstract bool IsCompleted { get; }

    /// <summary>
    /// 构造器
    /// </summary>
    /// <param name="timerId">唯一标示</param>
    /// <param name="duration">时间间隔</param>
    /// <param name="loopTimes">循环次数</param>
    /// <param name="roundComplete">单次完成回调</param>
    /// <param name="allComplete">全部完成回调</param>
    public TimerBase(int timerId, float duration, int loopTimes, TimerRoundCompleteCallback roundComplete, TimerAllCompleteCallback allComplete) { }

    /// <summary>
    /// 开始
    /// </summary>
    public abstract void Start();

    /// <summary>
    /// 轮训
    /// </summary>
    /// <param name="deltaTime"></param>
    public abstract void Update(float deltaTime);
    
    /// <summary>
    /// 停止
    /// </summary>
    public abstract void Stop();

    /// <summary>
    /// 重置
    /// </summary>
    public void Reset() {
        Stop();
        Start();
    }
}
