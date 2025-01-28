using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

// Este script contiene el funcionamiento del movimiento del player (movimiento y gravedad), así como sus efectos de sonido.
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private CanvasManager canvas;
    [SerializeField] private TMP_Text pressText;
    [SerializeField] private Animator canvasFadePanel;
    [SerializeField] private Animator canvasHitPanel;
    [SerializeField] private Animator canvasLowHealthPanel;
    [SerializeField] private Animator canvasStarAchievement;
    [SerializeField] private Transform sword;
    [SerializeField] private Animator swordAnim;

    // Variables que se asignan desde Unity
    public CharacterController characterController;
    public Transform groundCheck; // Para asignar el elemento que detecta el suelo.
    public LayerMask groundLayer; // Para asignar qué elemento es el suelo.
    public AudioSource steps; // Sonido de los pasos.
    public bool isDead;

    [SerializeField] private float speed = 10f; // Velocidad de movimiento del player.
    [SerializeField] private float sphereRadius = 0.3f; // Para la detección del suelo.
    [SerializeField] private float distanceDetection;
    [SerializeField] private float initialLife;
    [SerializeField] private Vector3 initialPosition;
    [SerializeField] private float pushForce;

    private Vector3 pushDirection = Vector3.zero;
    private float pushDecay = 5f;

    private float gravity = -9.81f;
    private Vector3 velocity;
    private bool isGrounded;

    private float damageCooldown = 2f;
    public bool canTakeDamage = true;
    private bool isLowHealthPanelPlaying = false;

    private bool isFrozen;

    private int score = 0;
    private bool starAchievementUnlocked = false;
    private bool isAchievementPlaying = false;
    private float currentLife;

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = -1;

        transform.position = initialPosition;

        currentLife = initialLife;

        canTakeDamage = true;

        isFrozen = false;

        pressText.enabled = false;

        canvasHitPanel.gameObject.SetActive(false);
        canvasLowHealthPanel.gameObject.SetActive(false);
    }

    void Update()
    {
        if (isFrozen)
        {
            steps.Pause();
            return;
        }

        // Determinamos cuándo el player está en el suelo.
        isGrounded = Physics.CheckSphere(groundCheck.position, sphereRadius, groundLayer);

        velocity.y = isGrounded && velocity.y < 0 ? -2f : velocity.y + gravity * Time.deltaTime;

        Vector3 moveInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        if (moveInput.sqrMagnitude > 1f) moveInput.Normalize();

        pushDirection = Vector3.Lerp(pushDirection, Vector3.zero, pushDecay * Time.deltaTime);

        Vector3 finalMove = (transform.TransformDirection(moveInput) * speed + pushDirection) * Time.deltaTime;
        finalMove.y += velocity.y * Time.deltaTime;

        characterController.Move(finalMove);

        if (isGrounded && moveInput.sqrMagnitude > 0)
        {
            if (!steps.isPlaying) steps.Play();
        }
        else steps.Stop();

        SwordAtack();
        DetectButtonPress();
        CheckCurrentLife();
        CheckScore();
    }

    public void SetFrozen(bool value)
    {
        isFrozen = value;

        if (isFrozen)
        {
            velocity = Vector3.zero;
            steps.Pause();
        }
    }

    private void SwordAtack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            swordAnim.SetTrigger("Swing");
            SoundManager.Instance.PlaySwordHit();
        }
    }

    private void DetectButtonPress()
    {
        Debug.DrawRay(transform.position, transform.forward * distanceDetection, Color.red);

        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, distanceDetection))
        {
            if (hit.transform.TryGetComponent(out DoorButton doorButton))
            {
                bool isActivated = doorButton.IsAlreadyActivated();
                pressText.enabled = !isActivated;

                if (!isActivated && Input.GetKeyDown(KeyCode.E))
                {
                    doorButton.Interact(this);
                    pressText.enabled = false;
                }
            }
        }
        else
        {
            pressText.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var tag = other.gameObject.tag;
        if (tag == "Star")
        {
            HandleStarPickup(other);
        }
        else if (tag == "Heart")
        {
            HandleHeartPickup(other);
        }
        else if (tag == "CheckPoint")
        {
            Debug.Log("Checkpoint reached");
            SaveCheckpointPosition();
        }
        else if (tag == "CamouflageCover")
        {
            SoundManager.Instance.PlayCamouflageDisappear();
            Destroy(other.gameObject);
        }
        else if (canTakeDamage)
        {
            if (tag == "Dagger")
            {
                ApplyDamage(25);
            }

            if (tag == "SpikeTrap")
            {
                HandleSpikeTrap();
            }
        }
    }

    private void HandleStarPickup(Collider other)
    {
        score++;
        DisableAndDestroy(other, SoundManager.Instance.PlayStarSound);
        canvas.CollectibleText.text = $"{score}/7";
    }

    private void HandleHeartPickup(Collider other)
    {
        if (currentLife < 100)
        {
            currentLife = Mathf.Min(currentLife + 50, 100);
            DisableAndDestroy(other, SoundManager.Instance.PlayHeartSound);
            canvas.HealthBar.fillAmount = currentLife / initialLife;
        }
    }

    private void HandleSpikeTrap()
    {
        SoundManager.Instance.PlaySpikeSound();
        currentLife = 0;
        canvas.HealthBar.fillAmount = currentLife / initialLife;
    }

    private void DisableAndDestroy(Collider other, Action playSound)
    {
        other.GetComponent<Collider>().enabled = false;
        playSound();
        StartCoroutine(CollectObject(other.gameObject));
    }

    // Empujar al jugador al recibir daño con algunos objetos
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (canTakeDamage && hit.gameObject.CompareTag("DangerousTrap"))
        {
            ApplyPushBack(hit);
            ApplyDamage(25);
        }
    }

    // Almacenar la posición a modo de checkpoint
    private void SaveCheckpointPosition()
    {
        Vector3 position = transform.position;
        Quaternion rotation = transform.rotation;

        PlayerPrefs.SetFloat("CheckpointPositionX", position.x);
        PlayerPrefs.SetFloat("CheckpointPositionY", position.y);
        PlayerPrefs.SetFloat("CheckpointPositionZ", position.z);
        PlayerPrefs.SetFloat("CheckpointRotationY", rotation.eulerAngles.y);
        PlayerPrefs.Save();

        Debug.Log($"Saved checkpoint position: {position.x}, {position.y}, {position.z}");
    }

    // Aplicar daño
    public void ApplyDamage(float damage)
    {
        currentLife -= damage;
        if (currentLife > 0)
        {
            SoundManager.Instance.PlayPlayerHurt();
            StartCoroutine(ShowHitPanel());
        }
        canvas.HealthBar.fillAmount = currentLife / initialLife;
        canTakeDamage = false;
        StartCoroutine(DamageCooldown());
    }

    public void ApplyPushBack(ControllerColliderHit hit)
    {
        Vector3 direction = (transform.position - hit.collider.bounds.center).normalized;
        direction.y = 0;
        pushDirection += direction * pushForce;
    }

    public void ApplyPushBack(Collision collision)
    {
        Vector3 direction = (transform.position - collision.collider.bounds.center).normalized;
        direction.y = 0;
        pushDirection += direction * pushForce;
    }

    // Comprobar la vida actual
    private void CheckCurrentLife()
    {
        if (currentLife <= 0)
        {
            HandlePlayerDeath();
        }
        else if (currentLife == 25 && !isLowHealthPanelPlaying)
        {
            StartCoroutine(ShowLowHealthPanel());
        }
    }

    private void CheckScore()
    {
        if (score == 7 && !starAchievementUnlocked)
        {
            StartCoroutine(WaitForAchievement());
        }
    }

    private void HandlePlayerDeath()
    {
        Debug.Log("You're dead");
        isDead = true;
        SoundManager.Instance.PlayPlayerDead();
        canvasFadePanel.Play("FadeOut");
        StartCoroutine(RespawnOnCheckpoint());
    }

    // Cargar el último checkpoint tocado
    private void LoadCheckpointPosition()
    {
        Vector3 checkpointPosition = initialPosition;
        Quaternion checkpointRotation = Quaternion.identity;

        if (PlayerPrefs.HasKey("CheckpointPositionX"))
        {
            checkpointPosition = new Vector3
            (
                PlayerPrefs.GetFloat("CheckpointPositionX"),
                PlayerPrefs.GetFloat("CheckpointPositionY"),
                PlayerPrefs.GetFloat("CheckpointPositionZ")
            );

            checkpointRotation = Quaternion.Euler(0, PlayerPrefs.GetFloat("CheckpointRotationY"), 0);
        }
        characterController.enabled = false;
        transform.position = checkpointPosition;
        transform.rotation = checkpointRotation;
        characterController.enabled = true;

        canvasFadePanel.Play("FadeIn");
        Debug.Log($"Respawned");

        currentLife = 25;
        canvas.HealthBar.fillAmount = currentLife / initialLife;
    }

    // Rutina para los items
    private IEnumerator CollectObject(GameObject collectible)
    {
        Animator animator = collectible.GetComponent<Animator>();

        animator?.SetTrigger("IsObtained");

        yield return new WaitForSeconds(1);

        Destroy(collectible);
    }

    private IEnumerator WaitForAchievement()
    {
        if (!isAchievementPlaying)
        {
            isAchievementPlaying = true;
            yield return new WaitForSeconds(1.5f);

            canvasStarAchievement.SetBool("IsUnlocked", true);
            starAchievementUnlocked = true;
            SoundManager.Instance.PlayAchievementUnlocked();


            yield return new WaitForSeconds(7f);

            canvasStarAchievement.SetBool("IsUnlocked", false);
            isAchievementPlaying = false;
        }
    }

    // Cooldown para esperar un tiempo y poder volver a recibir daño
    private IEnumerator DamageCooldown()
    {
        yield return new WaitForSeconds(damageCooldown);
        canTakeDamage = true;
    }

    // Rutina para el respawn
    private IEnumerator RespawnOnCheckpoint()
    {
        Debug.Log("Checkpoint position: " + PlayerPrefs.GetFloat("CheckpointPositionX") + ", " + PlayerPrefs.GetFloat("CheckpointPositionY") + ", " + PlayerPrefs.GetFloat("CheckpointPositionZ"));
        isFrozen = true;
        pushDirection = Vector3.zero;

        yield return new WaitForSeconds(4f);

        LoadCheckpointPosition();

        Debug.Log("Player position after respawn: " + transform.position);

        isFrozen = false;
        isDead = false;
    }

    private IEnumerator ShowHitPanel()
    {
        canvasHitPanel.gameObject.SetActive(true);
        canvasHitPanel.Play("HitPanelAnim");

        yield return new WaitForSeconds(0.75f);

        canvasHitPanel.gameObject.SetActive(false);
    }

    private IEnumerator ShowLowHealthPanel()
    {
        isLowHealthPanelPlaying = true;
        canvasLowHealthPanel.gameObject.SetActive(true);

        SoundManager.Instance.PlayLowHealthSound();

        while (currentLife == 25)
        {
            canvasLowHealthPanel.Play("LowHealthPanel");
            yield return new WaitForSeconds(canvasLowHealthPanel.GetCurrentAnimatorStateInfo(0).length);
        }

        canvasLowHealthPanel.gameObject.SetActive(false);
        isLowHealthPanelPlaying = false;

        SoundManager.Instance.StopLowHealthSound();
    }

    private IEnumerator FadeOutWall(GameObject wall)
    {
        Renderer wallRenderer = wall.GetComponent<Renderer>();
        Color initialColor = wallRenderer.material.color;
        float fadeDuration = 2f;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            wallRenderer.material.color = new Color(initialColor.r, initialColor.g, initialColor.b, alpha);
            yield return null;
        }
        wall.SetActive(false);
    }
}
