using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script que contiene el funcionamiento de los botones que accionan las dagas.
public class Button : MonoBehaviour
{
    [SerializeField] private GameManagerSO gm;
    [SerializeField] private AudioSource slabSound;
    [SerializeField] private int idButton;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            gm.InteractiveObject(idButton);
            slabSound.Play();
        }
    }
}
