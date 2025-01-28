using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script que contiene el comportamiento del sonido de trampa de lanzas. Cuando el jugador atraviesa su BoxCollider, se puede escuchar su sonido.
public class SpearsSound : MonoBehaviour
{
    [SerializeField] private SpearsBehaviour spears;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            spears.SwitchAndPlaySound();
        }
    }
}
