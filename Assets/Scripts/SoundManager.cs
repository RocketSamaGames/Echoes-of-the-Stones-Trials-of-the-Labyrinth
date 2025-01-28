using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource starSound;
    public AudioSource heartSound;
    public AudioSource playerHurt;
    public AudioSource playerDeath;
    public AudioSource camouflageDisappear;
    public AudioSource spikeSound;
    public AudioSource checkpointSound;
    public AudioSource playerLowHealthSound;
    public AudioSource swordHit;
    public AudioSource slimeDeath;
    public AudioSource slimeHurt;
    public AudioSource slimeHit;
    public AudioSource slimeWalk;
    public AudioSource slimeRun;
    public AudioSource enemyDisappear;
    public AudioSource achievementUnlocked;

    public static SoundManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayStarSound()
    {
        if (starSound != null && starSound.clip != null)
        {
            starSound.Play();
        }
    }

    public void PlayHeartSound()
    {
        if (heartSound != null && heartSound.clip != null)
        {
            heartSound.Play();
        }
    }

    public void PlayPlayerHurt()
    {
        if (playerHurt != null && playerHurt.clip != null)
        {
            playerHurt.Play();
        }
    }

    public void PlayPlayerDead()
    {
        if (playerDeath != null && playerDeath.clip != null)
        {
            playerDeath.Play();
        }
    }

    public void PlayCamouflageDisappear()
    {
        if (camouflageDisappear != null && camouflageDisappear.clip != null)
        {
            camouflageDisappear.Play();
        }
    }

    public void PlaySpikeSound()
    {
        if (spikeSound != null && spikeSound.clip != null)
        {
            spikeSound.Play();
        }
    }

    public void PlayCheckpointSound()
    {
        if (checkpointSound != null && checkpointSound.clip != null)
        {
            checkpointSound.Play();
        }
    }

    public void PlayLowHealthSound()
    {
        if (playerLowHealthSound != null && playerLowHealthSound.clip != null)
        {
            playerLowHealthSound.loop = true;
            playerLowHealthSound.Play();
        }
    }

    public void StopLowHealthSound()
    {
        playerLowHealthSound.loop = false;
        playerLowHealthSound.Stop();
    }

    public void PlaySwordHit()
    {
        if (swordHit != null && swordHit.clip != null)
        {
            swordHit.Play();
        }
    }

    public void PlaySlimeDeath()
    {
        if (slimeDeath != null && slimeDeath.clip != null)
        {
            slimeDeath.Play();
        }
    }

    public void PlaySlimeHurt()
    {
        if (slimeHurt != null && slimeHurt.clip != null)
        {
            slimeHurt.Play();
        }
    }

    public void PlaySlimeHit()
    {
        if (slimeHit != null && slimeHit.clip != null)
        {
            slimeHit.Play();
        }
    }

    public void PlaySlimeWalk()
    {
        if (slimeWalk != null && slimeWalk.clip != null)
        {
            slimeWalk.loop = true;
            slimeWalk.Play();
        }
    }

    public void StopSlimeWalk()
    {
        slimeWalk.loop = false;
        slimeWalk.Stop();
    }

    public void PlaySlimeRun()
    {
        if (slimeRun != null && slimeRun.clip != null)
        {
            slimeRun.loop = true;
            slimeRun.Play();
        }
    }

    public void StopSlimeRun()
    {
        slimeRun.loop = false;
        slimeRun.Stop();
    }

    public void PlayEnemyDisappear()
    {
        if (enemyDisappear != null && enemyDisappear.clip != null)
        {
            enemyDisappear.Play();
        }
    }

    public void PlayAchievementUnlocked()
    {
        if (achievementUnlocked != null && achievementUnlocked.clip != null)
        {
            achievementUnlocked.Play();
        }
    }
}
