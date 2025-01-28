using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Script que contiene el comportamiento de los enemigos, así como sus animaciones, sonidos y rutinas.
public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] private float initialLife;
    [SerializeField] private Animator enemyAnimator;
    [SerializeField] private PlayerMovement target;
    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private SkinnedMeshRenderer slimeRenderer;

    private string walkingAnim = "IsWalking";
    private string idleAnim = "IsIdle";
    private string runningAnim = "IsRunning";
    private string attackAnim = "IsAttacking";
    private string hurtAnim = "IsHurt";
    private string deathAnim = "IsDead";
    private int routine;
    private float cronometer;
    private Quaternion angle;
    private float grade;
    private bool attacking;
    private float damageCooldown = 2f;
    private bool canTakeDamage;
    private float currentLife;
    private bool hasAttacked;
    private bool isStunned;
    private bool chasingPlayer;
    private bool isDead;
    private bool isPlayerNearby;

    private void Start()
    {
        canTakeDamage = true;
        hasAttacked = false;
        isStunned = false;
        chasingPlayer = false;
        isPlayerNearby = false;
        currentLife = initialLife;
    }

    private void Update()
    {
        Behaviour();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (target.canTakeDamage == false) return;
        if (collision.gameObject.CompareTag("Player"))
        {
            target.ApplyPushBack(collision);
            target.ApplyDamage(50);
        }

        if (!canTakeDamage) return;

        if (collision.gameObject.CompareTag("PlayerSword") && !isStunned)
        {
            SoundManager.Instance.StopSlimeRun();
            canTakeDamage = false;
            isStunned = true;
            enemyAnimator.SetBool(runningAnim, false);
            enemyAnimator.SetBool(attackAnim, false);
            enemyAnimator.SetBool(hurtAnim, true);
            ApplyDamage(50);

            StartCoroutine(StunEnemy());
        }
    }

    private void Behaviour()
    {
        if (isStunned) return;
        if (Vector3.Distance(transform.position, target.transform.position) > 8)
        {
            chasingPlayer = false;
            cronometer += 1 * Time.deltaTime;
            if (cronometer >= 4)
            {
                routine = UnityEngine.Random.Range(0, 2);
                cronometer = 0;
            }
            switch (routine)
            {
                case 0:

                    enemyAnimator.SetBool(walkingAnim, false);
                    enemyAnimator.SetBool(runningAnim, false);
                    enemyAnimator.SetBool(idleAnim, true);
                    if (isPlayerNearby && !isDead)
                    {
                        SoundManager.Instance.StopSlimeWalk();
                        SoundManager.Instance.StopSlimeRun();
                    }
                    break;

                case 1:
                    grade = UnityEngine.Random.Range(0, 360);
                    angle = Quaternion.Euler(0, grade, 0);
                    routine++;
                    if (isPlayerNearby && !isDead)
                    {
                        SoundManager.Instance.PlaySlimeWalk();
                        SoundManager.Instance.StopSlimeRun();
                    }
                    break;

                case 2:
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, angle, 0.5f);
                    transform.Translate(Vector3.forward * 1 * Time.deltaTime);
                    enemyAnimator.SetBool(idleAnim, false);
                    enemyAnimator.SetBool(runningAnim, false);
                    enemyAnimator.SetBool(walkingAnim, true);
                    if (isPlayerNearby && !isDead)
                    {
                        SoundManager.Instance.StopSlimeRun();
                    }
                    break;
            }
        }
        else
        {
            if (currentLife <= 0)
            {
                if (isPlayerNearby && !isDead)
                {
                    SoundManager.Instance.StopSlimeRun();
                    SoundManager.Instance.StopSlimeWalk();
                }
                return;
            }

            if (Vector3.Distance(transform.position, target.transform.position) > 2 && !attacking)
            {
                if (!chasingPlayer && !isDead)
                {
                    if (isPlayerNearby)
                    {
                        SoundManager.Instance.PlaySlimeRun();
                        SoundManager.Instance.StopSlimeWalk();
                    }
                }

                chasingPlayer = true;

                var lookPos = target.transform.position - transform.position;
                lookPos.y = 0;
                var rotation = Quaternion.LookRotation(lookPos);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 2);
                enemyAnimator.SetBool(walkingAnim, false);
                enemyAnimator.SetBool(idleAnim, false);
                enemyAnimator.SetBool(runningAnim, true);
                transform.Translate(Vector3.forward * 2 * Time.deltaTime);

                enemyAnimator.SetBool(attackAnim, false);
                hasAttacked = false;
            }
            else
            {
                if (chasingPlayer)
                {
                    if (isPlayerNearby)
                    {
                        SoundManager.Instance.StopSlimeRun();
                    }
                }

                chasingPlayer = false;
                enemyAnimator.SetBool(walkingAnim, false);
                enemyAnimator.SetBool(runningAnim, false);
                enemyAnimator.SetBool(attackAnim, true);
                attacking = true;

                if (!hasAttacked)
                {
                    if (isPlayerNearby)
                    {
                        SoundManager.Instance.PlaySlimeHit();
                    }
                    hasAttacked = true;
                }
            }
        }
    }

    private void FinalAnim()
    {
        enemyAnimator.SetBool(attackAnim, false);
        attacking = false;
    }

    public void ApplyDamage(float damage)
    {
        currentLife -= damage;
        if (currentLife > 0)
        {
            SoundManager.Instance.PlaySlimeHurt();
        }
        else
        {
            CheckCurrentLife();
        }
        canTakeDamage = false;
        StartCoroutine(DamageCooldown());
    }

    private void CheckCurrentLife()
    {
        if (currentLife <= 0)
        {
            GetComponent<CapsuleCollider>().enabled = false;
            isDead = true;
            SoundManager.Instance.PlaySlimeDeath();
            enemyAnimator.SetBool(attackAnim, false);
            enemyAnimator.SetBool(runningAnim, false);
            enemyAnimator.SetBool(hurtAnim, false);
            enemyAnimator.SetBool(deathAnim, true);
            SoundManager.Instance.StopSlimeRun();
            SoundManager.Instance.StopSlimeWalk();
            Debug.Log("Dead");

            StartCoroutine(FadeAndDestroy());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerNearby = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerNearby = false;
            SoundManager.Instance.StopSlimeRun();
            SoundManager.Instance.StopSlimeWalk();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && isDead)
        {
            isPlayerNearby = false;
            SoundManager.Instance.StopSlimeRun();
            SoundManager.Instance.StopSlimeWalk();
        }
    }

    private IEnumerator DamageCooldown()
    {
        yield return new WaitForSeconds(damageCooldown);
        canTakeDamage = true;
    }

    private IEnumerator StunEnemy()
    {
        yield return new WaitForSeconds(0.5f);

        enemyAnimator.SetBool(hurtAnim, false);
        if (Vector3.Distance(transform.position, target.transform.position) > 2 && chasingPlayer)
        {
            enemyAnimator.SetBool(runningAnim, true);
            SoundManager.Instance.PlaySlimeRun();
        }
        else
        {
            SoundManager.Instance.StopSlimeRun();
            enemyAnimator.SetBool(idleAnim, true);
        }

        isStunned = false;
        attacking = false;
        canTakeDamage = true;
    }

    private IEnumerator RestartAnim()
    {
        yield return new WaitForSeconds(2f);

        enemyAnimator.SetBool(hurtAnim, false);
        isStunned = false;
        canTakeDamage = true;
        SoundManager.Instance.PlaySlimeRun();
    }

    private IEnumerator FadeAndDestroy()
    {
        yield return new WaitForSeconds(0.35f);

        SoundManager.Instance.PlayEnemyDisappear();

        yield return new WaitForSeconds(1f);

        float fadeDuration = 1f;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            float scale = Mathf.Lerp(1, 0, elapsedTime /  fadeDuration);
            transform.localScale = Vector3.one * scale;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = Vector3.zero;
        

        yield return new WaitForSeconds(1f);

        Destroy(gameObject);
    }
}
