using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Intenté implementar alguna forma de cambiar el efecto de sonido de los pasos cuando camina sobre diferentes superficies, pero no di con
// la clave (ya que se trata de un terrain y las superficies son texturas pintadas en el suelo), así que decidí investigarlo más adelante.
public class Footstep : MonoBehaviour
{
    private enum TerrainTags
    {
        Grass,
        Ground
    }

    [SerializeField] private AudioSource[] footStepAudios;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
