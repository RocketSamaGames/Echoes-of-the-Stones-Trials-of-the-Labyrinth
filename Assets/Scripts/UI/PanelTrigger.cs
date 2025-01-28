using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Este script contiene el funcionamiento del fundido a negro tras salir del laberinto, así como la función de cargar la pantalla de título.
public class PanelTrigger : MonoBehaviour
{
    // Variable para asignar el animator en Unity.
    public Animator canvasAnimator;

    // Variable para el retardo de cambio de escena.
    private float delayBeforeLoad = 5.0f;

    // Cuando el player atraviesa los muros invisibles se activa un efecto de fundido a negro en la pantalla.
    private void OnTriggerEnter(Collider other)
    {
        // Se comprueba si el GameObject que atraviesa el muro es el player.
        if (other.CompareTag("Player"))
        {
            Debug.Log("El jugador a tocado el panel"); // Esto lo puse para verificar el funcionamiento del fundido.
            canvasAnimator.Play("FadeOut"); // Se ejecuta la animación de fundido.
            StartCoroutine(LoadScreenTittle()); // Se inicia la coroutine para cargar la pantalla de título.
        }
    }

    private IEnumerator LoadScreenTittle()
    {
        yield return new WaitForSeconds(delayBeforeLoad); // Asignamos el tiempo.
        SceneManager.LoadScene("TittleScreen"); // Cargamos la pantalla de título.
    }
}
