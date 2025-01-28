using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Daggers : MonoBehaviour
{
    [SerializeField] private GameManagerSO gM;
    [SerializeField] private int idDaggers;
    [SerializeField] private float speed;
    [SerializeField] private Vector3 direction;
    [SerializeField] private AudioSource audioSource;

    private bool trapIsActivated = false;

    private void Start()
    {
        gM.OnButtonActivated += ActivateDaggers;
    }

    private void Update()
    {
        if (trapIsActivated)
        {
            transform.Translate(direction * speed * Time.deltaTime, Space.World);
        }
    }

    private void ActivateDaggers(int idButtonActivated)
    {
        if (idButtonActivated == idDaggers)
        {
            audioSource.Play();
            trapIsActivated = true;
            gM.OnButtonActivated -= ActivateDaggers;
        }
    }
}
