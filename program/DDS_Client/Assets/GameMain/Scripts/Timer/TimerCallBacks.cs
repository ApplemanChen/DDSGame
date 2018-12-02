//------------------------------------------------------------
// Copyright © 2017-2020 Chen Hua. All rights reserved.
// CreateTime:2018/12/1 13:09:52
// Author: 一条猪儿虫
// Email: 1184923569@qq.com
//------------------------------------------------------------

/// <summary>
/// 计时器单次完成回调
/// </summary>
/// <param name="timerId">计时器id</param>
public delegate void TimerRoundCompleteCallback(int timerId);

/// <summary>
/// 计时器全部完成回调
/// </summary>
/// <param name="timerId">计时器id</param>
public delegate void TimerAllCompleteCallback(int timerId);
