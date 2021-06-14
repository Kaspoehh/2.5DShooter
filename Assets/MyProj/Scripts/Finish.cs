using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Finish : MonoBehaviour
{
    private AudioSource _audioSource;

    [SerializeField]private GameObject victoryCanvas;
    [SerializeField]private GameObject playerCanvas;

    [SerializeField] private GameObject cherryLeft;
    [SerializeField] private GameObject cherryMiddle;
    [SerializeField] private GameObject cherryRight;
    
    
    private void Awake()
    {
        _audioSource = this.GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Play");
            _audioSource.Play();
            other.GetComponent<PlayerMovement>().enabled = false;
            other.GetComponent<Animator>().SetBool("Walking", false);
            victoryCanvas.SetActive(true);
            playerCanvas.SetActive(false);
            
            
        }
    }
}
