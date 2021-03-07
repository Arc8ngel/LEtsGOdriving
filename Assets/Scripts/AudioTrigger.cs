using KartGame.KartSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(AudioSource))]
public class AudioTrigger : MonoBehaviour
{
    Collider _collider;
    AudioSource _audioSource;

    // Start is called before the first frame update
    void Awake()
    {
        _collider = gameObject.GetComponent<Collider>();
        _audioSource = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collider)
    {
        if( collider.gameObject.GetComponent<ArcadeKart>() != null )
        {
            _audioSource?.Play();
        }
    }
}
