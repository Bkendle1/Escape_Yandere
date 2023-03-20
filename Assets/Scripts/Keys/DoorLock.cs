using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLock : MonoBehaviour
{
    [SerializeField] private Colorkeys m_lock;
    [SerializeField] private AudioClip _audioClip;
    [SerializeField] private Animator m_animator;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //does the player have the key?
            if (other.GetComponent<Manager>().DoWeHaveTheKey(m_lock))
            {
                m_animator.SetBool("IsLock", false);
            }
            //player doesn't have key
            else
            {
                _audioSource.PlayOneShot(_audioClip);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            m_animator.SetBool("IsLock", true);
        }
    }
}
