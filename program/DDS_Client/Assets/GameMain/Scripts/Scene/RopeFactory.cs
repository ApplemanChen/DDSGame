//------------------------------------------------------------
// Copyright © 2017-2020 Chen Hua. All rights reserved.
// CreateTime:2018/11/28 15:15:34
// Author: 一条猪儿虫
// Email: 1184923569@qq.com
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 绳子生成工厂
/// </summary>
public class RopeFactory:MonoBehaviour
{
    [Header("起始钉子，不可见")]
    [SerializeField]
    private Rigidbody2D m_Hook;
    [Header("绳结预制体")]
    [SerializeField]
    private GameObject m_ItemPrefab;
    [Header("绳结个数")]
    [SerializeField]
    private int m_ItemLen;

    private void Start()
    {
        GenerateRope(m_ItemPrefab, m_ItemLen);
    }

    /// <summary>
    /// 生成绳子
    /// </summary>
    /// <param name="prefab">绳结的预制体</param>
    /// <param name="len">绳结个数</param>
    public void GenerateRope(GameObject prefab,int len)
    {
        if(prefab.GetComponent<HingeJoint2D>() == null)
        {
            Debug.LogError("RopeFactory: ==> 绳结预制体上没有HingeJoint2D组件！");
            return;
        }

        Rigidbody2D temp = m_Hook;
        //生成
        for (int i = 0;i<len;i++)
        {
            GameObject itemGo = GameObject.Instantiate<GameObject>(prefab);
            itemGo.transform.SetParent(transform);
            itemGo.transform.localEulerAngles = Vector3.zero;
            itemGo.transform.localPosition = Vector3.zero;
            itemGo.transform.localScale = Vector3.one;

            HingeJoint2D hingeJoint = itemGo.GetComponent<HingeJoint2D>();
            hingeJoint.connectedBody = temp;

            temp = itemGo.GetComponent<Rigidbody2D>();
        }
    }
}
