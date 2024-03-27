using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Audio Clips")]
    [SerializeField] public AudioClip bgm;
    [SerializeField] public AudioClip[] footstepSounds;
    [SerializeField] public AudioClip jumpSfx;
    [SerializeField] public AudioClip jumpSfx2;
    [SerializeField] public AudioClip deathSfx;
    [SerializeField] public AudioClip gameOverSfx;
    [SerializeField] public AudioClip winSfx;
    public AudioClip truckEngineSFX;

    [Header("Footstep Settings")]
    [SerializeField] public float minTimeBetweenFootsteps = 0.3f;
    [SerializeField] public float maxTimeBetweenFootsteps = 0.6f;

    private float timeSinceLastFootstep; // Time since the last footstep sound
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

    private void Start()
    {
        PlayMusic(bgm);
    }

    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.Play();
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
            walkStepCount = (walkStepCount + 1) % footstepSounds.Length;
            timeSinceLastFootstep = Time.time; // Update the time since the last footstep sound
        }
    }
}
