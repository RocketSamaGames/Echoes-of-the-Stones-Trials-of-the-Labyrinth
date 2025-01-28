using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
