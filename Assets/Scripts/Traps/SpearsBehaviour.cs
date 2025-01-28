using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// Script que contiene el comportamiento de movimiento de la trampa de lanzas.
public class SpearsBehaviour : MonoBehaviour
{
    [SerializeField] private float waitingTimeGoingUp;
    [SerializeField] private float waitingTimeGoingDown;
    [SerializeField] private float goingUpSpeed;
    [SerializeField] private float goingDownSpeed;
    [SerializeField] private Transform upPoint;
    [SerializeField] private Transform downPoint;
    [SerializeField] private AudioSource spearsAudioSource;
    [SerializeField] private AudioClip goingUpClip;
    [SerializeField] private AudioClip goingDownClip;

    private bool hasPlayedSound = false;
    private bool previousMovingDown = false;
    private Vector3 targetPosition;
    private bool movingDown;
    private bool isWaiting;

    private void Start()
    {
        targetPosition = downPoint.position;
    }

    private void Update()
    {
        if (!isWaiting)
        {
            float step = (movingDown ? goingDownSpeed : goingUpSpeed) * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

            if (Vector3.Distance(transform.position, targetPosition) < 0.001f)
            {
                if (movingDown)
                {
                    StartCoroutine(WaitBeforMovingUp());
                }
                else
                {
                    StartCoroutine(WaitBeforMovingDown());
                }
            }
        }
    }

    public void SwitchAndPlaySound()
    {
        if (movingDown != previousMovingDown)
        {
            hasPlayedSound = false;
            previousMovingDown = movingDown;
        }

        if (movingDown)
        {
            spearsAudioSource.clip = goingDownClip;

            if (!hasPlayedSound)
            {
                spearsAudioSource.Play();
                hasPlayedSound = true;
            }
        }
        else
        {
            spearsAudioSource.clip = goingUpClip;

            if (!hasPlayedSound)
            {
                spearsAudioSource.Play();
                hasPlayedSound = true;
            }
        }
    }

    private IEnumerator WaitBeforMovingUp()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitingTimeGoingUp);
        movingDown = false;
        targetPosition = upPoint.position;
        isWaiting = false;
    }

    private IEnumerator WaitBeforMovingDown()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitingTimeGoingDown);
        movingDown = true;
        targetPosition = downPoint.position;
        isWaiting = false;
    }
}
