using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Este script contiene el funcionamiento del cambio de música al salir y entrar del laberinto.
public class MusicSwitcher : MonoBehaviour
{
    // Variables para asignar los sonidos "musicales" en Unity.
    public AudioSource InsideMusic; // Audio que suena en el interior del laberinto.
    public AudioSource OutsideMusic; // Audio que suena en el exterior del laberinto.

    // Cuando el player atraviesa la puerta de salida del laberinto la música cambia.
    private void OnTriggerEnter(Collider other)
    {
        // Se comprueba si la etiqueta del GameObject que colisiona con el Box Collider es el player.
        if (other.CompareTag("Player"))
        {
            // Si está sonando la música del interior, esta se detiene y empieza a sonar la del exterior.
            if (InsideMusic.isPlaying)
            {
                InsideMusic.Stop();
                OutsideMusic.Play();
            }
            // Si está sonando la música del exterior, esta se detiene y empieza a sonar la del interior.
            else
            {
                InsideMusic.Play();
                OutsideMusic.Stop();
            }
        }
        
    }
}
