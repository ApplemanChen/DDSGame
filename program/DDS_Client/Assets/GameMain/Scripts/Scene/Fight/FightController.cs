using System;
using GameFramework.Event;
using UnityEngine;
using System.Collections.Generic;

public class FightController : MonoBehaviour {
    public List<Rope> ropeList;
    public Rigidbody2D player;

    private Rope m_CurRope = null;
    private HingeJoint2D m_BodyJoint;
    //默认向右
    private int m_Drection = 1;
    private bool m_IsConnected = false;

    private void Awake()
    {
        m_BodyJoint = player.GetComponent<HingeJoint2D>();

        GameManager.Event.Subscribe(TriggerRopItemEventArgs.EventId, OnTriggerRopeItem);
    }

    private void OnTriggerRopeItem(object sender, GameEventArgs e)
    {
        TriggerRopItemEventArgs evt = (TriggerRopItemEventArgs)e;
        if(m_CurRope != evt.Rope)
        {
            m_CurRope = evt.Rope;
            m_CurRope.ConnectBody(m_BodyJoint);
            m_IsConnected = true;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (m_IsConnected == false)
            {
                ropeList[0].ConnectBody(m_BodyJoint);
                m_CurRope = ropeList[0];
                m_IsConnected = true;
            }else
            {
                ReleaseBody();
                m_IsConnected = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            m_Drection = -1;

            if(m_CurRope)
            {
                m_CurRope.AddForce(Vector2.left * 1000);
            }
        }

        if(Input.GetKeyDown(KeyCode.D))
        {
            m_Drection = 1;

            if(m_CurRope)
            {
                m_CurRope.AddForce(Vector2.right * 1000);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            player.AddForce(new Vector3(m_Drection*100, 3000,0));
        }

    }

    /// <summary>
    /// 释放人物
    /// </summary>
    public void ReleaseBody()
    {
        m_BodyJoint.enabled = false;
        m_BodyJoint.connectedBody = null;
    }
}
