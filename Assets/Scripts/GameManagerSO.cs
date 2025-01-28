using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameManager")]
public class GameManagerSO : ScriptableObject
{
    public event Action<int> OnButtonActivated;
    public void InteractiveObject(int idButton)
    {
        // Lanzar evento de botón activado.
        OnButtonActivated?.Invoke(idButton);
    }
}
