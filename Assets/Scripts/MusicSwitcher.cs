using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Este script contiene el funcionamiento del cambio de música al salir y entrar del laberinto.
public class MusicSwitcher : MonoBehaviour
{
    public AudioSource InsideMusic;
    public AudioSource OutsideMusic;

    // Cuando el player atraviesa la puerta de salida del laberinto la música cambia.
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (InsideMusic.isPlaying)
            {
                InsideMusic.Stop();
                OutsideMusic.Play();
            }
            else
            {
                InsideMusic.Play();
                OutsideMusic.Stop();
            }
        }
        
    }
}
