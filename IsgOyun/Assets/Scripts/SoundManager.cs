using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;  // For background music
    public AudioSource sfxSource;    // For sound effects

    [Header("Audio Clips")]
    public AudioClip[] musicClips;  // Array to hold different music tracks
    public AudioClip[] sfxClips;    // Array to hold sound effect clips

    [Header("Volume Settings")]
    [Range(0f, 1f)] public float musicVolume = 0.5f;
    [Range(0f, 1f)] public float sfxVolume = 0.5f;

    private void Awake()
    {
        // Ensure only one instance of SoundManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep the sound manager alive across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Set initial volumes
        musicSource.volume = musicVolume;
        sfxSource.volume = sfxVolume;

        // Optionally, play default music
        PlayMusic(0); // Play first music track as the default background music
    }

    // Play background music
    public void PlayMusic(int trackIndex)
    {
        if (trackIndex >= 0 && trackIndex < musicClips.Length)
        {
            musicSource.clip = musicClips[trackIndex];
            musicSource.Play();
        }
    }

    // Stop music
    public void StopMusic()
    {
        musicSource.Stop();
    }

    // Play a sound effect
    public void PlaySFX(int clipIndex)
    {
        if (clipIndex >= 0 && clipIndex < sfxClips.Length)
        {
            sfxSource.PlayOneShot(sfxClips[clipIndex]);
        }
    }

    // Adjust music volume
    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        musicSource.volume = volume;
    }

    // Adjust SFX volume
    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
        sfxSource.volume = volume;
    }

    public void RandomMusic()
    {
        PlayMusic(Random.Range(0, musicClips.Length));
    }
    
    public void RandomSfx()
    {
        PlaySFX(Random.Range(0, musicClips.Length));
    }
}
