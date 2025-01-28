using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearsSound : MonoBehaviour
{
    [SerializeField] private SpearsBehaviour spears;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            spears.SwitchAndPlaySound();
        }
    }
}
