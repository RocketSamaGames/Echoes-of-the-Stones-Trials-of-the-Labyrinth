using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// Script que contiene el funcionamiento de la cámara del player.
public class CameraMovement : MonoBehaviour
{
    public Vector2 sensitivity;

    [SerializeField] private Transform gameCamera;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity.x * Time.deltaTime;

        float mouseY = Input.GetAxis("Mouse Y") * sensitivity.y * Time.deltaTime;

        if (mouseX != 0)
        {
            transform.Rotate(Vector3.up * mouseX);
        }

        if (mouseY != 0)
        {
            float angle = (gameCamera.localEulerAngles.x - mouseY + 360) % 360;
            if (angle > 180)
            {
                angle -= 360;
            }

            angle = Mathf.Clamp(angle, -90, 80);

            gameCamera.localEulerAngles = Vector3.right * angle;
        }
    }
}


