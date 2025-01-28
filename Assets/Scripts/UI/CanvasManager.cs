using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Script que contiene las propiedades del texto de la puntuación y de la barra de vida.
public class CanvasManager : MonoBehaviour
{
    [SerializeField] private TMP_Text collectibleText;
    [SerializeField] private Image healthBar;

    public TMP_Text CollectibleText { get => collectibleText; }
    public Image HealthBar { get => healthBar; }
}
