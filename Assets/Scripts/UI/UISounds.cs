using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Este script contiene el funcionamiento de los efectos de sonido de la pantalla de título.
public class UISounds : MonoBehaviour
{
    public AudioSource sounds;
    public AudioClip clickAudio;
    public AudioClip switchAudio;

    // Función para el sonido al pulsar el botón.
    public void ClickAudioOn()
    {
        sounds.PlayOneShot(clickAudio);
    }

    // Función para el sonido al pasar por encima del botón.
    public void SwitchAudioOn()
    {
        sounds.PlayOneShot(switchAudio);
    }
}
