using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Script que contiene el funcionamiento de la pantalla de título (funciones de los botones, efectos de sonido y cierre del programa).
public class TittleScreenBehaviour : MonoBehaviour
{
    public Animator canvasAnimator;
    public GameObject panel;

    private float delayLoad = 3f;
    private float delayDisablePanel = 1.8f;

    private void Start()
    {
        SoundManager.Instance.StopLowHealthSound();
        SoundManager.Instance.StopSlimeRun();
        SoundManager.Instance.StopSlimeWalk();

        Cursor.visible = true; 
        Cursor.lockState = CursorLockMode.None;
        
        StartCoroutine(DisablePanel());
    }

    // Función del botón para iniciar el juego.
    public void StartGame()
    {
        panel.SetActive(true);
        canvasAnimator.Play("FadeOut");
        
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
        yield return new WaitForSeconds(delayDisablePanel); 
        panel.SetActive(false); 
    }
}
