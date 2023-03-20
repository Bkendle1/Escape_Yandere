using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doors : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private bool isOpen;
    [SerializeField] private bool isOpenForAgent;
    [SerializeField] private float openDistance;
    private Transform m_player;
    private Transform m_agent;
    
    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        m_agent = GameObject.FindWithTag("Agent").transform;
        m_player = GameObject.FindWithTag("Player").transform;
        isOpen = false;
        isOpenForAgent = false;
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
            Debug.Log("inside distance is less than open distance");
            //if the door is closed...
            if (isOpen == false)
            {
                Debug.Log("isOpen == false");

                if (m_player.GetComponent<FPSControls>().hasFired)
                {
                    StartCoroutine(OpenDoor());
                    isOpen = true;

                }
            }
            //if the door is open...
            else if (isOpen)
            {
                Debug.Log("isOpen == true");

                if (m_player.GetComponent<FPSControls>().hasFired)
                {
                    StartCoroutine(CloseDoor());
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
                StartCoroutine(OpenDoor());
                isOpenForAgent = true;
            } 
        }
        else
        {
            if (isOpenForAgent)
            {
                StartCoroutine(CloseDoor());
                isOpenForAgent = false;
            }
        }
    }
        
    

    IEnumerator OpenDoor()
    {
        Debug.Log("Door open");
        anim.Play("Opening");
        yield return new WaitForSeconds(.5f);
    }

    IEnumerator CloseDoor()
    {
        Debug.Log("Door closed");
        anim.Play("Closing");
        yield return new WaitForSeconds(.5f);
    }
}
