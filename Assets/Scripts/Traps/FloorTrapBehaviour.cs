using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script que contiene el comportamiento de la trampa de suelo.
public class FloorTrapBehaviour : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private PlayerMovement player;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }

            if (player.isDead)
            {
                audioSource.Stop();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
             audioSource.Stop();
        }
    }
}
