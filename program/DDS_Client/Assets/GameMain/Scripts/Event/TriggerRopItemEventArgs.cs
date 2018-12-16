//------------------------------------------------------------
// Copyright © 2017-2020 Chen Hua. All rights reserved.
// CreateTime:2018/12/9 13:19:28
// Author: 一条猪儿虫
// Email: 1184923569@qq.com
//------------------------------------------------------------

using System;
using UnityEngine;
using GameFramework.Event;

/// <summary>
/// 触发绳结事件
/// </summary>
public class TriggerRopItemEventArgs : GameEventArgs
{
    public static readonly int EventId = typeof(TriggerRopItemEventArgs).GetHashCode();

    public override int Id
    {
        get
        {
            return EventId;
        }
    }

    /// <summary>
    /// 当前绳子
    /// </summary>
    public Rope Rope
    {
        private set;
        get;
    }

    /// <summary>
    /// 触发的对应绳结
    /// </summary>
    public RopeItem RopeItem
    {
        private set;
        get;
    }

    public override void Clear()
    {
        RopeItem = null;
        Rope = null;
    }

    public TriggerRopItemEventArgs Fill(RopeItem item,Rope rope)
    {
        RopeItem = item;
        Rope = rope;
        return this;
    }
}