using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// Este script contiene el funcionamiento de la cámara del player.
public class CameraMovement : MonoBehaviour
{
    // Variable para la sensibilidad de la cámara. El valor se lo asignamos en Unity.
    public Vector2 sensitivity;

    // Con esto asignamos en Unity qué GameObject es la cámara.
    [SerializeField] private Transform gameCamera;

    private void Start()
    {
        // Al iniciar esta escena bloqueamos el ratón a la ventana Game.
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        // Variables para obtener el movimiento del ratón. Se multiplica por la sensibilidad y por Time.deltaTime.
        float mouseX = Input.GetAxis("Mouse X") * sensitivity.x * Time.deltaTime;

        float mouseY = Input.GetAxis("Mouse Y") * sensitivity.y * Time.deltaTime;

        // Rotación horizontal del player (cápsula).
        if (mouseX != 0)
        {
            transform.Rotate(Vector3.up * mouseX);
        }

        // Rotación vertical de la cámara con limitaciones.
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


