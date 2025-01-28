using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script que contiene el comportamiento de los checpoints para cambiar de color cuando el jugador pasa por su BoxCollider y reproducir su sonido.
public class CheckpointBehaviour : MonoBehaviour
{
    [SerializeField] private Material yellowMaterial;

    public bool isActivated;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Renderer renderer = GetComponent<Renderer>();
            Material[] currentMaterials = renderer.materials;

            currentMaterials[1] = yellowMaterial;

            renderer.materials = currentMaterials;

            if (isActivated == false)
            {
                SoundManager.Instance.PlayCheckpointSound();
                isActivated = true;
            }
        }
    }
}
