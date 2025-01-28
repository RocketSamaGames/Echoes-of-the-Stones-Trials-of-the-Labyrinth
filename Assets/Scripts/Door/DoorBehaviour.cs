using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class DoorBehaviour : MonoBehaviour
{
    [SerializeField] private GameManagerSO gM;
    [SerializeField] private Transform destinationPoint;
    [SerializeField] private int idDoor;
    [SerializeField] private float speed;

    private bool openDoor;

    private void Start()
    {
        gM.OnButtonActivated += Open;
    }

    private void Update()
    {
        if (openDoor)
        {
            transform.position = Vector3.MoveTowards(transform.position, destinationPoint.position, speed * Time.deltaTime);
        }
    }

    private void Open(int idDoorButton)
    {
        if (idDoorButton == idDoor)
        {
            openDoor = true;
            gM.OnButtonActivated -= Open;
        }
    }
}
