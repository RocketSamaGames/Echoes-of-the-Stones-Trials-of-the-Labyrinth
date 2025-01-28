using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeBehaviour : MonoBehaviour
{
    [SerializeField] private Transform blade;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip clip1;
    [SerializeField] private AudioClip clip2;
    private bool rotatingToLeft = true;
    private float previousRotation;

    private void Start()
    {
        previousRotation = blade.transform.rotation.eulerAngles.z;
    }

    private void Update()
    {
        if (!audioSource.isPlaying)
        {
            if (rotatingToLeft)
            {
                audioSource.clip = clip1;
            }
            else
            {
                audioSource.clip = clip2;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            float currentRotation = blade.transform.rotation.eulerAngles.z;

            if (currentRotation > 180) currentRotation -= 360;

            if (currentRotation <= 30f  && currentRotation >= -30f)
            {
                if (currentRotation > previousRotation && !rotatingToLeft && !audioSource.isPlaying)
                {
                    rotatingToLeft = true;
                    if(!audioSource.isPlaying)
                    {
                        audioSource.Play();
                    }
                }
                else if (currentRotation < previousRotation && rotatingToLeft && !audioSource.isPlaying)
                {
                    rotatingToLeft = false;
                    if (!audioSource.isPlaying)
                    {
                        audioSource.Play();
                    }
                }
            }
            previousRotation = currentRotation;
        }   
    }
}
