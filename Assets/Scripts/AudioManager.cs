using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] private AudioSource musicaSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Audio Clip")]
    public AudioClip bgm;
    public AudioClip[] footstepSounds;
    public AudioClip jumpSfx;
    public AudioClip jumpSfx2;
    public AudioClip deathSfx;
    public AudioClip gameOverSfx;
    public AudioClip winSfx;

    private float timeSinceLastFootstep; // Time since the last footstep sound
    public float minTimeBetweenFootsteps = 0.3f; // Minimum time between footstep sounds
    public float maxTimeBetweenFootsteps = 0.6f; // Maximum time between footstep sounds
    private int walkStepCount = 0;

    public static AudioManager Instance { get; private set; }

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
    // Start is called before the first frame update
    void Start()
    {
        PlayMusic(bgm);
    }

    public void PlayMusic(AudioClip clip)
    {
        musicaSource.clip = clip;
        musicaSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void PlayWalkSFX()
    {
        // Check if enough time has passed to play the next footstep sound
        if (Time.time - timeSinceLastFootstep >= Random.Range(minTimeBetweenFootsteps, maxTimeBetweenFootsteps))
        {
            AudioClip footstepSound = footstepSounds[walkStepCount];
            sfxSource.PlayOneShot(footstepSound);
            walkStepCount++;
            if (walkStepCount > footstepSounds.Length - 1)
            {
                walkStepCount = 0;

            }
            timeSinceLastFootstep = Time.time; // Update the time since the last footstep sound

        }
    }
}
