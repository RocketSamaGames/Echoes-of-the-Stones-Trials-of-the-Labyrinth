using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script que contiene el comportamiento de los botones que accionan las puertas.
public class DoorButton : MonoBehaviour
{
    [SerializeField] private GameManagerSO gM;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera secondaryCamera;
    [SerializeField] private AudioSource buttonSound;
    [SerializeField] private AudioSource doorSound;
    [SerializeField] private Transform destinationPoint;
    [SerializeField] private int idDoorButton;
    [SerializeField] private float speed;
    
    private bool doorActivated;
    private bool buttonActivated;
    private bool isCameraSwitched;
    private float lookDuration = 2.5f;
    private bool alreadyActivated;
    private float cameraSwitchEndTime = 0f;

    private void Start()
    {
        secondaryCamera.gameObject.SetActive(false);
        mainCamera.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (buttonActivated)
        {
            transform.position = Vector3.MoveTowards(transform.position, destinationPoint.position, speed * Time.deltaTime);
        }

        if (doorActivated && !isCameraSwitched)
        {
            mainCamera.gameObject.SetActive(false);
            secondaryCamera.gameObject.SetActive(true);

            cameraSwitchEndTime = Time.time + lookDuration;
            isCameraSwitched = true;
        }

        if (isCameraSwitched && Time.time >= cameraSwitchEndTime)
        {
            secondaryCamera.gameObject.SetActive(false);
            mainCamera.gameObject.SetActive(true);
            isCameraSwitched = false;
            doorActivated = false;
        }
    }

    public void Interact(PlayerMovement player)
    {
        if (alreadyActivated) return;
        alreadyActivated = true;

        player.SetFrozen(true);

        StartCoroutine(PlayingSounds(player));
    }

    public bool IsAlreadyActivated()
    {
        return alreadyActivated;
    }

    private IEnumerator PlayingSounds(PlayerMovement player)
    {
        player.SetFrozen(true);
        buttonActivated = true;
        if (!buttonSound.isPlaying)
        {
            buttonSound.Play();
        }
        
        yield return new WaitForSeconds(buttonSound.clip.length);

        doorActivated = true;
        gM.InteractiveObject(idDoorButton);
        if (!doorSound.isPlaying)
        {
            doorSound.Play();
        }
        
        yield return new WaitForSeconds(doorSound.clip.length);
        player.SetFrozen(false);
    }
}
