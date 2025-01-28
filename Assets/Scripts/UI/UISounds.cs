using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Este script contiene el funcionamiento de los efectos de sonido de la pantalla de título.
public class UISounds : MonoBehaviour
{
    // Variables que asignamos en Unity.
    public AudioSource sounds;
    public AudioClip clickAudio;
    public AudioClip switchAudio;

    // Función para el sonido al pulsar el botón.
    public void ClickAudioOn()
    {
        // Se reproduce el sonido una vez.
        sounds.PlayOneShot(clickAudio);
    }

    // Función para el sonido al pasar por encima del botón.
    public void SwitchAudioOn()
    {
        // Se reproduce el sonido una vez.
        sounds.PlayOneShot(switchAudio);
    }
}
