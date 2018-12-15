//------------------------------------------------------------
// Copyright © 2017-2020 Chen Hua. All rights reserved.
// CreateTime:2018/12/9 13:17:24
// Author: 一条猪儿虫
// Email: 1184923569@qq.com
//------------------------------------------------------------

using GameFramework;
using UnityEngine;

/// <summary>
/// 绳结
/// </summary>
public class RopeItem:MonoBehaviour
{
    private void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            GameManager.Event.Fire(this, ReferencePool.Acquire<TriggerRopItemEventArgs>().Fill(this));
        }
    }
}