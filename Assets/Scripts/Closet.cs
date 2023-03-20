using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Closet : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private bool isOpen;
    [SerializeField] private bool isOpenForAgent;
    [SerializeField] private float openDistance;
    private Transform m_player;
    private Transform m_agent;
    
    void Awake()
    {
        isOpen = false;
        isOpenForAgent = false;
        anim = GetComponent<Animator>();
        
    }

    private void Start()
    {
        m_agent = GameObject.FindWithTag("Agent").transform;
        m_player = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {
        AgentAccess();
    }

    private void OnMouseOver()
    {
        float distance = Vector3.Distance(transform.position, m_player.position);
        if (distance <= openDistance)
        {
            //if the door is closed...
            if (isOpen == false)
            {
                if (m_player.GetComponent<FPSControls>().hasFired)
                {
                    StartCoroutine(OpenCloset());
                    isOpen = true;
                }
            }
            //if the door is open...
            else if (isOpen)
            {
                if (m_player.GetComponent<FPSControls>().hasFired)
                {
                    StartCoroutine(CloseCloset());
                    isOpen = false;
                }
            }
        }
    }
    
    //if agent is near a door, open it
    private void AgentAccess()
    {
        float distance = Vector3.Distance(transform.position, m_agent.position);
        if (distance <= openDistance)
        {
            if (isOpenForAgent == false)
            {
                StartCoroutine(OpenCloset());
                isOpenForAgent = true;
            } 
        }
        else
        {
            if (isOpenForAgent)
            {
                StartCoroutine(CloseCloset());
                isOpenForAgent = false;
            }
        }
    }
    
    IEnumerator OpenCloset()
    {
        Debug.Log("Closet open");
        anim.Play("ClosetOpening");
        yield return new WaitForSeconds(.5f);
    }
    
    IEnumerator CloseCloset()
    {
        Debug.Log("Closet closed");
        anim.Play("ClosetClosing");
        yield return new WaitForSeconds(.5f);
    }
}
