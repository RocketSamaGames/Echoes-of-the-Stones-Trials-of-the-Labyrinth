using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Este script contiene el funcionamiento de la pantalla de título (funciones de los botones, efectos de sonido y cierre del programa).
public class TittleScreenBehaviour : MonoBehaviour
{
    // Variables que se asignan en Unity.
    public Animator canvasAnimator;
    public GameObject panel;

    private float delayLoad = 3f; // Retardo para cargar una escena.
    private float delayDisablePanel = 1.8f; // Retardo para desactivar el panel.

    private void Start()
    {
        SoundManager.Instance.StopLowHealthSound();
        SoundManager.Instance.StopSlimeRun();
        SoundManager.Instance.StopSlimeWalk();

        // Al iniciar esta escena hacemos visible y desbloqueamos el ratón.
        Cursor.visible = true; 
        Cursor.lockState = CursorLockMode.None;
        // Iniciamos la coroutine para desactivar el panel, ya que, de lo contrario, no podríamos pulsar los botones (el panel está superpuesto).
        StartCoroutine(DisablePanel());
    }

    // Función del botón para iniciar el juego.
    public void StartGame()
    {
        // Volvemos a activar el GameObject panel para hacer un Fade Out.
        panel.SetActive(true);
        // Ejecutamos la animación.
        canvasAnimator.Play("FadeOut");
        // Iniciamos la coroutine para cargar el juego
        StartCoroutine(LoadGame());
    }

    // Función del botón para cerrar el juego.
    public void ExitGame()
    {
        Application.Quit();
    }

    private IEnumerator LoadGame()
    {
        yield return new WaitForSeconds(delayLoad);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private IEnumerator DisablePanel()
    {
        yield return new WaitForSeconds(delayDisablePanel); // Asignamos el tiempo.
        panel.SetActive(false); // Desactivamos el panel para poder acceder a los botones.
    }
}
