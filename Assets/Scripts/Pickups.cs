using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Pickups : MonoBehaviour
{
    [SerializeField] private int pickupValue = 1;
    [SerializeField] private AudioClip _audioClip;
    private AudioSource _audioSource;
    private MeshRenderer[] _meshRenders;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        //store all the mesh renderers from the children
        _meshRenders = GetComponentsInChildren<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject.GetComponent<BoxCollider>());
            //update score
            GameManager.Instance.UpdateScore(pickupValue);
            //play pickup sfx
            _audioSource.PlayOneShot(_audioClip);
            //destroy all meshes to make it invisible
            foreach (MeshRenderer mesh in _meshRenders)
            {
                Destroy(mesh);
            }
            DestroyAfterPickup();
        }
    }

    private void DestroyAfterPickup()
    {
        //destroy the entire gameObject after the sfx finishes
        Destroy(this.gameObject, _audioClip.length);
    }
}
