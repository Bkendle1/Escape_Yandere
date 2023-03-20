using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    [SerializeField] private Colorkeys m_key;
    [SerializeField] private AudioClip m_audioClip;

    private AudioSource m_audioSource;

    private void Awake()
    {
        m_audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //if player doesn't have key...
        if (other.CompareTag("Player") && !other.GetComponent<Manager>().m_keyStorage.Contains(m_key))
        {
            other.GetComponent<Manager>().AddKey(m_key);
            m_audioSource.PlayOneShot(m_audioClip);
            Destroy(GetComponentInChildren<MeshRenderer>());
            DestroyAfterPickup();
        }
        else
        {
            Debug.Log("like i said, you already have this key");
            return;
        }
    }

    private void DestroyAfterPickup()
    {
        //Destroy key after the sfx plays
        Destroy(this.gameObject, m_audioClip.length);
    }

}

public enum Colorkeys
{
    Red,
    Green,
    Blue,
    Black
}