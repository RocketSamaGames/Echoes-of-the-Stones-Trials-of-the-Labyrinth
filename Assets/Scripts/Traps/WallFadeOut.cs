using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallFadeOut : MonoBehaviour
{
    [SerializeField] private AudioSource wallAudio;
    public Material rockMaterial;
    public float fadeDuration = 1.5f;
    private bool isFading = false;

    private void Start()
    {
        rockMaterial = new Material(GetComponent<Renderer>().material);
        GetComponent<Renderer>().material = rockMaterial;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isFading)
        {
            StartCoroutine(WallFadingOut());
        }
    }

    private IEnumerator WallFadingOut()
    {
        isFading = true;
        Color originalColor = rockMaterial.color;
        float elapsedTime = 0;
        wallAudio.Play();

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, elapsedTime / fadeDuration);
            rockMaterial.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        rockMaterial.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0);
        gameObject.SetActive(false);
    }
}
