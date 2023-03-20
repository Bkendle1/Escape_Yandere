using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    private AudioSource m_audioSource;
    [SerializeField] private AudioClip m_audioClip;

    private void Awake()
    {
        m_audioSource = GetComponent<AudioSource>();
    }
    
    public void StepSFX()
    {
        m_audioSource.Stop(); //we don't want one sound to wait for another to play
        m_audioSource.PlayOneShot(m_audioClip);
        m_audioSource.pitch = Random.Range(.8f, 1.2f); ;
    }

}
