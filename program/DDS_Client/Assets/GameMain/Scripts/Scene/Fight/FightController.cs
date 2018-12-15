using System;
using GameFramework.Event;
using UnityEngine;

public class FightController : MonoBehaviour {
    public RopeFactory rope;
    public Rigidbody2D player;

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
        Debug.Log("ropeItem:"+evt.RopeItem.name);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (m_IsConnected == false)
            {
                rope.ConnectBody(m_BodyJoint);
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
            //player.AddForce(Vector2.left * 1000);

            rope.AddForce(Vector2.left * 1000);
        }

        if(Input.GetKeyDown(KeyCode.D))
        {
            m_Drection = 1;
            //player.AddForce(Vector2.right * 1000);

            rope.AddForce(Vector2.right * 1000);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            player.AddForce(new Vector3(m_Drection*1000, 1000,0));
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
