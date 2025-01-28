using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] private TMP_Text collectibleText;
    [SerializeField] private Image healthBar;

    public TMP_Text CollectibleText { get => collectibleText; }
    public Image HealthBar { get => healthBar; }
}
