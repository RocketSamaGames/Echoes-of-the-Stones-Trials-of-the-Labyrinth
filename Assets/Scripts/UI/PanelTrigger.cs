using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Script que contiene el funcionamiento del fundido a negro tras salir del laberinto, así como la función de cargar la pantalla de título.
public class PanelTrigger : MonoBehaviour
{
    public Animator canvasAnimator;

    private float delayBeforeLoad = 5.0f;

    // Cuando el player atraviesa los muros invisibles se activa un efecto de fundido a negro en la pantalla.
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("El jugador a tocado el panel");
            canvasAnimator.Play("FadeOut");
            StartCoroutine(LoadScreenTittle());
        }
    }

    private IEnumerator LoadScreenTittle()
    {
        yield return new WaitForSeconds(delayBeforeLoad);
        SceneManager.LoadScene("TittleScreen");
    }
}
